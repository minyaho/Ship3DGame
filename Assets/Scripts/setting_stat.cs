using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setting_stat : MonoBehaviour
{
    public static float difficulty;
    // Start is called before the first frame update

    void Start()
    {
        difficulty = 0.0f;
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        // difficulty = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
