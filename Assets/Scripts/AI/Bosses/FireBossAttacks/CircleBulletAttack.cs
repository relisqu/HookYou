using System.Collections;
using System.Collections.Generic;
using AI.Bullets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI
{
    public class CircleBulletAttack : Attack
    {
        [BoxGroup("References")] [SerializeField]
        private ShootingModule ShootingModule;

        [BoxGroup("Circle bullet attack")] [SerializeField]
        private int CircleBulletCount;

        [BoxGroup("Circle bullet attack")] [SerializeField]
        private float CircleBulletSpeed;

        public override IEnumerator StartAttack()
        {
            for (int i = 0; i < CircleBulletCount; i++)
                ShootingModule.Shoot<StandardBullet>(CircleBulletSpeed, 1,
                    Quaternion.AngleAxis(i * 365 / CircleBulletCount, Vector3.forward));
            yield return null;
        }
    }
}