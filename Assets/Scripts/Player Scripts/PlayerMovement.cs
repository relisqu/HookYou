using System;
using Assets.Scripts;
using UnityEngine;

namespace Player_Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private SwordAttack Sword;
        [SerializeField] private Hook Hook;

        [Header("Default speed constrains:")] [SerializeField]
        private float DefaultWalkingSpeed;

        [Range(0, 10)] [SerializeField] private float HookFlyingMultiplier;
        [Range(0, 1)] [SerializeField] private float SwordSlowDownMultiplier;

        private Vector2 movement;
        private bool isMoving;
        [SerializeField]private Rigidbody2D rigidbody2D;
        private bool previouslyMoved;
        public Vector2 GetMovement => movement;
        public bool IsMoving => isMoving;


        private void Update()
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement.Normalize();
            Hook.SetPlayerWalkingMovement(movement);
            isMoving = movement.sqrMagnitude > 0;
            rigidbody2D.velocity = movement * GetCurrentSpeed();
            Debug.Log(rigidbody2D.velocity);
            switch (isMoving)
            {
                case true:
                    if (!previouslyMoved) AudioManager.instance.Play("walk");

                    break;
                case false:
                    AudioManager.instance.StopPlaying("walk");
                    break;
            }

            previouslyMoved = isMoving;
        }
 

        public float GetCurrentSpeed()
        {
            var currentSpeed = DefaultWalkingSpeed;
            if (Hook.CurrentHookState == Hook.HookState.Hooking)
            {
                currentSpeed *= HookFlyingMultiplier;
            }

            if (Sword.StartedAttack)
            {
                currentSpeed *= SwordSlowDownMultiplier;
            }

            return currentSpeed;
        }
    }
}