using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SliderJoystick : MonoBehaviour
{
    public event Action FirstTouch;
    public float HorizontalPosition { get; private set; }
    [SerializeField] private float ScreenPart = 0.3f;
    
    private float SavedTouchPosition;
    private float joystickCenter;
    private float screenWidth = Screen.width;
    private bool isFirstTouch = true;
    private float JoystickToScreenPoint { get { return (screenWidth / 2 * ScreenPart); } }
    private float test;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isFirstTouch)
            {
                FirstTouch?.Invoke();
                isFirstTouch = false;
            }

            joystickCenter = Input.mousePosition.x - SavedTouchPosition * JoystickToScreenPoint;
        }

        if (Input.GetMouseButton(0))
        {
            float distanceNormallized = (Input.mousePosition.x - joystickCenter) / JoystickToScreenPoint;
            distanceNormallized /= 2f;

            if (distanceNormallized > 1)
            {
                joystickCenter += (distanceNormallized - 1) * JoystickToScreenPoint;
                distanceNormallized = 1;
            }
            else if (distanceNormallized < -1)
            {
                joystickCenter += (distanceNormallized + 1);
                distanceNormallized = -1;
            }
            HorizontalPosition = distanceNormallized;
            SavedTouchPosition = HorizontalPosition * 2f;
        }
    }
}
