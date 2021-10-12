using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public static int bulletAmount;
    public bool isDamaging;
    public LayerMask wallLayer;
    private float speed;

    private void Awake()
    {
        bulletAmount += 1;
    }

    public void FixedUpdate()
    {
        transform.position += transform.up * (Time.fixedDeltaTime * speed);
    }

    private void OnDestroy()
    {
        bulletAmount -= 1;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (wallLayer == (wallLayer | (1 << other.gameObject.layer))) gameObject.SetActive(false);
    }

    public void SetStats(float speed, float size)
    {
        isDamaging = true;
        transform.localScale = Vector3.one * size;
        this.speed = speed;
    }

   
}