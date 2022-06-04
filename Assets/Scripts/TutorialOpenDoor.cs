using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOpenDoor : MonoBehaviour
{
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.transform.childCount == 0)
        {
            Destroy (gameObject);
        }
    }


}
