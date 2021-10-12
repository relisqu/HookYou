using UnityEngine;

namespace Grappling_Hook.Test
{
    [CreateAssetMenu(fileName = "BossState", menuName = "ScriptableObject/FirstBossStates", order = 0)]
    public class BossPhaseState : ScriptableObject
    {
        [Header("Boss stats:")] public float bossSpeed;

        public float pushPower;
        public float pushTime;
        public float overheatTime;
        public float rotationAngle;

        [Header("Shot stats:")] public int shotAmount;

        public float shotSize;
        public float shotSpeed;
        public float shotDelay;
    }
}