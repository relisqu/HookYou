using System.Collections.Generic;
using UnityEngine;

public class CanonModule : MonoBehaviour
{
    public Bullet bulletObject;
    public Transform shootingPosition;
    public int amountToPool;

    private List<Bullet> bullets;

    private void Start()
    {
        bullets = new List<Bullet>();
        Bullet tmp;
        for (var i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(bulletObject);
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

    public Bullet GetPooledBullet()
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