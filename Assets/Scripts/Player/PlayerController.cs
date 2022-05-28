using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

    // float fMoveSpeed = 2; //移動速度

    // float fRotateSpeed = 0.05f; //旋轉速度
    // public GameObject body;
    // private Rigidbody myrb;


    // // Start is called before the first frame update
    // void Start()
    // {        
    //     myrb = GetComponent<Rigidbody>();
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //    MoveControlByTranslateGetAxis();
    // }

    // void MoveControlByTranslateGetAxis()
    // {
    //     Rigidbody mainRB = body.GetComponent<Rigidbody>();
    //     float horizontal = Input.GetAxis("Horizontal"); //A D 左右
    //     float vertical = Input.GetAxis("Vertical"); //W S 上 下

    //     // transform.Translate(Vector3.forward * vertical * fMoveSpeed * Time.deltaTime);//W S 上 下
    //     // transform.Translate(Vector3.right * horizontal * fMoveSpeed * Time.deltaTime);//A D 左右
    //     // float horizontal = Input.GetAxis("Horizontal");
    //     // float vertical = Input.GetAxis("Vertical");
 
    //     mainRB.AddTorque(0f, horizontal * fRotateSpeed, 0f);
    //     mainRB.AddForce(transform.forward * vertical * fMoveSpeed);
       

    //     myrb.AddTorque(0f, horizontal * fRotateSpeed, 0f);
    //     myrb.AddForce(transform.forward * vertical * fMoveSpeed);

    //     float velocityMagnitude = mainRB.velocity.magnitude;
    //     mainRB.velocity = transform.forward * velocityMagnitude;
    //     myrb.velocity = transform.forward * velocityMagnitude;

    // }
    public float forwardSpeed = 25f;
    public float strafeSpeed = 7.5f;
    public float hoverSpeed = 5f;

    public float forwardAcceleration = 2.5f;
    public float strafeAcceleration  = 2f;
    public float hoverAcceleration   = 2f;
    public float lookRateSpeed = 90f;

    public float rollSpeed = 90f;
    public float rollAcceleration = 3.5f;
    private float activeForwardSpeed;
    private float activeStrafeSpeed;
    private float activeHoverSpeed;

    private float rollInput;

    private Vector2 lookInput;
    private Vector2 screenCenter;
    private Vector2 mouseDistance;

    void Start()
    {
        screenCenter.x = Screen.width * 0.5f;
        screenCenter.y = Screen.height * 0.5f;
    }
    void Update()
    {
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);

        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, mouseDistance.x * lookRateSpeed * Time.deltaTime, rollInput * rollSpeed * Time.deltaTime, Space.Self);

        activeForwardSpeed = Mathf.Lerp(activeForwardSpeed, Input.GetAxisRaw("Vertical") * forwardSpeed, forwardAcceleration * Time.deltaTime);
        activeStrafeSpeed  = Mathf.Lerp(activeStrafeSpeed, Input.GetAxisRaw("Horizontal") * strafeSpeed, strafeAcceleration * Time.deltaTime);
        activeHoverSpeed   = Mathf.Lerp(activeHoverSpeed, Input.GetAxisRaw("Hover") * hoverSpeed, hoverAcceleration * Time.deltaTime);

        transform.position += transform.forward * activeForwardSpeed * Time.deltaTime;
        transform.position += (transform.right   * activeStrafeSpeed  * Time.deltaTime) + (transform.up * activeHoverSpeed * Time.deltaTime);


    }
}
