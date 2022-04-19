using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace AI.Bosses.Attacks
{
    public class GroundAttack : Attack
    {
        [SerializeField] private GameObject ShockWave;
        [SerializeField] private float Radius;
        [SerializeField] [Range(0.1f, 5f)] private float Speed;

        public override IEnumerator StartAttack()
        {
            ShockWave.transform.localScale = Vector3.zero;
            ShockWave.SetActive(true);
            yield return ShockWave.transform.DOScale(Radius * Vector3.one, Speed).SetSpeedBased().WaitForCompletion();
            ShockWave.SetActive(false);
        }
    }
}