using System.Collections.Generic;
using Destructibility;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AI
{
    public abstract class BossStage : MonoBehaviour
    {
        [SerializeField] private BossStage NextStage;
        [TextArea] [Tooltip("Comments shown in inspector")]
        public string Notes;
        public abstract void Attack();
        public abstract void StopCurrentAttack();

        public void MoveToNextStage()
        {
            StopCurrentAttack();
            if (NextStage != null) NextStage.Attack();
        }

        public BossStage GetNextStage()
        {
            return NextStage;
        }

     
    }
}