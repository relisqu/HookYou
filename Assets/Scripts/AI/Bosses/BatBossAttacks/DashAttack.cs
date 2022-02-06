using System.Collections;
using DG.Tweening;
using Player_Scripts;
using Sirenix.Serialization;
using UnityEngine;

namespace AI.Bosses.BatBossAttacks
{
    public class DashAttack : Attack
    {
        private bool _isDashing;
        [SerializeField] private PopupVFX WarningVFX;
        [SerializeField] private PlayerMovement Player;
        [SerializeField] private float PrepareDuration;
        [SerializeField] private float DashRange;
        [SerializeField] private float DashSpeed;

        public override IEnumerator StartAttack()
        {
            _isDashing = true;
            WarningVFX.InitiateObject();
            yield return new WaitForSeconds(PrepareDuration);
            var position = transform.position;
            var playerPosition = Player.transform.position;
            var distance = (playerPosition - position).normalized * DashRange;
            yield return transform.DOMove(position + distance, DashSpeed).SetSpeedBased()
                .SetEase(Ease.OutCubic)
                .OnComplete(() => { _isDashing = false; }).WaitForCompletion();
        }
    }
}