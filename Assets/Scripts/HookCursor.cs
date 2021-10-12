using System;
using Player_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HookCursor : MonoBehaviour
{
    [SerializeField] private Hook Hook;
    [SerializeField] private GameObject Cursor;

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("FirstLocation");
        }
    }
}