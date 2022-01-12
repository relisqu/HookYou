using System;
using Destructibility;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    [SerializeField] [FormerlySerializedAs("wallLayer")]
    private LayerMask WallsLayerMask;
    [SerializeField] private float DefaultSpeed;

    [SerializeField] public Health Health;
    public void FixedUpdate()
    {
        transform.position += transform.up * (Time.fixedDeltaTime * _currentSpeed);
    }

    private void Awake()
    {
        _currentSpeed = DefaultSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (WallsLayerMask == (WallsLayerMask | (1 << other.gameObject.layer)))
        {
            Health.TakeDamage(1);
        }
    }

    public void SetStats(float speed, float size)
    {
        transform.localScale = Vector3.one * size;
        _currentSpeed = speed;
    }

    private float _currentSpeed;
}