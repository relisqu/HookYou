using System;
using UnityEngine;

public class RotateAppearance : MonoBehaviour
{
    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        var lookRotation = (camera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -Vector2.SignedAngle(lookRotation, Vector3.right)));
    }
}