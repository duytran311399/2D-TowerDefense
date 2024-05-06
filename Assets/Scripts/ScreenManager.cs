using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : SingletonDontDestroyMono<ScreenManager>
{
    public ScreenMainMenu SL_MainMenu;
    public ScreenGamePlay SL_GamePlay;

    public void Start()
    {
        SL_MainMenu.Close(); ;
    }
}
