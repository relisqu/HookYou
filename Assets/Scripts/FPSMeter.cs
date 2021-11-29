using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMeter : MonoBehaviour
{
    private float time;
    private int frameCount;
    [SerializeField] private int FPSCutSlider;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /*
                Application.targetFrameRate = FPSCutSlider;
                time += Time.deltaTime;
                frameCount++;
                if (time >= 1)
                {
                    time -= 1;
                    frameCount = 0;
                }
        */
    }
}