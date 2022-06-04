using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
class LockSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _crosshair;
    [SerializeField] private RectTransform _missileCrosshair;
    [SerializeField] private RectTransform _normalCrosshair;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private GameObject _aimTarget;
   
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private AudioSource _lockingSound;

    // =============================================

    [Header("Parameters")]
    [SerializeField] private float _lockingTime = 2;
    private float _lockingDelay;
    private float _pulseDelay;
    private float _pulseTime;

    // =============================================
    private LockedDictionary _lockedDict;
    private GameObject _lockingTarget = null;
    private GameObject _preLockingTarget = null;
    private GameObject _realTarget = null;
    private RawImage _crosshiarImage;
    private void Start()
    {
        _lockingSound.playOnAwake = false;
        _lockedDict = GetComponent<LockedDictionary>();
        _crosshiarImage = _crosshair.gameObject.GetComponent<RawImage>();
    }

    private void FixedUpdate()
    {
        bool flag = Input.GetMouseButton(1);
        _normalCrosshair.gameObject.SetActive(!flag);
        _missileCrosshair.gameObject.SetActive(flag);
        if( flag == true )
        {
            RaycastTarget();
            DrawRectTransform();
            CheckReady();
        }
        checkTargetAlive();
 
    }

    private void CheckReady()
    {
        if( (_preLockingTarget == null) || _preLockingTarget.GetInstanceID().Equals(_lockingTarget?.GetInstanceID())  )
        {
            if(  _lockingDelay > _lockingTime )
            {
                _realTarget = _lockingTarget;
                _text.text = (( _realTarget == null ) ? "" : "Target: \n" + _realTarget.gameObject.name) ;
                _text.color = Color.green;
                _crosshiarImage.color = Color.green;
            }
            else
            {
                LockingPules();
                _realTarget = null;    
                _text.text = (( _lockingTarget == null ) ? "" : "Locking Target: \n" + _lockingTarget.gameObject.name) ;     
                _text.color = Color.red;
                _lockingDelay += Time.fixedDeltaTime;
                _crosshiarImage.color = Color.red;
            }
        }
        else{
            _lockingDelay = 0;
            _realTarget = null;     
            _text.text = "";    
            _crosshiarImage.color = Color.red;
        }
        
    }

    private void checkTargetAlive()
    {
        if( _lockingTarget == null )
        {
            ResetSystem();
        }
    }
    private void RaycastTarget()
    {

        Vector3 screenPoint = Camera.main.WorldToScreenPoint( _aimTarget.transform.position );

        for (int x = -1; x <= 1 ; x++)
        {
            for (int y = -1; y <= 1 ; y++)
            {
                for (int z = -1; z <= 1 ; z++)
                {
                    Ray ray = Camera.main.ScreenPointToRay( screenPoint + (new Vector3(x, y ,z) * 20) );
                    RaycastHit hitInfo;

                    if( Physics.Raycast( ray, out hitInfo, 3000, _layerMask ) ) {
                        
                        GameObject obj = hitInfo.transform.gameObject;
                        _preLockingTarget = _lockingTarget;
                        _lockingTarget = obj;  
                        return;                  
                    }
                }
            }
        }
    }

    private void LockingPules()
    {
        if( _pulseDelay > _pulseTime  )
        {
            _pulseDelay = 0;
            _pulseTime = Mathf.Max(((_lockingTime - _lockingDelay) / (_lockingTime * 3)), 0.05f);
            _lockingSound.Play();
        }
        else{
            _pulseDelay += Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {

    }

    private void OnTriggerExit(Collider collider)
    {
        if( (_layerMask.value & 1 << collider.gameObject.layer ) == 1 << collider.gameObject.layer )
        {
            _lockingTarget = null;
            ResetSystem();
        }
    }

    private void DrawRectTransform()
    {
        if( _lockingTarget != null )
        {
            Rect visualRect = RendererBoundsInScreenSpace( _lockingTarget.GetComponentInChildren<Collider>() );

            _crosshair.position = new Vector2( visualRect.xMin, visualRect.yMin );

            _crosshair.sizeDelta = new Vector2( visualRect.width, visualRect.height ); 

            _text.gameObject.transform.position = new Vector2( visualRect.xMin + visualRect.width + (visualRect.width / 5), visualRect.yMin + visualRect.height - (visualRect.height / 20));
        }
    }
    private void ResetSystem()
    {
        _text.text = ""; 
        _lockingDelay = 0;
        _pulseDelay = 0;
        _realTarget = null;
        _crosshair.sizeDelta = new Vector2(0, 0);
    }

    static Vector3[] screenSpaceCorners;
	static Rect RendererBoundsInScreenSpace(Collider r) {
		// This is the space occupied by the object's visuals
		// in WORLD space.
		Bounds bigBounds = r.bounds;

		if(screenSpaceCorners == null)
			screenSpaceCorners = new Vector3[8];

		Camera theCamera = Camera.main;

		// For each of the 8 corners of our renderer's world space bounding box,
		// convert those corners into screen space.
		screenSpaceCorners[0] = theCamera.WorldToScreenPoint( new Vector3( bigBounds.center.x + bigBounds.extents.x, bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z ) );
		screenSpaceCorners[1] = theCamera.WorldToScreenPoint( new Vector3( bigBounds.center.x + bigBounds.extents.x, bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z ) );
		screenSpaceCorners[2] = theCamera.WorldToScreenPoint( new Vector3( bigBounds.center.x + bigBounds.extents.x, bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z ) );
		screenSpaceCorners[3] = theCamera.WorldToScreenPoint( new Vector3( bigBounds.center.x + bigBounds.extents.x, bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z ) );
		screenSpaceCorners[4] = theCamera.WorldToScreenPoint( new Vector3( bigBounds.center.x - bigBounds.extents.x, bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z ) );
		screenSpaceCorners[5] = theCamera.WorldToScreenPoint( new Vector3( bigBounds.center.x - bigBounds.extents.x, bigBounds.center.y + bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z ) );
		screenSpaceCorners[6] = theCamera.WorldToScreenPoint( new Vector3( bigBounds.center.x - bigBounds.extents.x, bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z + bigBounds.extents.z ) );
		screenSpaceCorners[7] = theCamera.WorldToScreenPoint( new Vector3( bigBounds.center.x - bigBounds.extents.x, bigBounds.center.y - bigBounds.extents.y, bigBounds.center.z - bigBounds.extents.z ) );

		// Now find the min/max X & Y of these screen space corners.
		float min_x = screenSpaceCorners[0].x;
		float min_y = screenSpaceCorners[0].y;
		float max_x = screenSpaceCorners[0].x;
		float max_y = screenSpaceCorners[0].y;

		for (int i = 1; i < 8; i++) {
			if(screenSpaceCorners[i].x < min_x) {
				min_x = screenSpaceCorners[i].x;
			}
			if(screenSpaceCorners[i].y < min_y) {
				min_y = screenSpaceCorners[i].y;
			}
			if(screenSpaceCorners[i].x > max_x) {
				max_x = screenSpaceCorners[i].x;
			}
			if(screenSpaceCorners[i].y > max_y) {
				max_y = screenSpaceCorners[i].y;
			}
		}

		return Rect.MinMaxRect( min_x, min_y, max_x, max_y );

	}

    public GameObject GetTargetObject()
    {
        return _realTarget;
    }

    public GameObject GetLockingTarget()
    {
        return _lockingTarget;
    }
    public bool LockingCompleted()
    {
        return (_realTarget != null);
    }
}