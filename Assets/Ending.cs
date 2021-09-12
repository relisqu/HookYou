using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public ParticleSystem confetti;
    void Start()
    {
        StartCoroutine(ThrowConfettiOnce());
    }

    public void ThrowConfetti()
    {
        
    }

    IEnumerator ThrowConfettiOnce()
    {
        while (true)
        {
            
            confetti.Play();
            yield return new WaitForSeconds(5f);
        }
        yield return new WaitForSeconds(0.01f);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("FirstLocation");
        }
    }
}
