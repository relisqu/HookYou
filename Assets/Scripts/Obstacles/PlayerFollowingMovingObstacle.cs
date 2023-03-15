using System;
using Player_Scripts;
using UnityEngine;

namespace Obstacles
{
    public class PlayerFollowingMovingObstacle : MonoBehaviour
    {
        public static Transform Player;
        //private static Transform Player;

        private void Start()
        {
            if (Player == null)
            {
                Player = FindObjectOfType<PlayerMovement>().transform;
            }
        }

        private void Update()
        {
            throw new NotImplementedException();
        }
    }
}