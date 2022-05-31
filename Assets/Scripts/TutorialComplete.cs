using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialComplete : MonoBehaviour
{
    public GameObject gameComplete;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log ("Hit goal");
        gameComplete.gameObject.SetActive(true);
        TimeCounter.stopCount();
    }
}
