using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarBalloonIdleState : WarBalloonTurretState
{
    public override void Enter(WarBalloonTurret parent)
    {
        base.Enter(parent);

        parent.Animator.SetBool("Shoot", false);
    }

    public override void Update()
    {
        if (parent.DefaultRotation != parent.Rotator.rotation)
        {
            parent.Rotator.rotation = Quaternion.RotateTowards(parent.Rotator.rotation, parent.DefaultRotation, Time.deltaTime * parent.RotationSpeed);
        }

        if (parent.Target != null)
        {
            if (parent.CanSeeTarget(((parent.Target.position+parent.AimOffset)- parent.GunBarrels[0].position),parent.GunBarrels[0].position, "Player"))
            {
                parent.ChangeState(new WarBalloonFindTargetState());
            }
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            parent.Target = other.transform;
            parent.ChangeState(new WarBalloonFindTargetState());
        }
    }
}
