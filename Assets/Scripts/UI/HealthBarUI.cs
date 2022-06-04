using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    // https://www.youtube.com/watch?v=p6w4cCvz09c

    public GameObject healthUIPrefab;

    public Transform barPoint;

    public bool alwaysVisible;

    public float visibleTime = 3; 
    private float timeLift; 

    Image healthSlider;

    Transform UIBar;
    Transform cam;

    EnemyStats enemy;
    float currentHealth;
    float maxHealth;

    void Awake()
    {
        enemy = GetComponent<EnemyStats>();
        enemy.UpdateHealthBarOnAttack+=UpdateHealthBar;
    }

    void OnEnable()
    {
        cam = Camera.main.transform;
        
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIBar = Instantiate(healthUIPrefab, canvas.transform).transform;
                healthSlider = UIBar.GetChild(0).GetComponent<Image>();

                UIBar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if(currentHealth <= 0)
        {
            Destroy(UIBar.gameObject);
        }
        UIBar.gameObject.SetActive(true);

        timeLift = visibleTime;



        float sliderPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent; 
    }

    void LateUpdate()
    {
        if(UIBar != null)
        {
            UIBar.position = barPoint.position;
            UIBar.forward = -cam.forward;

            if(timeLift <=0 && !alwaysVisible)
            {
                UIBar.gameObject.SetActive(false);
            }
            else
            {
                timeLift -= Time.deltaTime;
            }
        }
    }
}
