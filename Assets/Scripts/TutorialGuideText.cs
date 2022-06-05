using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialGuideText : MonoBehaviour
{
    public TextMeshProUGUI helpText;
    private bool isEnabled = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.H))
        {
            isEnabled = !isEnabled;
            helpText.enabled = isEnabled;
        }
    }
}
