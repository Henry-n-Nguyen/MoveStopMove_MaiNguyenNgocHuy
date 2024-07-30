using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    protected bool destroyOnClose;

    private void Awake()
    {
        RectTransform rect = GetComponent<RectTransform>();
        float ratio = (float) Screen.width / Screen.height;
        if (ratio > 2.1f)
        {
            Vector2 leftBottom = rect.offsetMin;
            Vector2 rightTop = rect.offsetMax;

            leftBottom.y = 0;
            rightTop.y = -100f;

            rect.offsetMin = leftBottom;
            rect.offsetMax = rightTop;
        }
    }

    private void Start()
    {
        Setup();
    }

    // Call before active Canvas
    public virtual void Setup()
    {
        
    }

    //Open canvas
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }

    //Close canvas with time
    public virtual void Close(float time)
    {
        Invoke(nameof(CloseDirectly), time);
    }

    //Close canvas directly
    public virtual void CloseDirectly()
    {
        if (destroyOnClose)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
