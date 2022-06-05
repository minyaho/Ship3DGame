using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarBalloonTurret : EnemyStats
{
    protected WarBalloonTurretState currentState;

    public Transform Target { get; set; }
   
    [Header("Turret Elements")]
    [SerializeField] private Transform rotator;

    [SerializeField] private Transform ghostRotator;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private Transform[] gunBarrels;

    [SerializeField] private GameObject projectile;

    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource fireSound;


    [Header("Parameters")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Vector3 aimOffset;

    public Quaternion DefaultRotation { get; set; }

    public Transform Rotator { get => rotator; set => rotator = value; }
    public Vector3 AimOffset { get => aimOffset; set => aimOffset = value; }
    public Transform GhostRotator { get => ghostRotator; set => ghostRotator = value; }
    public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
    public Transform[] GunBarrels { get => gunBarrels; set => gunBarrels = value; }
    public Animator Animator { get => animator; set => animator = value; }

    private void Start()
    {
        base.Start();
        fireSound.playOnAwake = false;
        DefaultRotation = rotator.rotation;
        ChangeState(new WarBalloonIdleState());
    }

    private void Update()
    {
        currentState.Update();
        base.Update();
    }

    public bool CanSeeTarget(Vector3 direction, Vector3 origin, string tag)
    {
        RaycastHit hit;
        // Debug.Log(tag);

        if (Physics.Raycast(origin,direction, out hit, Mathf.Infinity,layerMask))
        {
            if (hit.collider.tag == tag)
            {
                return true;
            }
        }

        return false;
    }

    public void Shoot(int index)
    {
        Vector3 rand = new Vector3(1, 1, 1) * 0.05f;
        Vector3 v = GunBarrels[index].forward + MathUtilities.Random( -rand, rand );
        Quaternion headingDirection = Quaternion.FromToRotation(projectile.transform.forward, v);

        GameObject bullet = Instantiate(projectile, GunBarrels[index].position, headingDirection);
        bullet.GetComponent<Projectile>().Direction = v;
        fireSound.Play();
    }
    public void ChangeState(WarBalloonTurretState newState)
    {
        if (newState != null)
        {
            newState.Exit();
        }
        this.currentState = newState;

        newState.Enter(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }
}
