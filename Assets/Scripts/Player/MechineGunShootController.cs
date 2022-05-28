using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MechineGunShootController : MonoBehaviour
{
    public Transform aimTarget;
    public bool IsAvailable = true;
    public Transform mechineGunL;
    public Transform mechineGunR;
    public GameObject mechineGunBullet;
    public float mechineGunDelay  = 0.5f;
    private float shootDelay = 0.05f;

    void Start()
    {

    }
    void Update()
    {
        
        if( Input.GetMouseButton( 0 ) )
        {
            if ( (shootDelay -= Time.deltaTime) <= 0) 
            { 
                Shoot();
                shootDelay = mechineGunDelay; 
            }
        }
    }

    void Shoot()
    {
        GameObject bullectL = Instantiate( mechineGunBullet, mechineGunL.position, mechineGunL.rotation );
        GameObject bullectR = Instantiate( mechineGunBullet, mechineGunR.position, mechineGunR.rotation );

        bullectL.gameObject.transform.LookAt(aimTarget);
        bullectR.gameObject.transform.LookAt(aimTarget);

    }
}
