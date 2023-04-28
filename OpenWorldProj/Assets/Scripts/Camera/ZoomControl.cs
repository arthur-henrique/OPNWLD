using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class ZoomControl : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputProvider;
    [SerializeField] private CinemachineFreeLook freeLookCameraToZoom;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float zoomAcceleration = 2.5f;
    [SerializeField] private float zoomInnerRange = 3f;
    [SerializeField] private float zoomOuterRange = 50f;

    private float currentMiddleRingRadius = 10f;
    private float newMiddleRingRadius = 10f;

    [SerializeField] private float zoomYAxis = 0f;
    public float ZoomYAxis
    {
        get { return zoomYAxis; }
        set
        {
            if (zoomYAxis == value) return;

        }
    }

}
