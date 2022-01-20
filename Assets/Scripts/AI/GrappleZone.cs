using System.Collections;
using UnityEngine;

namespace AI
{
    public class GrappleZone : MonoBehaviour
    {
        [SerializeField] private Collider2D GrappleCollider2D;
        [SerializeField] private float ActivatedGrappleDuration;

        [SerializeField]
        [Tooltip(
            "Allows to give a delay before activation of grapple. Needed for some inner animations of shooting etc")]
        private float GrappleDelay;

        public void DisableCollider()
        {
            GrappleCollider2D.enabled = false;
        }

        public void EnableCollider()
        {
            GrappleCollider2D.enabled = true;
        }
        
        public IEnumerator ActivateGrappleCollider()
        {   
            
            yield return new WaitForSeconds(GrappleDelay);
            EnableCollider();
            yield return new WaitForSeconds(ActivatedGrappleDuration);
            DisableCollider();
        }
    }
}