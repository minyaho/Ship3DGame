using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Timer{
    public SimpleTimer timer;
    public Image cooldown;
}

[System.Serializable]
public class Stats{
    public TMP_Text engine;
    public TMP_Text score_board;

    public int score;
    public float hp; 
    public Image hp_bar;
}
public class UIctrl_player : MonoBehaviour
{
    [SerializeField] private HelicopterController Controller;
    [SerializeField] private Timer[] timers;

    [SerializeField] private Stats info;

    // Start is called before the first frame update
    void Start()
    {
        //cooldown_bomb.gameObject.SetActive(false);
        foreach(Timer t in timers){
            t.cooldown.gameObject.SetActive(false);
            t.cooldown.fillAmount = 0.0f;
        }
        info.score = 0;
        info.hp = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Timer t in timers){
            float clock =  t.timer.GetRemainingTime();
            if(clock != 1.0){
                t.cooldown.gameObject.SetActive(true);
                t.cooldown.fillAmount = clock;
            }
            //Debug.Log("timer:"+t.timer.GetRemainingTime());
        }
        info.engine.text = "Engine: " + Controller.EngineForce.ToString("F2");
        info.hp_bar.fillAmount = info.hp;
    }


}
