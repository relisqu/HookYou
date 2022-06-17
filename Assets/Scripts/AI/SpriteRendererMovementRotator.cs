using UnityEngine;

namespace AI
{
    public class SpriteRendererMovementRotator : MonoBehaviour
    {
        private Vector3 previousPosition;
        private bool _isRotatedLeft = true;

        private void OnEnable()
        {
            previousPosition = transform.position;
        }

        private void FixedUpdate()
        {
            var currentPosition = transform.position;
            var isCurrentlyRotatedLeft = previousPosition.x > currentPosition.x;
            if (isCurrentlyRotatedLeft == _isRotatedLeft && Mathf.Abs(previousPosition.x - currentPosition.x) > 0.001f)
            {
                var sign = isCurrentlyRotatedLeft ? 1f : -1f;
                transform.localScale = new Vector3(sign * 1f, transform.localScale.y, transform.localScale.z);
            }

            previousPosition = currentPosition;
            _isRotatedLeft = isCurrentlyRotatedLeft;
        }
    }
}