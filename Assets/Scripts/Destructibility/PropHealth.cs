using UnityEngine;

namespace Destructibility
{
    public class PropHealth: Health
    {
        public override void Die()
        {
           // gameObject.SetActive(false);
           Died?.Invoke();
           
        }
    }
}