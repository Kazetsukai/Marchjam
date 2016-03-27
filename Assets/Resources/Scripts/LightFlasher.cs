using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class LightFlasher : MonoBehaviour
{
    [SerializeField] AnimationCurve TimeVsIntensity;   
    [SerializeField] bool StartImmediately = true; 
    [SerializeField] bool FlashNow = false;
    Light light;
    
    void Start()
    {
        light = GetComponent<Light>();

        if (StartImmediately)
        {
            FlashNow = true;
        }
    }
    
    void Update()
    {      
        if (FlashNow)
        {
            Flash();
            FlashNow = false;
        }
    }

    public void Flash()
    {
        StartCoroutine(DoFlash());
    }

    IEnumerator DoFlash()
    {
        float elapsed = 0;
        float duration = TimeVsIntensity.keys[TimeVsIntensity.keys.Length - 1].time;

        do
        {
            elapsed += Time.deltaTime;
            light.intensity = TimeVsIntensity.Evaluate(elapsed);
            yield return null;
        }
        while (elapsed / duration < 1);
    }
}
