using System;
using Assets.Scripts;
using Destructibility;
using HookBlocks;
using Player_Scripts;
using UnityEngine;

namespace Additional_Technical_Settings_Scripts
{
    public class CursorChange : MonoBehaviour
    {
        [SerializeField] private Texture2D StandardCursor;
        [SerializeField] private Texture2D HookBlockEnableCursor;
        [SerializeField] private Texture2D HookBlockDisableCursor;
        [SerializeField] private Texture2D EnemyEnableCursor;
        private Player _player;
        private Hook _hook;

        private void OnEnable()
        {
            Cursor.SetCursor(StandardCursor, Vector2.one, CursorMode.ForceSoftware);
            _player = FindObjectOfType<Player>();
            _hook = _player.Hook;
        }

        void Update()
        {
            var hit = _hook.GetCurrentHit();


            if (hit.transform == null)
            {
                Cursor.SetCursor(HookBlockDisableCursor, Vector2.one, CursorMode.ForceSoftware);
                return;
            }

            if (!hit.collider.gameObject.TryGetComponent(out HookBlock block)) return;
            Cursor.SetCursor(block.GetType() == typeof(NonStickyBlock) ? HookBlockEnableCursor : EnemyEnableCursor,
                Vector2.one, CursorMode.ForceSoftware);
        }
    }
}