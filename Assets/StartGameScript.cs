using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    private bool started;
    void Start()
    {
      DontDestroyOnLoad(gameObject);
      if(!started)
          AudioManager.instance.Play("zelda_music");
      started = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
