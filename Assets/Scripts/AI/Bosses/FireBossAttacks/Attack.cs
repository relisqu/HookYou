using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public abstract class Attack : MonoBehaviour
    {
        [SerializeField]protected List<Attack> AttackIngredients;
        public abstract IEnumerator StartAttack();
    }
}