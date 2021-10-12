using UnityEngine;

namespace Destructibility
{
    public class PlayerHealth : Health
    {
        [SerializeField]private Player Player;
        public override void Die()
        {
            Player.Die();
        }
    }
}