using System;
using System.Collections;
using Assets.Scripts.Old_Scripts;
using UnityEngine;

namespace Assets.Scripts
{
    public class Hook : MonoBehaviour
    {
        [Header("Main Camera")] [SerializeField]
        private Camera Camera;

        [Header("References:")] [SerializeField]
        private Transform PlayerTransform;

        [SerializeField] private SpringJoint2D PlayerSpringJoint2D;
        [SerializeField] private Transform HookPivot;
        [SerializeField] private GrappleRope Rope;

        [Header("Raycast settings:")] [SerializeField]
        public LayerMask HookFocusLayers;

        [SerializeField] public LayerMask HookIgnoreLayers;

        [Header("Distance:")] [SerializeField] private float MaxDistance;

        [Header("Launching")] 
        [Range(0, 3)] [SerializeField] private float LaunchSpeed;
        [SerializeField] private float DropCooldown;
        public float WallHangDuration;
        public float HookReload;
        public HookState CurrentHookState => currentHookState;

        public enum HookState
        {
            NotHooking,
            Hooking,
            OnWall,
            DroppedHook
        }

        private void Start()
        {
            currentHookState = HookState.NotHooking;
            PlayerSpringJoint2D.enabled = false;
            Rope.enabled = false;
        }

        private void Update()
        {
            firePointDistanceVector = Camera.ScreenToWorldPoint(Input.mousePosition) - HookPivot.position;
            if (Input.GetMouseButtonUp(1))
            {
                switch (currentHookState)
                {
                    case HookState.NotHooking:
                        ThrowHook();
                        break;
                    case HookState.Hooking:
                    {
                        if (TrySetupGrapplePoint())
                        {
                            if (wallHangingCoroutine != null) StopCoroutine(wallHangingCoroutine);
                            if (droppingCoroutine != null) StopCoroutine(droppingCoroutine);
                            if (hookingCoroutine != null) StopCoroutine(hookingCoroutine);
                            ThrowHook();
                        }
                        else
                        {
                            if (hookingCoroutine != null) StopCoroutine(hookingCoroutine);
                            if (wallHangingCoroutine != null) StopCoroutine(wallHangingCoroutine);
                            droppingCoroutine = StartCoroutine(DropHook());
                        }
                        break;
                    }
                    case HookState.OnWall:
                    {
                        if (TrySetupGrapplePoint())
                        {
                            if (wallHangingCoroutine != null) StopCoroutine(wallHangingCoroutine);
                            if (droppingCoroutine != null) StopCoroutine(droppingCoroutine);
                            ThrowHook();
                        }

                        break;
                    }
                }
            }
        }

        public Vector3 GetHookDirection()
        {
           return  (grapplePoint - PlayerTransform.position).normalized;
        }

        private IEnumerator MoveToWall()
        {
            currentHookState = HookState.Hooking;
            Vector2 playerPosition = PlayerTransform.position;
            var currentDistance = Vector2.Distance(playerPosition, grapplePoint);
            PlayerSpringJoint2D.distance = currentDistance;
            Rope.enabled = true;
            while (PlayerSpringJoint2D.distance > 0.3f && currentDistance > 0.3f)
            {
                PlayerSpringJoint2D.distance =
                    Mathf.Lerp(PlayerSpringJoint2D.distance, 0.1f, Time.fixedDeltaTime * LaunchSpeed);

                yield return null;
            }

            yield return new WaitForSeconds(0.01f);
            wallHangingCoroutine = StartCoroutine(HangOnWall());
        }

        private IEnumerator HangOnWall()
        {
            Rope.SetHookMovingOnWall();
            currentHookState = HookState.OnWall;
            yield return new WaitForSeconds(WallHangDuration);
            droppingCoroutine = StartCoroutine(DropHook());
        }

        private IEnumerator DropHook()
        {
            currentHookState = HookState.DroppedHook;
            PlayerSpringJoint2D.enabled = false;
            Rope.enabled = false;
            yield return new WaitForSeconds(DropCooldown);
            currentHookState = HookState.NotHooking;
        }


        void ThrowHook()
        {
            if (TrySetupGrapplePoint())
            {
                PlayerSpringJoint2D.connectedAnchor = grapplePoint;
                PlayerSpringJoint2D.enabled = true;
                currentHookState = HookState.Hooking;
                hookingCoroutine = StartCoroutine(MoveToWall());
            }
        }

        bool TrySetupGrapplePoint()
        {
            if (Physics2D.Raycast(HookPivot.position, firePointDistanceVector.normalized, MaxDistance,
                HookFocusLayers))
            {
                var hit = Physics2D.Raycast(HookPivot.position, firePointDistanceVector.normalized, Mathf.Infinity,
                    HookFocusLayers);
                if (hit.collider != null)
                {
                    grapplePoint = hit.point;
                    Rope.SetupLinePoints(HookPivot, grapplePoint);
                    Rope.SetHook();
                    return true;
                }
            }

            return false;
        }

        [SerializeField] private HookState currentHookState;
        private Vector2 firePointDistanceVector;
        private Coroutine hookingCoroutine;
        private Coroutine wallHangingCoroutine;
        private Coroutine droppingCoroutine;
        private Vector3 grapplePoint;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(HookPivot.position, MaxDistance);
        }
    }
}