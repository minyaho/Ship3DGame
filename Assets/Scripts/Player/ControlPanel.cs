using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ControlPanel : MonoBehaviour {
    // public AudioSource MusicSound;

    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera _mainCamera;
    [SerializeField] private CinemachineVirtualCamera _bombCamera;

    [Header("Key Binding")]
    [SerializeField]
    KeyCode SpeedUp = KeyCode.Space;
    [SerializeField]
    KeyCode SpeedDown = KeyCode.LeftShift;
    [SerializeField]
    KeyCode Down = KeyCode.LeftControl;
    [SerializeField]
    KeyCode Forward = KeyCode.W;
    [SerializeField]
    KeyCode Back = KeyCode.S;
    [SerializeField]
    KeyCode Left = KeyCode.A;
    [SerializeField]
    KeyCode Right = KeyCode.D;
    [SerializeField]
    KeyCode TurnLeft = KeyCode.Q;
    [SerializeField]
    KeyCode TurnRight = KeyCode.E;
    [SerializeField]
    KeyCode MusicOffOn = KeyCode.M;
    
    private KeyCode[] keyCodes;
    
    [Header("System Mode")]
    [SerializeField] private LockSystem _lockSystem;
    [SerializeField] private RectTransform _normalCrosshair;
    private bool BombingMode = false;
    public Action<PressedKeyCode[]> KeyPressed;

    public bool AllowUserControl {get; set;} = true;
    private void Awake()
    {
        keyCodes = new[] {
                            SpeedUp,
                            SpeedDown,
                            Down,
                            Forward,
                            Back,
                            Left,
                            Right,
                            TurnLeft,
                            TurnRight
                        };

    }

    void Start () {
	
	}

    void Update()
    {
        // 不接受任何輸入
        if( AllowUserControl == false )
        {
            return;
        }

        // 切換到轟炸
        if( Input.GetMouseButtonDown(2) && _lockSystem.Enable == false && _bombCamera.gameObject.activeSelf )
        {
            BombingMode = !BombingMode;
            int priority = _mainCamera.m_Priority;
            _mainCamera.m_Priority = _bombCamera.m_Priority;
            _bombCamera.m_Priority = priority;
        }
        // 切換到火箭
        else if( Input.GetMouseButton(1) && BombingMode == false )
        {
            _lockSystem.Enable = true;
        }
        else
        {
            _lockSystem.Enable = false;
        }

        _normalCrosshair.gameObject.SetActive( !(_lockSystem.Enable || BombingMode) );
    }
	void FixedUpdate ()
	{
	    var pressedKeyCode = new List<PressedKeyCode>();
	    for (int index = 0; index < keyCodes.Length; index++)
	    {
	        var keyCode = keyCodes[index];
	        if (Input.GetKey(keyCode))
                pressedKeyCode.Add((PressedKeyCode)index);
	    }

	    if (KeyPressed != null)
	        KeyPressed(pressedKeyCode.ToArray());

        // for test
        /*if (Input.GetKey(MusicOffOn))
        {
            if (  MusicSound.volume == 1) return;
            if (MusicSound.isPlaying)
                MusicSound.Stop();
            else
                MusicSound.volume = 1;
                MusicSound.Play();
        }*/

	}
}
