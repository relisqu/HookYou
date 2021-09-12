using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Old_Scripts;
using UnityEngine;

public class MovableWallScript : MonoBehaviour
{
    public Animator animator;
    public float stayingUpTime;
    public float stayingDownTime;
    public bool isUp = true;
    public bool isDown;
    public Player player;

    
    IEnumerator MoveWall()
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

    void Start()
    {
        StartCoroutine(MoveWall());
    }
    
    void Update()
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
}
