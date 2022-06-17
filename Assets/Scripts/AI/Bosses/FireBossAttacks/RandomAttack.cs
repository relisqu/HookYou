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
            _currentAttack = randomAttack;
            yield return randomAttack.StartAttack();
        }

        public override Attack GetCurrentAttack()
        {
            return _currentAttack.GetCurrentAttack();
        }
    }
}