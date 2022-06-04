using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public abstract class Attack : MonoBehaviour
    {
        public abstract IEnumerator StartAttack();
    }
}