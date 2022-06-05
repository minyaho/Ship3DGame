using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarBalloon : EnemyStats
{

    [Header("Parameters")]
    public float ForwardTiltForce = 20f;
    public float TurnTiltForce = 40f;
    private Rigidbody rb;
     private Vector2 hMove = Vector2.zero;
    private Vector2 hTilt = Vector2.zero;
    private void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        StartCoroutine( StableRotaion() );
    }   

    private void Update()
    {
        base.Update();
        hMove.x = transform.rotation.x;
        hMove.y = transform.rotation.y;
    }

    private void FixedUpdate()
    {


        hTilt.x = Mathf.Lerp(hTilt.x, hMove.x * TurnTiltForce, Time.deltaTime);
        hTilt.y = Mathf.Lerp(hTilt.y, hMove.y * ForwardTiltForce, Time.deltaTime);
        rb.transform.localRotation = Quaternion.Euler(hTilt.y, rb.transform.localEulerAngles.y, -hTilt.x);
    }

    IEnumerator StableRotaion()
    {
        while (true)
        {        
            yield return new WaitForSeconds( 2f );
            float tempY = 0;
            float tempX = 0;
            // stable lurn
            if (hMove.x > 0)
                tempX = -Time.deltaTime * 0.05f;
            else
                if (hMove.x < 0)
                    tempX = Time.deltaTime  * 0.05f;

            hMove.x += tempX;
            hMove.x = Mathf.Clamp(hMove.x, -1, 1);

            hMove.y += tempY;
            hMove.y = Mathf.Clamp(hMove.y, -1, 1);
        }
       
    }
}
