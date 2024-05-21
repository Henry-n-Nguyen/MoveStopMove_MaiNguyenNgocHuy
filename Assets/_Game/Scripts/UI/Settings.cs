using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : UICanvas
{
    public void ResumeGame()
    {
        UIManager.instance.CloseDirectly<Settings>();

        UIManager.instance.OpenUI<DynamicJoyStick>();
        UIManager.instance.OpenUI<Ingame>();
    }

    public void ReturnHome()
    {
        UIManager.instance.CloseDirectly<Settings>();

        UIManager.instance.OpenUI<MainMenu>();
    }
}
