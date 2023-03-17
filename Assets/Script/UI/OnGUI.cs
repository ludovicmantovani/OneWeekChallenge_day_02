using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private OrbitControls orbitControls;

    private bool _isOpen = true;
    private bool _swipe = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (orbitControls)
        {
            orbitControls.LockPlayerControls = true;
            orbitControls.MakeShiftCamera();
        }
        _swipe = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (orbitControls)
        {
            orbitControls.LockPlayerControls = false;
            orbitControls.MakeShiftCamera();
        }
        _swipe = true;
    }

    void Start()
    {
        
    }


    void Update()
    {
        if (_swipe)
        {
            float finalVal = _isOpen ? -450f : 0f;
            float tmp = Mathf.Lerp(transform.localPosition.x, finalVal, Time.deltaTime * 2f);
            if (Mathf.Abs(tmp - finalVal) <= 0.1)
            {
                _swipe = false;
                _isOpen = !_isOpen;
            }
            Vector3 localPosition = new Vector3(
                tmp,
                transform.localPosition.y,
                transform.localPosition.z);
            transform.localPosition = localPosition;
        }
    }
}
