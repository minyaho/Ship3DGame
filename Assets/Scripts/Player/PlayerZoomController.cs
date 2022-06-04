using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerZoomController : MonoBehaviour
{
   /* [SerializeField] private CinemachineFreeLook freeLookCameraToZoom;
    [SerializeField] private float zoomSpeed = 3.0f;
    [SerializeField] private float zoomAcceleration = 2.5f;
    [SerializeField] private float zoomInnerRange = 3.0f;
    [SerializeField] private float zoomOuterRange = 50.0f;
    [SerializeField] private float zoomYAxis = 0f;

    private float currentMiddleRigRadius = 10.0f;
    private float newMiddleRigRadius = 10.0f;
    public float ZoomYAxis
    {
        get { return zoomYAxis; }
        set 
        {
            if( zoomYAxis == value ) return;
            zoomYAxis = value;
            AdjustCameraZoomIndex( ZoomYAxis );
        }
    }

    private void Awake()
    {
        inputProvider.FindActionMap("Free look Camera").FindAction("MouseZoom").performed += cntxt => ZoomYAxis = cntxt.ReadValue<float>();
        inputProvider.FindActionMap("Free look Camera").FindAction("MouseZoom").canceled += cntxt => ZoomYAxis = 0.0f;
    }
    private void OnEnable()
    {
        inputProvider.FindAction("MouseZoom").Enable();
    }
    private void OnDisable()
    {
        inputProvider.FindAction("MouseZoom").Disable();
    }

    public void AdjustCameraZoomIndex(float zoomYAxis){
        if( zoomYAxis == 0 ){ return; }
        if( zoomYAxis < 0 )
        {
            newMiddleRigRadius = currentMiddleRigRadius + zoomSpeed;
        }
        if( zoomYAxis > 0 )
        {
            newMiddleRigRadius = currentMiddleRigRadius - zoomSpeed;
        }
    }

    private void LateUpdate()
    {
        UpdateZoomLevel();
    }

    private void UpdateZoomLevel()
    {
        if( currentMiddleRigRadius == newMiddleRigRadius ){ return; }
        currentMiddleRigRadius = Mathf.Lerp(currentMiddleRigRadius, newMiddleRigRadius, zoomAcceleration * Time.deltaTime);
        currentMiddleRigRadius = Mathf.Clamp(currentMiddleRigRadius, zoomInnerRange, zoomOuterRange);

        freeLookCameraToZoom.m_Orbits[1].m_Radius = currentMiddleRigRadius;
        freeLookCameraToZoom.m_Orbits[0].m_Height = freeLookCameraToZoom.m_Orbits[1].m_Radius;
        freeLookCameraToZoom.m_Orbits[2].m_Height = -freeLookCameraToZoom.m_Orbits[1].m_Radius;
    }*/

}
