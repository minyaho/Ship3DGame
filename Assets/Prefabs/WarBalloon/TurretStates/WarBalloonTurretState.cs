using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WarBalloonTurretState
{
    protected WarBalloonTurret parent;

    public virtual void Enter(WarBalloonTurret parent)
    {
        this.parent = parent;
    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void OnTriggerEnter(Collider other)
    {

    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            parent.Target = null;
            parent.ChangeState(new WarBalloonIdleState());
        }
    }
}
