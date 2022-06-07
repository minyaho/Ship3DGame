using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialComplete : MonoBehaviour
{
    [SerializeField] private GameObject mainPlayer;

    public GameObject gameComplete;

    void OnCollisionEnter(Collision collision)
    {

        if( collision.transform.tag == "Player" )
        {
            Debug.Log ("Hit goal");
            gameComplete.gameObject.SetActive(true);
            PlayerState playerstate = mainPlayer.GetComponent<PlayerState>();
            playerstate.timerUpdate = false;
            mainPlayer.SetActive(false);
        }
    }
}
