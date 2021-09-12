using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class AbyssColliderChanger : MonoBehaviour
    {
        [SerializeField] private Collider2D AbyssLayerCollider;
        [SerializeField] private float ChangeReactTime;
        public void SetAbyssTrigger(bool isTrigger)
        {
            AbyssLayerCollider.isTrigger = isTrigger;
        }

        IEnumerator ChangeCondition(bool value)
        {
            yield return new WaitForSeconds(ChangeReactTime);
            AbyssLayerCollider.isTrigger = value;
        }
    }
}