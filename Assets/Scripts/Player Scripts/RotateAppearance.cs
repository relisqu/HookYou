using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RotateAppearance : MonoBehaviour
{
    [FormerlySerializedAs("camera")] [SerializeField] private Camera Camera;

    private void Start()
    {
    }

    private void Update()
    {
        //if (!Input.GetMouseButtonDown(0)) return;
        var lookRotation = (Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -Vector2.SignedAngle(lookRotation, Vector3.right)));
    }

    private void OnDrawGizmos()
    {
        var mousePos = Input.mousePosition;
        mousePos.z=Camera.nearClipPlane;
        Gizmos.DrawCube(Camera.ScreenToWorldPoint(mousePos), Vector3.one);
    }
}