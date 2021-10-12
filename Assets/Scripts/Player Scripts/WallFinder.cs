using UnityEngine;

public class WallFinder : MonoBehaviour
{
    [SerializeField] private LayerMask WallLayers;
    public bool IsNearWall;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var o = other.gameObject;
        IsNearWall = WallLayers == (WallLayers | (1 << o.layer));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var otherObj = other.gameObject;
        if (WallLayers == (WallLayers | (1 << otherObj.layer))) IsNearWall = false;
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        var otherObj = other.gameObject;
        IsNearWall = WallLayers == (WallLayers | (1 << otherObj.layer));
    }
}