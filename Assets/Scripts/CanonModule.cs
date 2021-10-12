﻿using System.Collections.Generic;
using UnityEngine;

public class CanonModule : MonoBehaviour
{
    public BossBullet bulletObject;
    public Transform shootingPosition;
    public int amountToPool;

    private List<BossBullet> bullets;

    private void Start()
    {
        bullets = new List<BossBullet>();
        BossBullet tmp;
        for (var i = 0; i < amountToPool; i++)
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
            if (bullet != null)
                Destroy(bullet.gameObject);
    }

    public BossBullet GetPooledBullet()
    {
        for (var i = 0; i < amountToPool; i++)
            if (!bullets[i].gameObject.activeInHierarchy)
                return bullets[i];

        return null;
    }

    public void Shoot(float shotSpeed, float shotSize, Quaternion rotation)
    {
        var bullet = GetPooledBullet();

        if (bullet == null) return;

        bullet.transform.position = shootingPosition.position;
        bullet.transform.rotation = rotation;
        bullet.SetStats(shotSpeed, shotSize);
        bullet.gameObject.SetActive(true);
    }
}