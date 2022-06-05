using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIctrl_gameover : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btnReturn;

    void Start()
    {
        btnReturn.onClick.AddListener(clickReturn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void clickReturn(){
        Debug.Log("[Log] return");
        SceneManager.LoadScene("UI_start");
    }
}
