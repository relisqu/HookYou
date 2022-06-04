using System.Collections;
using UnityEngine;

namespace AI.Bosses.FireBossAttacks
{
    public class MultipliedAttack : CombinedAttack
    {
        [Tooltip("Repeats the first attack RepeatNumber times")]
        [SerializeField]private float RepeatNumber;
        [SerializeField]private float DelayBetweenShots;
        public override IEnumerator StartAttack()
        {
            for (var index = 0; index < RepeatNumber; index++)
            {
                var attack = AttackIngredients[0];
                yield return attack.StartAttack();
                if (index != AttackIngredients.Count - 1) yield return new WaitForSeconds(RepeatNumber);
            }
        }
    }
}