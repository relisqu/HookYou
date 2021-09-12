using System;
using System.Collections;
using System.Collections.Generic;
using Grappling_Hook.Test;
using UnityEngine;

public class CanonModule : MonoBehaviour
{
    public BossBullet bulletObject;
    public Transform shootingPosition;

    private List<BossBullet> bullets;
    public int amountToPool;

    void Start()
    {
        bullets = new List<BossBullet>();
        BossBullet tmp;
         for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(bulletObject);
                tmp.isDamaging = true;
                tmp.gameObject.SetActive(false);
                bullets.Add(tmp);
            }
        
    }

    private void OnDestroy()
    {
        if (bullets == null) return;
        foreach (var bullet in bullets)
        {
            if (bullet != null) Destroy(bullet.gameObject);
        }
    }

    public BossBullet GetPooledBullet()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!bullets[i].gameObject.activeInHierarchy)
            {
                return bullets[i];
            }
        }

        return null;
    }

    public void Shoot(float shotSpeed, float shotSize, Quaternion rotation)
    {
        BossBullet bullet = GetPooledBullet();

        if (bullet == null) return;

        bullet.transform.position = shootingPosition.position;
        bullet.transform.rotation = rotation;
        bullet.SetStats(shotSpeed, shotSize);
        bullet.gameObject.SetActive(true);
    }
}