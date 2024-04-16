using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using HuySpace;

public class DynamicJoyStick : MonoBehaviour
{
    public static DynamicJoyStick instance;

    [SerializeField] private GameObject joyStickHolder;
    [SerializeField] private Image splittedjoyStick;
    [SerializeField] private Image joyStick;

    private bool isPressed;
    public bool IsPressed => isPressed;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        isPressed = false;

        splittedjoyStick.color = new Color(splittedjoyStick.color.r, splittedjoyStick.color.g, splittedjoyStick.color.b, 0f);
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
            joyStickHolder.transform.position = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isPressed = true;

            splittedjoyStick.color = new Color(splittedjoyStick.color.r, splittedjoyStick.color.g, splittedjoyStick.color.b, 1f);
            joyStick.color = new Color(joyStick.color.r, joyStick.color.g, joyStick.color.b, 1f);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OnInit();
        }
    }
}
