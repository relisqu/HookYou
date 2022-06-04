using System.Collections;
using System.Collections.Generic;
using Destructibility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI
{
    public class FireBossStage : BossStage
    {
        [SerializeField] private Attack StartAttack;
        [SerializeField] private List<Attack> AttackOrder;
        [SerializeField] private float DelayBetweenAttacks;
        [SerializeField] private bool IsImmuneToDamage;

        [ShowIf("IsImmuneToDamage")] [BoxGroup("References")] [SerializeField]
        private SwordDestructible SwordDestructible;

        public override void Attack()
        {
            if (IsImmuneToDamage) SwordDestructible.SetImmuneToDamage(IsImmuneToDamage);

            StartCoroutine(PhaseAttack());
        }

        IEnumerator PhaseAttack()
        {
            if (StartAttack != null) yield return StartAttack.StartAttack();
            while (true)
            {
                foreach (var attack in AttackOrder)
                {
                    yield return attack.StartAttack();
                    yield return new WaitForSeconds(DelayBetweenAttacks);
                }
            }

            yield return null;
        }

        public override void StopCurrentAttack()
        {
            StopAllCoroutines();
            if (IsImmuneToDamage) SwordDestructible.SetImmuneToDamage(!IsImmuneToDamage);
        }
    }
}