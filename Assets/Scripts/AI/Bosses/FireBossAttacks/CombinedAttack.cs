using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Bosses.FireBossAttacks
{
    public class CombinedAttack : Attack
    {
        [SerializeField] private float AttacksDelay;
        [SerializeField]protected List<Attack> AttackIngredients;

        protected Attack _currentAttack;
        public override IEnumerator StartAttack()
        {
            for (var index = 0; index < AttackIngredients.Count; index++)
            {
                var attack = AttackIngredients[index];
                _currentAttack = attack;
                yield return attack.StartAttack();
                if (index != AttackIngredients.Count - 1) yield return new WaitForSeconds(AttacksDelay);
            }
        }
        public override Attack GetCurrentAttack()
        {
            return _currentAttack.GetCurrentAttack();
        }
    }
}