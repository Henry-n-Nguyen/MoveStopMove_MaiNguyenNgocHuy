using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using HuySpace;
using System;

public class DynamicJoyStick : MonoBehaviour
{
    public static DynamicJoyStick instance;

    [SerializeField] private GameObject joyStickHolder;
    [SerializeField] private Transform joyStickTransform;
    [SerializeField] private Image joyStick;

    // Event Actions
    public event Action OnTouchScreenAction;
    public event Action OnReleaseScreenAction;

    private Coroutine wakeUserUpAction;

    // Public
    public bool IsPressed { get; private set; } = false;

    private void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        OnInit();
    }

    // Update is called once per frame
    void Update()
    {
        GatherMouseInput();
    }

    private void OnInit()
    {
        OnRelease();
    }

    private void OnRelease()
    {
        OnReleaseScreenAction?.Invoke();
    }
    private void OnTouch()
    {
        OnTouchScreenAction?.Invoke();
    }

    private void GatherMouseInput()
    {
        if (!IsPressed)
        {
            joyStickTransform.position = Input.mousePosition;
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnTouch();

            IsPressed = true;
            if (wakeUserUpAction != null) StopCoroutine(wakeUserUpAction);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            IsPressed = false;
            wakeUserUpAction = StartCoroutine(WakeUserUp(2f));
        }
    }

    private IEnumerator WakeUserUp(float time)
    {
        yield return new WaitForSeconds(time);

        OnInit();
    }
}
