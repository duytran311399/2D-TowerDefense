using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : SingletonDontDestroyMono<ScreenManager>
{
    public ScreenMainMenu SL_MainMenu;
    public ScreenGamePlay SL_GamePlay;
    public ScreenLevelSellector SL_LevelSellector;

    public void Start()
    {
        CloseAllScreen();
        SL_MainMenu.Open(); ;
    }

    public void CloseAllScreen()
    {
        SL_MainMenu.Close();
        SL_GamePlay.Close();
        SL_LevelSellector.Close();
    }
}
