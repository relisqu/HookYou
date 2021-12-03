using System;
using System.Collections;
using DG.Tweening;
using Player_Scripts;
using UnityEngine;

namespace AI
{
    public class DashingAI : MonoBehaviour
    {
        [SerializeField] private PlayerMovement Player;
        [SerializeField] private float Speed;
        [SerializeField] private float AttackRadius;
        [SerializeField] private float DeathImpulse;
        [Tooltip("How far the AI will fly while dashing")][SerializeField] private float DashRange;
        [Tooltip("How fast the AI will fly while dashing")][SerializeField] private float DashSpeed;
        [SerializeField] private float PauseDuration;
        private bool _isAttacking;
        private void Update()
        {
            if(_isAttacking) return;
            var distance = Vector2.Distance(Player.transform.position, transform.position);
            if (distance > AttackRadius)
            { 
                var newDistance= Vector2.MoveTowards(transform.position,Player.transform.position,Speed*Time.deltaTime);
                transform.position = newDistance;
            }
            else
            {
                StartCoroutine(Dash());
            }
        }

        IEnumerator Dash()
        {
            _isAttacking = true;
            yield return new WaitForSeconds(PauseDuration);
            var position = transform.position;
            var playerPosition = Player.transform.position;
            var newPosition = (playerPosition - position) * DashRange + position;
            transform.DOMove(newPosition, 1/DashSpeed).SetEase(Ease.OutCubic).OnComplete(() => { _isAttacking = false;});
            
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, AttackRadius);
        }
    }
}