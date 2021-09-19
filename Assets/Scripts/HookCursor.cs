using System;
using Assets.Scripts;
using UnityEngine;

public class HookCursor : MonoBehaviour
{
    [SerializeField] private Hook Hook;
    [SerializeField] private GameObject Cursor;

    private void FixedUpdate()
    {
        var hit = Hook.GetRaycastHit();
        if (hit.collider != null && Hook.IsAbleToHook())
        {
            Cursor.SetActive(true);
            Cursor.transform.position = hit.point;
        }
        else
        {
            Cursor.SetActive(false);
        }
    }
}