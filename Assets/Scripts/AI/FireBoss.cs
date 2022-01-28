using System;
using System.Collections;
using AI.Bullets;
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

        [FormerlySerializedAs("BulletCount")] [BoxGroup("Circle bullet attack")] [SerializeField]
        private int CircleBulletCount;

        [FormerlySerializedAs("BulletSpeed")] [BoxGroup("Circle bullet attack")] [SerializeField]
        private float CircleBulletSpeed;

        [BoxGroup("Player directed attack")] [SerializeField]
        private int DirectedBulletCount;


        [BoxGroup("Player directed attack")] [SerializeField]
        private float DirectedBulletSpeed;

        [BoxGroup("Player directed attack")] [SerializeField]
        private float DelayBetweenShots;

        void AttackA()
        {
            StartCoroutine(AttackLookingAtPlayer(DirectedBulletCount, DelayBetweenShots, DirectedBulletSpeed));
        }

        IEnumerator BossBehaviour()
        {
            while (true)
            {
                AttackA();
                yield return new WaitForSeconds(1.5f);
                AttackAPlus();
                yield return new WaitForSeconds(1.5f);
                CircleBulletAttack();
                yield return new WaitForSeconds(0.8f);
            }
            yield return null;
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

        void AttackAPlus()
        { 
            StartCoroutine(AttackLookingAtPlayer(DirectedBulletCount, DelayBetweenShots*0.5f, DirectedBulletSpeed*1.5f));
        }

        void GroundAttack()
        {
        }

        private void Start()
        {
            StartCoroutine(BossBehaviour());
        }

        void CircleBulletAttack()
        {
            for (int i = 0; i < CircleBulletCount; i++)
                ShootingModule.Shoot<StandardBullet>(CircleBulletSpeed, 1,
                    Quaternion.AngleAxis(i * 365 / CircleBulletCount, Vector3.forward));
        }
    }
}