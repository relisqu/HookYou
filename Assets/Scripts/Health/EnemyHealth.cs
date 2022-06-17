using UnityEngine;

namespace Destructibility
{
    public class EnemyHealth : Health
    {
        private bool isDangerous=true;
        public bool IsDangerous => isDangerous;
        public override void Die()
        {
            Died?.Invoke();
        }

        public void MarkAsDangerous(bool value)
        {
            isDangerous = value;
        }
        
        
        public void SetDangerous()
        {
            isDangerous = true;
        }
        
        public void SetSafe()
        {
            isDangerous = false;
        }
    }
}