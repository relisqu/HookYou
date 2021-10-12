using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour
{
    [SerializeField]private int Damage;
    [SerializeField]private float Speed;
    [SerializeField]public bool IsPlayerFriendly;
    [FormerlySerializedAs("wallLayer")] public LayerMask WallLayers;


    public void FixedUpdate()
    {
        transform.position += transform.up * (Time.fixedDeltaTime * Speed);
    }

    public int GetDamage => Damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (WallLayers == (WallLayers | (1 << other.gameObject.layer))) gameObject.SetActive(false);
    }
}