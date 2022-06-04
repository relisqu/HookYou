using System.Collections;
using AI.Bosses.FireBossAttacks;
using UnityEngine;

namespace AI
{
    public class RandomAttack : CombinedAttack
    {
        public override IEnumerator StartAttack()
        {
            var randomAttack = AttackIngredients[Random.Range(0, AttackIngredients.Count)];
            yield return randomAttack.StartAttack();
        }
    }
}