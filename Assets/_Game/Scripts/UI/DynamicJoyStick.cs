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
    [SerializeField] private Image splittedjoyStick;
    [SerializeField] private Image joyStick;

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

        splittedjoyStick.enabled = false;
        joyStick.color = new Color(joyStick.color.r, joyStick.color.g, joyStick.color.b, 0f);
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
            isPressed = true;

            splittedjoyStick.enabled = true;
            joyStick.color = new Color(joyStick.color.r, joyStick.color.g, joyStick.color.b, 1f);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnInit();
        }
    }
}
