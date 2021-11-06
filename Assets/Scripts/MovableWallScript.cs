using System.Collections;
using Player_Scripts;
using UnityEngine;

public class MovableWallScript : MonoBehaviour
{
    public Animator animator;
    public float stayingUpTime;
    public float stayingDownTime;
    public bool isUp = true;
    public bool isDown;
    public Player player;

    private void Start()
    {
        StartCoroutine(MoveWall());
    }

    private void Update()
    {
        /* if (player != null  && player.wall != this)
         {
             player = null;
         }
 
         if (player != null && player.isHangingOnWall && isDown )
         {
             gun.RemoveRope();
             player = null;
         }*/
    }


    private IEnumerator MoveWall()
    {
        while (true)
        {
            animator.SetTrigger("WallDown");
            isDown = true;
            yield return new WaitForSeconds(stayingDownTime);
            animator.SetTrigger("WallUp");
            isDown = false;
            yield return new WaitForSeconds(stayingUpTime);
        }

        yield return new WaitForSeconds(0.01f);
    }
}