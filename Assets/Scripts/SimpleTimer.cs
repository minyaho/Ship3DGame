using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Simple cool down timer.
/// </summary>
public class SimpleTimer : MonoBehaviour
{
    /// <summary>
    /// The amount of time to take to cool down.
    /// </summary>
    [SerializeField] private float coolDownAmount = 0.5f;

    /// <summary>
    /// The remaining  time of this cool down.
    /// </summary>
    [SerializeField] private float remainingTime;

    /// <summary> 
    /// The definition of main method.
    /// </summary>
    public delegate void Method();
    /// <summary> 
    /// The main execute method for calling.
    /// </summary>
    private Method _mainMethod;

    /// <summary> 
    /// The check execute method for calling when fail (optional).
    /// </summary>
    private Method _onCountDownMethod;
    /// <summary> 
    /// The definition of check method.
    /// </summary>
    public delegate bool CheckMethod();
    /// <summary> 
    /// The check execute method for calling (optional).
    /// </summary>
    private CheckMethod _countDownCondition;


    /// <summary> 
    /// Get remaining time of cool down.
    /// </summary>
    public float GetRemainingTime()
    {
        return (remainingTime == coolDownAmount) ? 0 : remainingTime;
    }
    
    public float GetTimer()
    {
        return coolDownAmount;
    }
    private void SetCoolDown()
    {
        remainingTime = coolDownAmount;
    }  

    // Auto Repeat
    public void Binding(Method method)
    {
        SetCoolDown();
        _mainMethod = method;
    }

    public void Binding(Method method, CheckMethod countDownCondiction)
    {
        this.Binding(method);
        _countDownCondition = countDownCondiction;
    }

    public void Binding(Method method, Method coolDownNotCompleteMethod, CheckMethod check)
    {
        this.Binding(method, check);
        _onCountDownMethod = coolDownNotCompleteMethod;
    }



    /// <summary> 
    /// Force reset the cool down.
    /// </summary>
    public void Reset()
    {
        SetCoolDown();
    }

    /// <summary> 
    /// Is the cool down complete.
    /// </summary>
    public bool Running {get; set;} = true;

    public void SubCoolTime(float value)
    {
        remainingTime += value;
    }
    public void AddCoolTime(float value)
    {
        remainingTime -= value;
    }

    private void FixedUpdate()
    {
        // Count Down
        if( _countDownCondition != null )
        {
            if( _countDownCondition() )
            {
                remainingTime = Mathf.Max(remainingTime - Time.fixedDeltaTime, 0);
            }
            else{
                return;
            }
        }
        else
        {
            remainingTime = Mathf.Max(remainingTime - Time.fixedDeltaTime, 0);
        }


        if( (remainingTime <= 0) && Running == true )
        {
            SetCoolDown();
            _mainMethod();
        }
        else{
            if( _onCountDownMethod != null ){ _onCountDownMethod(); }
        }



    }


}
