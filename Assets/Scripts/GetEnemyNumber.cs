using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetEnemyNumber : MonoBehaviour
{
    public GameObject enemyNumber;
    public TextMeshProUGUI enemyText;

    // Update is called once per frame
    void Update()
    {
        enemyText.text = "Enemy: " + enemyNumber.transform.childCount.ToString ();
    }
}
