using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setting_stat : MonoBehaviour
{
    public float difficulty;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        difficulty = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
