using System.Collections;
using AI.Bullets;
using Player_Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI
{
    public class HomingShootAttack : Attack
    {
        [BoxGroup("References")] [SerializeField]
        private ShootingModule ShootingModule;

        [BoxGroup("References")] [SerializeField]
        private PlayerMovement Player;

        [BoxGroup("Player directed attack")] [SerializeField]
        private int DirectedBulletCount;
        
        [BoxGroup("Player directed attack")] [SerializeField]
        private float DirectedBulletSpeed;

        [BoxGroup("Player directed attack")] [SerializeField]
        private float DelayBetweenShots;

        public override IEnumerator StartAttack()
        {
            yield return StartCoroutine(AttackLookingAtPlayer(DirectedBulletCount, DelayBetweenShots, DirectedBulletSpeed));
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
    }
}