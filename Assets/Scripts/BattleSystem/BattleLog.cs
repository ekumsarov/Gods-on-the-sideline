using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using GameEvents;
using System.IO;

public class BattleLog // Info Manager
{
    private static BattleLog instance = null;

    public static void NewGame()
    {
        if (BattleLog.instance != null)
            BattleLog.instance = null;

        BattleLog.instance = new BattleLog();

        BattleLog.instance._log = new List<string>();
        BattleLog.instance._viewLog = new List<string>();
        
    }

    private List<string> _viewLog;
    private List<string> _log;
    private UIIconText _label;

    public static void ActivateVew(UIIconText label)
    {
        BattleLog.instance._label = label;
    }

    public static void PushLog(string text, bool showInViewLog = true)
    {
        text.Trim();
        BattleLog.instance._log.Add(text);
        if (showInViewLog)
            BattleLog.instance._viewLog.Add(text);

        if (BattleLog.instance._label == null)
            return;

        string labelText = "";
        for(int i = 0; i < BattleLog.instance._viewLog.Count; i++)
        {
            labelText += BattleLog.instance._viewLog[i] + "\n";
        }

        BattleLog.instance._label.IconText.Text(labelText);
        BattleLog.instance._label.IconText.ShowComplete();
    }

    public static void SaveLog()
    {
        if (BattleLog.instance._log.Count <= 0)
            return;

        string labelText = "";
        for (int i = 0; i < BattleLog.instance._log.Count; i++)
        {
            labelText += BattleLog.instance._log[i] + "\n";
        }
    
        File.WriteAllText("C:/Users/Ёрвин/Documents/BattleLog.txt", labelText);
        //StreamWriter writer = new StreamWriter(, true);
        //writer.Write(labelText);
    }
}