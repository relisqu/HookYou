using UnityEngine;

namespace Additional_Technical_Settings_Scripts
{
    public class SoundPlayer : MonoBehaviour
    {
        public void PlaySound(string sound)
        {
            AudioManager.instance.Play(sound);
        }
    }
}