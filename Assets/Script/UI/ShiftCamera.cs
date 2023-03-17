using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftCamera : MonoBehaviour
{
    [SerializeField] private float smoothTime = 2f;

    private float _shiftCameraValue = 0f;
    private float _saveCameraLocalX = 0f;
    private bool _isShift = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (_isShift)
        {
            float tmp = Mathf.Lerp(transform.localPosition.x, _shiftCameraValue, Time.deltaTime * smoothTime);
            if (Mathf.Abs(tmp - _shiftCameraValue) <= 0.01) _isShift = false;
            Vector3 localPosition = new Vector3(
                tmp,
                transform.localPosition.y,
                transform.localPosition.z);
            transform.localPosition = localPosition;
        }
    }


    public void Shift(float value)
    {
        _shiftCameraValue = value;
        _saveCameraLocalX = transform.localPosition.x;
        _isShift = true;
    }
}
