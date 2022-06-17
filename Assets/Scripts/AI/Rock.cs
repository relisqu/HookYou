using UnityEngine;

namespace AI
{
    public class Rock : MonoBehaviour
    {
        public void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}