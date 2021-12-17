using Assets.Scripts.Old_Scripts;
using UnityEngine;

public class MoodChange : MonoBehaviour
{
    public GameObject calmMode;
    public GameObject angryMode;
    public GameObject ring;
    public ParticleSystem angryParticles;
    public ParticleSystem sadParticles;

    private float previousX;
    private float size;

    private void Start()
    {
        sadParticles.Play();
        angryParticles.Stop();
        size = transform.localScale.x;
        angryMode.gameObject.SetActive(false);
        previousX = transform.position.x;
        ring.SetActive(false);
    }

    private void FixedUpdate()
    {
        var currentX = angryMode.transform.position.x;
        transform.localScale = previousX - currentX < 0 ? new Vector3(-size, size, size) : Vector3.one * size;
        previousX = currentX;
    }

    private void OnEnable()
    {
        Boss.ActivateBoss += WakeUp;
    }

    private void OnDisable()
    {
        Boss.ActivateBoss -= WakeUp;
    }

    private void WakeUp(LevelData _)
    {
        calmMode.gameObject.SetActive(false);
        angryMode.gameObject.SetActive(true);
        ring.SetActive(true);
        sadParticles.Stop();
        angryParticles.Play();
    }
}