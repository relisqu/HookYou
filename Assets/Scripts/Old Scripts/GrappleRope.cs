﻿using UnityEngine;

namespace Assets.Scripts.Old_Scripts
{
    public class GrappleRope : MonoBehaviour
    {
        [Header("General refrences:")] [SerializeField]
        LineRenderer m_lineRenderer;

        [SerializeField] private GameObject HookSprite;
        [SerializeField] private Animator HookAnimator;

        [Header("General Settings:")] [SerializeField]
        private int percision = 20;

        [SerializeField] public bool isGrappling = false;

        bool drawLine = true;
        bool straightLine = true;

        private Transform grapplePoint;
        private Vector3 firePoint;
        private static readonly int PlayerOnWall = Animator.StringToHash("PlayerOnWall");

        private void Awake()
        {
            m_lineRenderer = GetComponent<LineRenderer>();
            m_lineRenderer.enabled = false;
            m_lineRenderer.positionCount = percision;
            HookSprite.SetActive(false);
        }

        public void SetupLinePoints(Transform grapplePoint, Vector3 firePoint)
        {
            this.grapplePoint = grapplePoint;
            this.firePoint = firePoint;
        }

        public void SetHook()
        {
            var moveDirection = grapplePoint.position - firePoint;
            if (moveDirection == Vector3.zero) return;
            var angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            HookSprite.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            HookSprite.transform.position = firePoint;
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

        void LinePointToFirePoint()
        {
            for (int i = 0; i < percision; i++)
            {
                m_lineRenderer.SetPosition(i, firePoint);
            }
        }

        void Update()
        {
            if (drawLine)
            {
                DrawRope();
            }
        }

        void DrawRope()
        {
            if (!isGrappling)
            {
                isGrappling = true;
            }

            DrawRopeNoWaves();
        }


        void DrawRopeNoWaves()
        {
            m_lineRenderer.positionCount = 2;
            m_lineRenderer.SetPosition(0, grapplePoint.position);
            m_lineRenderer.SetPosition(1, firePoint);
        }

        public void SetHookMovingOnWall()
        {
            HookAnimator.SetTrigger(PlayerOnWall);
        }
    }
}