
using Assets.Scripts.LevelCreator;
using Player_Scripts;
using UnityEngine;

namespace Destructibility
{
    public class PlayerHealth : Health
    {
        [SerializeField]private Player Player;
        private LevelManager _levelManager;

        private void Awake()
        {
            _levelManager = FindObjectOfType<LevelManager>();
        }

        public override void Die()
        {
            Player.Die();
            Died?.Invoke();
            Respawn();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R) )
            {
                Die();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                _levelManager.GetCurrentRoom(Player).CompleteLevelAutomatically();
            }
        }
    }
}