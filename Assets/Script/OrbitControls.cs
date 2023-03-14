using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitControls : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool lockPlayerControls = false;
    [SerializeField] private float distance = 5.0f;
    [SerializeField] private float xSpeed = 120.0f;
    [SerializeField] private float ySpeed = 120.0f;
    [SerializeField] private float yMinLimit = -20f;
    [SerializeField] private float yMaxLimit = 80f;
    [SerializeField] private float distanceMin = 2f;
    [SerializeField] private float distanceMax = 5f;
    [SerializeField] private float smoothTime = 2f;
    [SerializeField] private float zoomSpeed = 5f;

    private Vector3 _position;
    private Quaternion _rotation;

    private float _xDeg = 0.0f;
    private float _yDeg = 0.0f;
    private float _initXDeg = 0.0f;
    private float _initYDeg = 0.0f;
    private float _currentDistance;
    private float _desiredDistance;
    private Quaternion _currentRotation;
    private Quaternion _desiredRotation;
    private Vector3 _positionOffset;
    private Vector3 _initPosition;

    public bool LockPlayerControls { get => lockPlayerControls; set => lockPlayerControls = value; }

    void Start()
    {
        var angles = transform.eulerAngles;
        _xDeg = angles.y;
        _yDeg = angles.x;

        _initXDeg = _xDeg;
        _initYDeg = _yDeg;

        _currentRotation = transform.rotation;
        _desiredRotation = transform.rotation;

        _position = transform.position;
        _positionOffset = _position - target.position;
        _initPosition = new Vector3(_position.x, _position.y, _position.z);

        _currentDistance = distance;
        _desiredDistance = distance;

        // lock du curseur
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!target)
            return;

        // rotation de la caméra
        if (Input.GetMouseButton(0) && !lockPlayerControls)
        {
            _xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            _yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }

        if (Input.GetKeyDown(KeyCode.R) && !lockPlayerControls)
        {
            ResetPosition();
        }

        // limitation rotation verticale
        _yDeg = ClampAngle(_yDeg, yMinLimit, yMaxLimit);

        // calculer rotation
        _desiredRotation = Quaternion.Euler(_yDeg, _xDeg, 0);
        _currentRotation = transform.rotation;

        // lissage de la rotation
        _rotation = Quaternion.Lerp(_currentRotation, _desiredRotation, Time.deltaTime * smoothTime);
        transform.rotation = _rotation;

        // zoom de la caméra molette souris
        if (!lockPlayerControls)
            _desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        _desiredDistance = Mathf.Clamp(_desiredDistance, distanceMin, distanceMax);

        // calculer position
        _currentDistance = Mathf.Lerp(_currentDistance, _desiredDistance, Time.deltaTime * smoothTime);
        _position = target.position - (_rotation * Vector3.forward * _currentDistance + _positionOffset);

        transform.position = _position;
    }


    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    public void ResetPosition()
    {
        transform.position = _initPosition;
        _xDeg = _initXDeg;
        _yDeg = _initYDeg;
    }
}
