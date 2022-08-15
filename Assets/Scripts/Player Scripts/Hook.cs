using System;
using System.Collections;
using Assets.Scripts.Old_Scripts;
using HookBlocks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player_Scripts
{
    public class Hook : MonoBehaviour
    {
        public enum HookState
        {
            NotHooking,
            Hooking,
            Grappling,
            OnWall,
            DroppedHook
        }

        public Action HookTouchedWall { get; set; }

        [Header("Main Camera")] [SerializeField]
        private Camera Camera;


        [Header("References:")] [SerializeField]
        private Transform PlayerTransform;

        [SerializeField] private ParticleSystem ParticleSystem;
        [SerializeField] private Transform ParticleSystemTransform;

        [SerializeField] private SpringJoint2D PlayerSpringJoint2D;

        [SerializeField] private SpringJoint2D HookSpringJoint2D;
        [SerializeField] private Transform HookFinalPivot;
        [SerializeField] private Transform HookStartPivot;
        [SerializeField] private GrappleRope Rope;


        [Header("Raycast settings:")] [SerializeField]
        private LayerMask HookFocusLayers;

        [SerializeField] private float MaxDistance;

        [Header("Launching Constants")] [Range(0, 10)] [SerializeField]
        private float LaunchSpeed;

        [Range(-1, 1)] [SerializeField] private float BreakForceDirection;
        [Range(0, 1)] [SerializeField] private float HookWallStopability;

        [FormerlySerializedAs("BreakForceRequiredTime")] [SerializeField]
        private float BreakRequiredTime;

        [SerializeField] private float DropDuration;
        [SerializeField] private float WallHangDuration;

        public HookState CurrentHookState { get; private set; }


        private void Start()
        {
            hookEndDefaultParent = HookFinalPivot.parent;
            CurrentHookState = HookState.NotHooking;
            PlayerSpringJoint2D.enabled = false;
            HookSpringJoint2D.enabled = false;
            Rope.enabled = false;
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(1) && TryGetCurrentSelectedTarget())
            {
                switch (CurrentHookState)
                {
                    case HookState.NotHooking:
                        ThrowHook();
                        break;
                    case HookState.Hooking:
                        break;
                    case HookState.Grappling:
                        break;
                    case HookState.OnWall:
                        ThrowHook();
                        break;
                    case HookState.DroppedHook:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (CurrentHookState == HookState.NotHooking) return;
            CalculateDropTime();
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(HookStartPivot.position, MaxDistance);
        }

        public void SetPlayerWalkingMovement(Vector3 movement)
        {
            playerMovement = movement;
        }

        private bool _isHookingObject;

        public void ThrowHook()
        {
            if (TrySetupGrapplePoint())
            {
                if (wallHangingCoroutine != null) StopCoroutine(wallHangingCoroutine);
                if (droppingCoroutine != null) StopCoroutine(droppingCoroutine);
                if (hookingCoroutine != null) StopCoroutine(hookingCoroutine);
                if (_isHookingObject)
                {
                    HookSpringJoint2D.enabled = true;
                    ((DefaultPushableBlock) currentBlock).AddActivitiesAtHookStart();
                    hookingCoroutine = StartCoroutine(MoveToWall(HookSpringJoint2D));
                    HookSpringJoint2D.connectedBody = currentHit.rigidbody;
                    CurrentHookState = HookState.Grappling;
                }
                else
                {
                    PlayerSpringJoint2D.enabled = true;
                    hookingCoroutine = StartCoroutine(MoveToWall(PlayerSpringJoint2D));
                    CurrentHookState = HookState.Hooking;
                }

                HookTouchedWall?.Invoke();
            }
        }

        private IEnumerator MoveToWall(SpringJoint2D springJoint2D)
        {
            Vector2 playerPosition = PlayerTransform.position;
            var currentDistance = Vector2.Distance(playerPosition, grapplePoint);
            springJoint2D.distance = currentDistance;
            Rope.enabled = true;
            var speed = currentBlock.RequiresSpecificHookSpeed() ? currentBlock.GetHookShotSpeed() : LaunchSpeed;
            while (springJoint2D.distance > HookWallStopability / 1.2f && currentDistance > HookWallStopability)
            {
                springJoint2D.distance =
                    Mathf.Lerp(springJoint2D.distance, 0.1f, Time.deltaTime * speed);

                yield return null;
            }

            yield return new WaitForSeconds(0.01f);
            currentBlock.TouchTheBlock(this);
        }


        private bool TrySetupGrapplePoint()
        {
            if (currentHit.collider != null && isAbleToHook)
            {
                grapplePoint = currentHit.point;
                HookFinalPivot.transform.position = grapplePoint;
                HookFinalPivot.transform.parent = currentHit.transform;
                Rope.SetupLinePoints(HookStartPivot, HookFinalPivot);
                Rope.SetHook();
                return true;
            }

            return false;
        }

        public bool TryGetCurrentSelectedTarget()
        {
            var position = HookStartPivot.position;
            HookToMouseDirection = Camera.ScreenToWorldPoint(Input.mousePosition) - position;
            isAbleToHook = Physics2D.Raycast(position, HookToMouseDirection.normalized, MaxDistance,
                HookFocusLayers);
            if (isAbleToHook)
            {
                currentHit = Physics2D.Raycast(position, HookToMouseDirection.normalized,
                    Mathf.Infinity,
                    HookFocusLayers);
                var foundComponent = currentHit.transform.gameObject.TryGetComponent(out HookBlock block);
                _isHookingObject = block.GetType() != typeof(StickyBlock);
                currentBlock = block;
                AudioManager.instance.Play("hook_hit");
                return foundComponent;
            }
            
            AudioManager.instance.Play("hook_fail");
            ParticleSystemTransform.transform.parent = null;
            ParticleSystemTransform.transform.position  = Camera.ScreenToWorldPoint(Input.mousePosition)+Vector3.forward*30;
            ParticleSystem.Play();


            return isAbleToHook;
        }

        public void DropHook()
        {
            if (hookingCoroutine != null) StopCoroutine(hookingCoroutine);
            if (wallHangingCoroutine != null) StopCoroutine(wallHangingCoroutine);
            if (droppingCoroutine != null) StopCoroutine(droppingCoroutine);
            droppingCoroutine = StartCoroutine(DropHookEnumerator());
        }

        public Vector3 GetHookDirection()
        {
            return (HookFinalPivot.position - HookStartPivot.position).normalized;
        }

        private void CalculateDropTime()
        {
            if (Vector3.Dot(playerMovement, GetHookDirection().normalized) < BreakForceDirection)
            {
                if (isTryingToBreakHook)
                {
                    currentBreakTime += Time.deltaTime;
                }
                else
                {
                    isTryingToBreakHook = true;
                    currentBreakTime = 0;
                }
            }
            else
            {
                currentBreakTime = 0;
                isTryingToBreakHook = false;
            }

            if (currentBreakTime > BreakRequiredTime) DropHook();
        }

        public void HangOnWall()
        {
            if (hookingCoroutine != null) StopCoroutine(hookingCoroutine);
            if (wallHangingCoroutine != null) StopCoroutine(wallHangingCoroutine);
            if (droppingCoroutine != null) StopCoroutine(droppingCoroutine);
            wallHangingCoroutine = StartCoroutine(HangOnWallIEnumerator());
        }

        private IEnumerator DropHookEnumerator()
        {
            if (CurrentHookState == HookState.Grappling)
            {
                ((DefaultPushableBlock) currentBlock).OnHookBreak();
            }

            CurrentHookState = HookState.DroppedHook;
            PlayerSpringJoint2D.enabled = false;
            HookSpringJoint2D.enabled = false;
            Rope.enabled = false;
            currentBreakTime = 0;
            isTryingToBreakHook = false;
            yield return new WaitForSeconds(DropDuration);
            CurrentHookState = HookState.NotHooking;
        }

        public void ClearHook()
        {
            PlayerSpringJoint2D.enabled = false;
            HookSpringJoint2D.enabled = false;
            Rope.enabled = false;
            currentBreakTime = 0;
            isTryingToBreakHook = false;
            CurrentHookState = HookState.NotHooking;
        }

        private IEnumerator HangOnWallIEnumerator()
        {
            Rope.SetHookMovingOnWall();
            CurrentHookState = HookState.OnWall;
            yield return new WaitForSeconds(WallHangDuration);
            droppingCoroutine = StartCoroutine(DropHookEnumerator());
        }


        public Transform GetPlayerTransform()
        {
            return PlayerTransform;
        }

        private float currentBreakTime;
        private RaycastHit2D currentHit;
        private Coroutine droppingCoroutine;
        private Vector3 grapplePoint;
        private Transform hookEndDefaultParent;
        private Coroutine hookingCoroutine;
        private Vector2 HookToMouseDirection;
        private bool isAbleToHook;
        private bool isTryingToBreakHook;
        private Vector3 playerMovement;
        private Coroutine wallHangingCoroutine;
        private HookBlock currentBlock;

        public bool IsInHookRadius(Transform obj)
        {
            return Vector2.Distance(obj.position, transform.position) <= MaxDistance;
        }
    }
}