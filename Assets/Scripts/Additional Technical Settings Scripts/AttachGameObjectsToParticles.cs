using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public GameObject m_Prefab;
    public float spriteScale;
    private readonly List<GameObject> m_Instances = new List<GameObject>();
    private ParticleSystem.Particle[] m_Particles;
    private ParticleSystem m_ParticleSystem;

    // Start is called before the first frame update
    private void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var count = m_ParticleSystem.GetParticles(m_Particles);

        while (m_Instances.Count < count)
            m_Instances.Add(Instantiate(m_Prefab, m_ParticleSystem.transform));

        var worldSpace = m_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World;
        for (var i = 0; i < m_Instances.Count; i++)
            if (i < count)
            {
                m_Instances[i].transform.localScale =
                    Vector3.one * (spriteScale * m_Particles[i].GetCurrentSize(m_ParticleSystem));
                if (worldSpace)
                    m_Instances[i].transform.position = m_Particles[i].position;
                else
                    m_Instances[i].transform.localPosition = m_Particles[i].position;
                m_Instances[i].SetActive(true);
            }
            else
            {
                m_Instances[i].SetActive(false);
            }
    }
}