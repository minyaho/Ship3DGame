using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class LockSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _crosshair;

    [SerializeField] private GameObject _aimTarget;
   

    [SerializeField] private LayerMask _layerMask;
    // =============================================

    private LockedDictionary _lockedDict;
    private GameObject targetObject;

    private void Start()
    {
        _lockedDict = GetComponent<LockedDictionary>();
    }

    private void FixedUpdate()
    {
        RaycastTarget();
        DrawRectTransform();
        checkTargetAlive();
    }

    private void checkTargetAlive()
    {
        if( targetObject == null )
        {
            ResetRectTransfrom();
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
                    Ray ray = Camera.main.ScreenPointToRay( screenPoint + (new Vector3(x, y ,z) * 10) );
                    RaycastHit hitInfo;

                    if( Physics.Raycast( ray, out hitInfo, 3000, _layerMask ) ) {
                        
                        GameObject obj = hitInfo.transform.gameObject;

                        targetObject = obj;                        
                    }
                }
            }
        }

    }

    private void OnTriggerEnter(Collider collider)
    {

    }

    private void OnTriggerExit(Collider collider)
    {
        if( (_layerMask.value & 1 << collider.gameObject.layer ) == 1 << collider.gameObject.layer )
        {
            targetObject = null;
            ResetRectTransfrom();
        }
    }

    private void DrawRectTransform()
    {
        if( targetObject != null )
        {
            Rect visualRect = RendererBoundsInScreenSpace( targetObject.GetComponentInChildren<Collider>() );

            _crosshair.position = new Vector2( visualRect.xMin, visualRect.yMin );

            _crosshair.sizeDelta = new Vector2( visualRect.width, visualRect.height ); 
        }
    }
    private void ResetRectTransfrom()
    {
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
        return targetObject;
    }
}