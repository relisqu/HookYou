using System;
using System.Collections;
using AI.Bullets;
using Destructibility;
using Player_Scripts;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace AI
{
    public class FireBoss : EnemyBehaviour
    {
        [BoxGroup("References")] [SerializeField]
        private ShootingModule ShootingModule;

        [BoxGroup("References")] [SerializeField]
        private PlayerMovement Player;

        [BoxGroup("References")] [SerializeField]
        private FireBoss OtherBoss;

        //[BoxGroup("References")] [SerializeField]
        //private Health Health;


        [SerializeField] private FireBossStage InitialStage;

        public override void ShowStunEffect()
        {
        }

        IEnumerator BeInStun()
        {
            SetStunned();
            isStunned = true;
            yield return new WaitForSeconds(12);
            isStunned = false;
        }

        IEnumerator AttackLookingAtPlayer(int bulletCount, float bulletDelay, float bulletSpeed)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                var playerPos = Player.transform.position;
                var bulletRequiredTime = Vector3.Distance(transform.position, playerPos) / bulletSpeed;
                var predictedPlayerPos = (Vector2) playerPos +
                                         Player.GetMovement * (Player.GetCurrentSpeed() * bulletRequiredTime);

                var angle = ShootingModule.GetAngleBetweenTwoPoints(transform.position, predictedPlayerPos);
                var rotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 90));

                ShootingModule.Shoot<TransparentBullet>(bulletSpeed, 1, rotation);
                yield return new WaitForSeconds(bulletDelay);
            }
        }


        public void StartAttacks()
        {
            InitialStage.Attack();
            _currentPhase = InitialStage;
            Health.TookDamage += ChangeAllBossPhases;
        }

        private void Start()
        {
        }

        void ChangePhase()
        {
            if (_currentPhase.GetNextStage() == null)
            {
                Health.Animator.PlayDeathAnimation();
                Health.Die();
                return;
                
            }

            if (_currentPhase != null)
            {
                _currentPhase.MoveToNextStage();
                _currentPhase = (FireBossStage)_currentPhase.GetNextStage();
            }
            
        }

        private void ChangeAllBossPhases()
        {
            var phaseRequiresOtherBossChange = _currentPhase.ChangesOtherBossPhaseOnComplete;
            ChangePhase();
            if(phaseRequiresOtherBossChange && OtherBoss!=null) OtherBoss.ChangePhase();
        }

        private FireBossStage _currentPhase;
    }
}