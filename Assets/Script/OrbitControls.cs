using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitControls : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 10.0f;
    [SerializeField] private float xSpeed = 120.0f;
    [SerializeField] private float ySpeed = 120.0f;
    [SerializeField] private float yMinLimit = -20f;
    [SerializeField] private float yMaxLimit = 80f;
    [SerializeField] private float distanceMin = .5f;
    [SerializeField] private float distanceMax = 15f;
    [SerializeField] private float zoomRate = 1f;
    [SerializeField] private float smoothTime = 2f;

    private Vector3 _position;
    private Quaternion _rotation;

    private float _xDeg = 0.0f;
    private float _yDeg = 0.0f;
    private float _currentDistance;
    private float _desiredDistance;
    private Quaternion _currentRotation;
    private Quaternion _desiredRotation;
    private Vector3 _positionOffset;

    void Start()
    {
        var angles = transform.eulerAngles;
        _xDeg = angles.y;
        _yDeg = angles.x;

        _currentRotation = transform.rotation;
        _desiredRotation = transform.rotation;

        _position = transform.position;
        _positionOffset = _position - target.position;

        _currentDistance = distance;
        _desiredDistance = distance;

        // lock du curseur
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!target)
            return;

        // rotation de la caméra
        if (Input.GetMouseButton(0))
        {
            _xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            _yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
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
        _desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 100 * zoomRate * Mathf.Abs(_desiredDistance);
        _desiredDistance = Mathf.Clamp(_desiredDistance, distanceMin, distanceMax);

        // calculer position
        _position = target.position - (_rotation * Vector3.forward * _desiredDistance + _positionOffset);
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
}
