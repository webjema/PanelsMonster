﻿
public class PanelsMonsterConfig {

}


public enum PanelName
{
    LoadingPanel,
    WelcomePanel,
    BonusPanel,
    InfoPanel,
    SmallInfoPanel,
    CustomInfoPanel,
}

public enum PanelPropertyName
{
    title,
    description,
    message,
    button1,
    button2
}

public enum PanelActionName
{
    none,
    close,
    button1click,
    button2click,
    button3click,

    logout
}

public enum ScreensName
{
    None,

    Background1,
    Background2,

    SplashScreenScene,
    LobbyScreenScene,
    GameMenuScreenScene,
    GameBoardScreenScene,

    LoginScreenScene,
    ProfileScreenScene,
    SettingsScreenScene,

}