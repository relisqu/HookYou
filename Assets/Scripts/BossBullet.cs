using System;
using System.Collections;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private float speed;
    public bool isDamaging;
    public LayerMask wallLayer;
    public static int bulletAmount = 0;

    private void Awake()
    {
        bulletAmount += 1;
    }

    private void OnDestroy()
    {
        bulletAmount -= 1;
    }

    public void SetStats(float speed, float size)
    {
        isDamaging = true;
        transform.localScale = Vector3.one * size;
        this.speed = speed;
    }

    public void FixedUpdate()
    {
        transform.position += transform.up * (Time.fixedDeltaTime * speed);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (wallLayer == (wallLayer | (1 << other.gameObject.layer)))
        {
            gameObject.SetActive(false);
        }
    }
}