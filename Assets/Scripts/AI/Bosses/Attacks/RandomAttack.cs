using System.Collections;
using UnityEngine;

namespace AI
{
    public class RandomAttack : Attack
    {
        public override IEnumerator StartAttack()
        {
            var randomAttack = AttackIngredients[Random.Range(0, AttackIngredients.Count)];
            yield return randomAttack.StartAttack();
        }
    }
}