using UnityEngine;

namespace Props
{
    public class Gem : MonoBehaviour
    {
        [SerializeField] private int Value;
        private bool isCollected;
        public void Collect()
        {
            if (!isCollected)
            {
                isCollected = true;
                Destroy(gameObject);
            }
        }

        public int GetValue =>Value;
    }
}