using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    private bool started;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (!started)
            AudioManager.instance.Play("zelda_music");
        started = true;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}