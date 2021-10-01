using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public ParticleSystem confetti;

    private void Start()
    {
        StartCoroutine(ThrowConfettiOnce());
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space)) SceneManager.LoadScene("FirstLocation");
    }

    public void ThrowConfetti()
    {
    }

    private IEnumerator ThrowConfettiOnce()
    {
        while (true)
        {
            confetti.Play();
            yield return new WaitForSeconds(5f);
        }

        yield return new WaitForSeconds(0.01f);
    }
}