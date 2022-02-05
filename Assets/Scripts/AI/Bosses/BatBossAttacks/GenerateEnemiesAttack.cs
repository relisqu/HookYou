using System.Collections;
using System.Collections.Generic;
using Additional_Technical_Settings_Scripts;
using Player_Scripts;
using UnityEngine;

namespace AI.BatBossAttacks
{
    public class GenerateEnemiesAttack : Attack
    {
        [SerializeField] private PlayerMovement Player;
        [SerializeField] private List<Transform> Points;

        [SerializeField] private GameObject Enemy;
        [SerializeField] private float MinDistanceFromPlayer;


        public override IEnumerator StartAttack()
        {
            foreach (var point in Points)
            {
                if (Vector2.Distance(point.position, Player.transform.position) > MinDistanceFromPlayer)
                {
                    var shooter = Instantiate(Enemy, point);
                    TemporaryObjectsCleaner.AddObject(shooter);
                    yield break;
                }
                
            }

            yield return null;
        }
    }
}