using UnityEngine;

namespace Destructibility
{
    public class EnemyHealth : Health
    {
        public override void Die()
        {
            Died?.Invoke();
        }
    }
}