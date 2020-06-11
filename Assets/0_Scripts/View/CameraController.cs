using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("Speed scrolling for the camera")]
    [SerializeField]
    private float _panSpeed = 10f;
    [SerializeField]
    private float _panBorderThickness = 10f;
    [SerializeField]
    private float _scrollSpeed;
    [Tooltip("Min and max for the zoom")]
    [SerializeField]
    private float _minY, _maxY;

    [Tooltip("Clamped position for the camera")]
    [SerializeField]
    private float _panLimitXNegative, _panLimitXPositive, _panLimitZNegative, _panLimitZPositive;

    void Update()
    {
        Vector3 pos = transform.position;

        #region Moving Camera
        if (Input.mousePosition.y >= Screen.height - _panBorderThickness || Input.GetKey(KeyCode.UpArrow))
        {
            pos.z += _panSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.y <= _panBorderThickness || Input.GetKey(KeyCode.DownArrow))
        {
            pos.z -= _panSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.x >= Screen.width - _panBorderThickness || Input.GetKey(KeyCode.RightArrow))
        {
            pos.x += _panSpeed * Time.deltaTime;
        }

        if (Input.mousePosition.x <= _panBorderThickness || Input.GetKey(KeyCode.LeftArrow))
        {
            pos.x -= _panSpeed * Time.deltaTime;
        }

        //TODO Create a map
        pos.x = Mathf.Clamp(pos.x, _panLimitXNegative, _panLimitXPositive);
        pos.z = Mathf.Clamp(pos.z, _panLimitZNegative, _panLimitZPositive);
        #endregion

        #region Mouse Scroll Wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float scrollKeyboard = Input.GetAxis("ScrollKeyboard");

        pos.y -= scroll * _scrollSpeed * Time.deltaTime;
        pos.y -= scrollKeyboard * _scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, _minY, _maxY);
        #endregion

        transform.position = pos;
    }
}