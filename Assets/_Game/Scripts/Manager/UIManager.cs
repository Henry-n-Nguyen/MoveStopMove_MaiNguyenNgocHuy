using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    Dictionary<System.Type, UICanvas> activatedCanvases = new Dictionary<System.Type, UICanvas>();
    Dictionary<System.Type, UICanvas> canvasPrefabs = new Dictionary<System.Type, UICanvas>();

    [SerializeField] Transform holder;

    [SerializeField] UICanvas[] canvases;

    private void Awake()
    {
        // Load UI canvas
        for (int i = 0; i < canvases.Length; i++)
        {
            canvasPrefabs.Add(canvases[i].GetType(), canvases[i]);
        }
    }

    // Open Canvas
    public T OpenUI<T>() where T : UICanvas
    {
        T currentCanvas = GetUI<T>();

        currentCanvas.Open();
        currentCanvas.Setup();

        return currentCanvas as T;
    }
    
    // Close Canvas after time
    public void CloseUI<T>(float time) where T : UICanvas
    {
        if (IsUILoaded<T>())
        {
            activatedCanvases[typeof(T)].Close(time);
        }
    }

    // Close Canvas directly
    public void CloseDirectly<T>() where T : UICanvas
    {
        if (IsUILoaded<T>())
        {
            activatedCanvases[typeof(T)].CloseDirectly();
        }
    }

    // Check Canvas have been created or not
    public bool IsUILoaded<T>() where T : UICanvas
    {
        return activatedCanvases.ContainsKey(typeof(T)) && activatedCanvases[typeof(T)] != null;
    }

    // Check Canvas have been activated or not
    public bool IsUIOpened<T>() where T : UICanvas
    {
        return IsUILoaded<T>() && activatedCanvases[typeof(T)].gameObject.activeSelf;
    }

    // Get Activated Canvas
    public T GetUI<T>() where T : UICanvas
    {
        if (!IsUILoaded<T>())
        {
            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab, holder);
            activatedCanvases[typeof(T)] = canvas;
        }

        return activatedCanvases[typeof(T)] as T;
    }

    public T GetUIPrefab<T>() where T : UICanvas
    {
        return canvasPrefabs[typeof(T)] as T;
    }

    // Close all canvas
    public void CloseAll()
    {
        foreach (var canvas in activatedCanvases)
        {
            var value = canvas.Value;

            if (value != null && value.gameObject.activeSelf)
            {
                value.CloseDirectly();
            }
        }
    }
}
