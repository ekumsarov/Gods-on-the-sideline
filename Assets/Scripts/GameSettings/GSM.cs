using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleEffects;

public class GSM 
{
    private static GameSettingManager _instanse;
    private static GSM _gsmInstanse;
    
    public static void NewGame()
    {
        if (GSM._instanse == null)
            GSM._instanse = GameObject.Find("GameSettings").GetComponent<GameSettingManager>();

        GSM._gsmInstanse = new GSM();
        GSM._gsmInstanse._startSettings = GSM._instanse.Data;
        GSM._gsmInstanse._actualSettings = GSM._instanse.Data;
    }

    public static GameSettings GameSettings => GSM._gsmInstanse._actualSettings;

    private GameSettings _actualSettings;
    private GameSettings _startSettings;

    public static void AddSettingsEffect(BattleEffect effect)
    {

    }
}


