using UnityEngine;

namespace Assets.Scripts.Old_Scripts
{
    public class GrappleRope : MonoBehaviour
    {
        private static readonly int PlayerOnWall = Animator.StringToHash("PlayerOnWall");

        [Header("General refrences:")] [SerializeField]
        private LineRenderer m_lineRenderer;

        [SerializeField] private GameObject HookSprite;
        [SerializeField] private Animator HookAnimator;

        [Header("General Settings:")] [SerializeField]
        private int percision = 20;

        [SerializeField] public bool isGrappling;

        private readonly bool drawLine = true;
        private Transform firePoint;

        private Transform grapplePoint;
        private bool straightLine = true;

        private void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            m_lineRenderer.enabled = false;
            m_lineRenderer.positionCount = percision;
            HookSprite.SetActive(false);
        }

        private void Update()
        {
            if (drawLine) DrawRope();
        }

        private void OnEnable()
        {
            m_lineRenderer.enabled = true;
            m_lineRenderer.positionCount = percision;
            straightLine = true;
            LinePointToFirePoint();
            HookSprite.SetActive(true);
        }


        private void OnDisable()
        {
            if (HookSprite != null) HookSprite.SetActive(false);
            m_lineRenderer.enabled = false;
            isGrappling = false;
        }

        public void SetupLinePoints(Transform grapplePoint, Transform firePoint)
        {
            this.grapplePoint = grapplePoint;
            this.firePoint = firePoint;
            SetHook();
        }

        public void SetHook()
        {
            var moveDirection = grapplePoint.position - firePoint.position;
            if (moveDirection == Vector3.zero) return;
            var angle = ((int)Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg/45)*45;
            var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            HookSprite.transform.rotation = rotation;
            HookSprite.transform.position = firePoint.position;
        }

        private void LinePointToFirePoint()
        {
            if (m_lineRenderer == null || firePoint == null) return;
            for (var i = 0; i < percision; i++) m_lineRenderer.SetPosition(i, firePoint.position);
        }

        private void DrawRope()
        {
            if (!isGrappling) isGrappling = true;

            DrawRopeNoWaves();
        }


        private void DrawRopeNoWaves()
        {
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPosition(0, grapplePoint.position);
            m_lineRenderer.SetPosition(1, firePoint.position);
            HookSprite.transform.position = firePoint.position;
        }

        public void SetHookMovingOnWall()
        {
            HookAnimator.SetTrigger(PlayerOnWall);
        }
    }
}