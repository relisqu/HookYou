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
        [SerializeField] private Texture2D EnemyDisableCursor;
        [SerializeField] private LayerMask CursorHitLayers;
        [SerializeField] private Player Player;
        [SerializeField] private SwordAttack Sword;
        [SerializeField]private Camera Camera;

        private void OnEnable()
        {
            Camera = Camera.main;
            Cursor.SetCursor(StandardCursor, Vector2.one, CursorMode.ForceSoftware);
        }

        void Update()
        {
            var hit= Physics2D.Raycast(new Vector2(Camera.ScreenToWorldPoint(Input.mousePosition).x,Camera.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0f);
            
            if (hit.collider == null)
            {
                Cursor.SetCursor(StandardCursor, Vector2.one, CursorMode.ForceSoftware);
                return;
            }
            
            if (hit.transform.gameObject.TryGetComponent(out EnemyHealth enemy))
            {
                if (Sword.IsInAttackRange(enemy.transform))
                {
                    Cursor.SetCursor(EnemyEnableCursor, Vector2.one, CursorMode.ForceSoftware);
                }
                else
                {
                    Cursor.SetCursor(EnemyDisableCursor, Vector2.one, CursorMode.ForceSoftware);
                }
            }
            else if (hit.transform.gameObject.TryGetComponent(out HookBlock block))
            {
                if (Player.Hook.IsInHookRadius(block.transform))
                {
                    Cursor.SetCursor(HookBlockEnableCursor, Vector2.one, CursorMode.ForceSoftware);
                    
                }
                else
                {
                    Cursor.SetCursor(HookBlockDisableCursor, Vector2.one, CursorMode.ForceSoftware);
                }
            }
        }
    }
}