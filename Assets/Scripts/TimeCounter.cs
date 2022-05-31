using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    private static bool stopCounting = false; 
    private float timeValue = 0;

    void Update()
    {
        if (stopCounting == false) {
            timeValue += Time.deltaTime;
            timeText.text = "Time: " + Mathf.Round (timeValue).ToString() + "s";
        }
    }

    public static void stopCount ()
    {
        stopCounting = true;
    }
}
