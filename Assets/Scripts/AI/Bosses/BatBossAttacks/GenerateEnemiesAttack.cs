using System.Collections;
using System.Collections.Generic;
using Additional_Technical_Settings_Scripts;
using DG.Tweening;
using Player_Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI.BatBossAttacks
{
    public class GenerateEnemiesAttack : Attack
    {
        [SerializeField] private PlayerMovement Player;
        [SerializeField] private List<Transform> Points;

        [SerializeField] private Transform RootTransform;
        [SerializeField] private GameObject Enemy;
        [SerializeField] private float MinDistanceFromPlayer;
        [SerializeField] private float MovementSpeed;

        [BoxGroup("Rotation animation before enemy spawn")] [SerializeField]
        private float RotationSpeed;

        [BoxGroup("Rotation animation before enemy spawn")] [SerializeField]
        private int SpinsAmount;

        [BoxGroup("Rotation animation before enemy spawn")] [SerializeField]
        private float CircleRadius;


        public override IEnumerator StartAttack()
        {
            var point = Points[Random.Range(0, Points.Count)];
            while (Vector2.Distance(point.position, Player.transform.position) < MinDistanceFromPlayer)
            {
                point = Points[Random.Range(0, Points.Count)];
                yield return null;
            }

            var direction = point.position - RootTransform.position;


            yield return RootTransform.DOMove(point.position - CircleRadius * direction.normalized, MovementSpeed)
                .SetSpeedBased()
                .WaitForCompletion();

            for (float i = 0; i < SpinsAmount * 365;)
            {
                var angle = RotationSpeed * 10f * Time.fixedDeltaTime;
                i += angle;
                RootTransform.RotateAround(point.position, Vector3.forward, angle);
                RootTransform.rotation = Quaternion.identity;
                yield return new WaitForFixedUpdate();
            }

            var shooter = Instantiate(Enemy, point);


            TemporaryObjectsCleaner.AddObject(shooter);

            yield return null;
        }
    }
}