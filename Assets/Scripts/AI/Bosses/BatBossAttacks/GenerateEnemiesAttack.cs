using System.Collections;
using System.Collections.Generic;
using Additional_Technical_Settings_Scripts;
using AI.Bosses.BatBossAttacks;
using DG.Tweening;
using Player_Scripts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI.BatBossAttacks
{
    public class GenerateEnemiesAttack : Attack
    {
        [SerializeField] private PlayerMovement Player;
        [SerializeField] private List<ShroomPoint> Points;

        [SerializeField] private Transform RootTransform;
        [SerializeField] private Vector3 RotationOffset;
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
            if (point.IsMushroomAlive()) yield break;
                
            PlayAttackAnimation();
            var pointDistance = RotationOffset + point.transform.position;
            
            while (Vector2.Distance(pointDistance, Player.transform.position) < MinDistanceFromPlayer)
            {
                point = Points[Random.Range(0, Points.Count)];
                pointDistance = RotationOffset + point.transform.position;
                yield return null;
            }

            var direction = pointDistance - RootTransform.position;


            yield return RootTransform.DOMove(pointDistance - CircleRadius * direction.normalized, MovementSpeed)
                .SetSpeedBased()
                .WaitForCompletion();

            for (float i = 0; i < SpinsAmount * 365;)
            {
                var angle = RotationSpeed * 10f * Time.fixedDeltaTime;
                i += angle;
                RootTransform.RotateAround(pointDistance, Vector3.forward, angle);
                RootTransform.rotation = Quaternion.identity;
                yield return new WaitForFixedUpdate();
            }
            if (point.IsMushroomActive())
            {
                point.RespawnShroom();
            }
            else
            {
                point.SpawnShroom();
                
            }

            //TemporaryObjectsCleaner.AddObject(shooter);

            yield return null;
        }
        
        public override Attack GetCurrentAttack()
        {
            return this;
        }
    }
}