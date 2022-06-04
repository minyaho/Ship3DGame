
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class FadeLight : MonoBehaviour
{
    [SerializeField] private float fadeStart = 1.0f;
    [SerializeField] private float fadeEnd = 1.0f;
    [SerializeField] private float fadeTime = 5.0f;
    private Light l;
    private float t = 0.0f;
    private void Start()
    {
        l = GetComponent<Light>();
    }
    private void FixedUpdate()
    {
        t += Time.fixedDeltaTime;
        l.intensity = Mathf.Lerp(fadeStart, fadeEnd, t / fadeTime);
    }


}
