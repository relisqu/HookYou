using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts
{
    public class AbyssColliderChanger : MonoBehaviour
    {
        
        [SerializeField] private List<CompositeCollider2D> AbyssLayerCollider;
        [SerializeField] private float ChangeReactTime;

        public void SetAbyssTrigger(bool isTrigger)
        {
            foreach (var collider in AbyssLayerCollider)
            {
                collider.isTrigger = isTrigger;
            }
        }

        private IEnumerator ChangeCondition(bool value)
        {
            yield return new WaitForSeconds(ChangeReactTime);
            foreach (var collider in AbyssLayerCollider)
            {
                collider.isTrigger = value;
            }
        }
    }
}