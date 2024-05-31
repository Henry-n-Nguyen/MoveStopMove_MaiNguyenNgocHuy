using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using HuySpace;

public class DynamicJoyStick : UICanvas
{
    public static DynamicJoyStick instance;

    [SerializeField] private GameObject joyStickHolder;
    [SerializeField] private Transform joyStickTransform;
    [SerializeField] private Image joyStick;

    private bool isTouchedBefore = false;

    private bool isPressed;
    public bool IsPressed => isPressed;

    private void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        OnInit();
    }

    private void OnInit()
    {
        isPressed = false;

        isTouchedBefore = false;
    }

    // Update is called once per frame
    void Update()
    {
        GatherMouseInput();
    }

    private void GatherMouseInput()
    {
        if (!isPressed)
        {
            joyStickTransform.position = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!isTouchedBefore)
            {
                isTouchedBefore = true;

                GamePlayManager.instance.OnTouch();
            }

            isPressed = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnInit();
        }
    }
}
