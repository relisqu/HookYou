using UnityEngine;

public class RotateAppearance : MonoBehaviour
{
    private static readonly int XDirection = Animator.StringToHash("xDirection");
    private static readonly int YDirection = Animator.StringToHash("yDirection");
    public Animator PlayerAnimator;
    public Camera Camera;

    private void FixedUpdate()
    {
        var lookRotation = (Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
    }
}