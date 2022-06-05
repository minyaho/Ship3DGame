using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOpenDoor : MonoBehaviour
{
    public GameObject enemy;
    //public GameObject door;
    Animator m_Animator;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.transform.childCount == 0)
        {
            m_Animator.Play ("OpenDoor");
            //Destroy (gameObject);
        }
    }


}
