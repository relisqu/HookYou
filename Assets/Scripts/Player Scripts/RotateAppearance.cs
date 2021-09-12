using UnityEngine;

public class RotateAppearance : MonoBehaviour
{
    public Animator PlayerAnimator;
    public Camera Camera;
    private static readonly int XDirection = Animator.StringToHash("xDirection");
    private static readonly int YDirection = Animator.StringToHash("yDirection");

    void FixedUpdate()
    {
        var lookRotation = (Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
    }
}