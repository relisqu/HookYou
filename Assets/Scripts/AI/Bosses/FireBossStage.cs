using System;
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
        [SerializeField] public bool ChangesOtherBossPhaseOnComplete;

        [BoxGroup("References")] [SerializeField]
        private SwordDestructible SwordDestructible;

        private void Start()
        {
            SwordDestructible = GetComponentInParent<SwordDestructible>();
        }

        public override void Attack()
        {
            print("Set "+SwordDestructible+" to "+ IsImmuneToDamage);

            StartCoroutine(PhaseAttack());
        }

        IEnumerator PhaseAttack()
        {
            print("Started phase: "+name);
            yield return new WaitForSeconds(0.1f);
            SwordDestructible.SetImmuneToDamage(IsImmuneToDamage);
            if (StartAttack != null)
            {
                _currentAttack = StartAttack;
                yield return StartAttack.StartAttack();
            }
            while (true)
            {
                foreach (var attack in AttackOrder)
                {
                    _currentAttack = attack;
                    yield return attack.StartAttack();
                    yield return new WaitForSeconds(DelayBetweenAttacks);
                }

                yield return null;
            }

            yield return null;
        }

        private Attack _currentAttack;
        public override void StopCurrentAttack()
        {
            StopAllCoroutines();
        }

        public Attack GetCurrentAttack()
        {
            return _currentAttack;
        }

    }
}