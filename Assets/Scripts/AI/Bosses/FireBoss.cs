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


        [BoxGroup("First Phase")] [SerializeField]
        private int AttacksCount;


        [SerializeField] private BossStage InitialStage;

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

        private float GetAngleBetweenTwoPoints(Vector3 a, Vector3 b)
        {
            return Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
        }


        void GroundAttack()
        {
        }

        private void Start()
        {
            InitialStage.Attack();
            _currentPhase = InitialStage;
            Health.TookDamage += _currentPhase.MoveToNextStage;
        }

        private BossStage _currentPhase;
    }
}