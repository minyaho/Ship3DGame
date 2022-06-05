using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarBalloonShootState : WarBalloonTurretState
{

    public override void Enter(WarBalloonTurret parent)
    {
        base.Enter(parent);
        parent.Animator.SetBool("Shoot", true);
    }
    public override void Update()
    {
        if (parent.Target != null)
        {
            parent.Rotator.LookAt(parent.Target.position + parent.AimOffset);
            
        }
        if (!parent.CanSeeTarget(parent.GunBarrels[0].forward, parent.Rotator.position, "Player"))
        {
            parent.ChangeState(new WarBalloonIdleState());
        }
    }
}
