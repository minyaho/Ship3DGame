using UnityEngine;

[RequireComponent(typeof(PlayerState))]
public class HelicopterController : MonoBehaviour
{
    [Header("Main References")]
    [SerializeField] private AudioSource HelicopterSound;
    [SerializeField] private AudioSource CrashSound;
    [SerializeField] private ControlPanel ControlPanel;
    [SerializeField] private Rigidbody HelicopterModel;
    [SerializeField] private Light ButtomLight;
    [SerializeField] private ParticleSystem _smoke;

    [Header("Rotor References")]
    [SerializeField] private HeliRotorController MainRotorController;
    [SerializeField] private HeliRotorController SubRotorController;

    [Header("Parameters")]
    public float TurnForce = 4f;
    public float ForwardForce = 10f;
    public float ForwardTiltForce = 20f;
    public float TurnTiltForce = 40f;
    public float EffectiveHeight = 100f;
    public float DownForce = 0f;

    public float turnTiltForcePercent = 6.5f;
    public float turnForcePercent = 7.3f;

    [SerializeField] private bool Crash = false;

    [Header("Explosion")]
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private int _explosionLifeTime = 3;

    private float _engineForce;
    public float EngineForce
    {
        get { return _engineForce; }
        set
        {
            MainRotorController.RotarSpeed = value * 80;
            SubRotorController.RotarSpeed = value * 40;
            HelicopterSound.pitch = Mathf.Clamp(value / 40, 0, 1.2f);
            if( _engineForce > 0 && HelicopterSound.isPlaying == false ){
                HelicopterSound.Play();
            }
            _engineForce = value;
        }
    }

    private Vector2 hMove = Vector2.zero;
    private Vector2 hTilt = Vector2.zero;
    private float hTurn = 0f;
    public bool IsOnGround = true;
    // Use this for initialization

    private bool crashX, crashY;
	void Start ()
	{
        SetSmokeRate(0);
        ControlPanel.KeyPressed += OnKeyPressed;
	}

	void Update () {
	}
  
    void FixedUpdate()
    {
        if( Crash == true )
        {
            hMove.x += Time.fixedDeltaTime * 1.6f * (crashX ? 1 : -1);
            hMove.x = Mathf.Clamp(hMove.x, -1, 1);

            hMove.y += Time.fixedDeltaTime * 1.6f * (crashY ? 1 : -1);
            hMove.y = Mathf.Clamp(hMove.y, -1, 1);
            EngineForce = Mathf.Max(EngineForce - 0.24f, 0.0f);
        }
        LiftProcess(HelicopterModel);
        MoveProcess(HelicopterModel);
        TiltProcess(HelicopterModel);
      


        // make light not to shin
        ButtomLight.intensity = Mathf.Clamp(1 + transform.position.y / 80, 0.0f, 4.0f);
    }

    public void MoveProcess(Rigidbody rb)
    {
        var turn = TurnForce * Mathf.Lerp(hMove.x, hMove.x * (turnTiltForcePercent - Mathf.Abs(hMove.y)), Mathf.Max(0f, hMove.y));
        hTurn = Mathf.Lerp(hTurn, turn, Time.fixedDeltaTime * TurnForce);
        rb.AddRelativeTorque(0f, hTurn * rb.mass, 0f);
        rb.AddRelativeForce(Vector3.forward * Mathf.Max(0f, hMove.y * ForwardForce * rb.mass));
    }

    public void LiftProcess(Rigidbody rb)
    {
        var upForce = 1 - Mathf.Clamp(rb.transform.position.y / EffectiveHeight, 0, 1) - DownForce;
        upForce = Mathf.Lerp(0f, EngineForce, upForce) * rb.mass;
        rb.AddRelativeForce(Vector3.up * upForce);
    }

    public void TiltProcess(Rigidbody rb)
    {
        hTilt.x = Mathf.Lerp(hTilt.x, hMove.x * TurnTiltForce, Time.deltaTime);
        hTilt.y = Mathf.Lerp(hTilt.y, hMove.y * ForwardTiltForce, Time.deltaTime);
        rb.transform.localRotation = Quaternion.Euler(hTilt.y, rb.transform.localEulerAngles.y, -hTilt.x);
    }

    private void OnKeyPressed(PressedKeyCode[] obj)
    {
        bool pressDown = false;
        float tempY = 0;
        float tempX = 0;

        // stable forward
        if (hMove.y > 0)
            tempY = - Time.fixedDeltaTime;
        else
            if (hMove.y < 0)
                tempY = Time.fixedDeltaTime;

        // stable lurn
        if (hMove.x > 0)
            tempX = -Time.fixedDeltaTime;
        else
            if (hMove.x < 0)
                tempX = Time.fixedDeltaTime;



        foreach (var pressedKeyCode in obj)
        {
            switch (pressedKeyCode)
            {
                case PressedKeyCode.SpeedUpPressed:
                    EngineForce += 0.1f;
                    break;
                case PressedKeyCode.SpeedDownPressed:

                    EngineForce -= 0.12f;
                    if (EngineForce < 0) EngineForce = 0;
                    break;
                case PressedKeyCode.DownPressed:
                    if (IsOnGround) break;
                    pressDown = true;
                    EngineForce -= 0.35f;
                    if (EngineForce < 0) EngineForce = 0;
                    DownForce += 0.25f;
                    break;
                case PressedKeyCode.ForwardPressed:
                    if (IsOnGround) break;
                    tempY = Time.fixedDeltaTime;
                    break;
                case PressedKeyCode.BackPressed:

                    if (IsOnGround) break;
                    tempY = -Time.fixedDeltaTime;
                    break;
                case PressedKeyCode.LeftPressed:

                    if (IsOnGround) break;
                    tempX = -Time.fixedDeltaTime;
                    break;
                case PressedKeyCode.RightPressed:

                    if (IsOnGround) break;
                    tempX = Time.fixedDeltaTime;
                    break;
                case PressedKeyCode.TurnRightPressed:
                    {
                        if (IsOnGround) break;
                        var force = (turnForcePercent - Mathf.Abs(hMove.y))*HelicopterModel.mass;
                        HelicopterModel.AddRelativeTorque(0f, force, 0);
                    }
                    break;
                case PressedKeyCode.TurnLeftPressed:
                    {
                        if (IsOnGround) break;
                        
                        var force = -(turnForcePercent - Mathf.Abs(hMove.y))*HelicopterModel.mass;
                        HelicopterModel.AddRelativeTorque(0f, force, 0);
                    }
                    break;

            }
        }

        hMove.x += tempX;
        hMove.x = Mathf.Clamp(hMove.x, -1, 1);

        hMove.y += tempY;
        hMove.y = Mathf.Clamp(hMove.y, -1, 1);

        if( pressDown == false )
        {
            DownForce = Mathf.Max(DownForce - 0.25f, 0f);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if( collision.gameObject.CompareTag("Terrain") )
        {
            IsOnGround = true;

            if( Crash == true )
            {
                // Exlosion effect
                if (_explosionPrefab)
                {
                    GameObject exlosionEffect = Instantiate(_explosionPrefab, this.transform.position, new Quaternion());
                    Destroy(exlosionEffect, _explosionLifeTime);
                }
                Destroy(this.gameObject);
                Destroy(ControlPanel.gameObject);
                GetComponent<PlayerState>().OnDestory();
            }
        }

    }
    // private void OnTriggerEnter (Collider other)
    // {
    //     Debug.Log( "trigger (name) : " + other.gameObject.name );
    // }

    private void OnCollisionExit(Collision collision)
    {
        IsOnGround = false;
    }

    public void SetSmokeRate(int rate)
    {
        ParticleSystem.EmissionModule m = _smoke.emission;
        m.rateOverTime = new ParticleSystem.MinMaxCurve(rate);
    }

    public void SetCrash()
    {
        if( Crash == false )
        {
            crashX = (Random.Range(-4, 5) & 1) == 0;
            crashY = (Random.Range(-4, 5) & 1) == 0;
            Crash = true;
            ControlPanel.AllowUserControl = false;
            GetComponent<BombingController>().AllowUserControl = false;
            GetComponent<RocketShootController>().AllowUserControl = false;
            GetComponent<MechineGunShootController>().AllowUserControl = false;
            CrashSound.playOnAwake = true;
            CrashSound.Play();
        }
    }
}