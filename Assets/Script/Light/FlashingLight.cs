using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    public float totalSeconds;
    public float maxIntensity;
    public float minIntensity;
    public Light myLight;        

    public void Start()
    {
        StartCoroutine(flashOn());
    }

    public IEnumerator flashOn()
    {
        float waitTime = totalSeconds / 2;
        while (myLight.intensity < maxIntensity)
        {
            myLight.intensity += Time.deltaTime / waitTime;
            yield return null;
        }
        while (myLight.intensity > minIntensity)
        {
            myLight.intensity -= Time.deltaTime / waitTime;
            yield return null;
        }
        StartCoroutine(flashOn());
        yield return null;
    }
}
