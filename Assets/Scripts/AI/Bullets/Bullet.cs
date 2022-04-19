using System;
using Destructibility;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    [SerializeField] [FormerlySerializedAs("wallLayer")]
    protected LayerMask WallsLayerMask;
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


    public void SetStats(float speed, float size)
    {
        transform.localScale = Vector3.one * size;
        _currentSpeed = speed;
    }

    private float _currentSpeed;
}