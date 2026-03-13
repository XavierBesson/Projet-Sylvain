using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EMainMenuKey
{
    PLAY,
    TUTO,
    QUIT,
    MAINMENU
}



public class MainMenuKey : UIObject
{
    [SerializeField] private EMainMenuKey key;

    public EMainMenuKey Key { get => key; set => key = value; }
}
