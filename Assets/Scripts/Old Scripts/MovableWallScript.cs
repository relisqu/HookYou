using System.Collections;
using Player_Scripts;
using UnityEngine;

namespace Old_Scripts
{
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
}