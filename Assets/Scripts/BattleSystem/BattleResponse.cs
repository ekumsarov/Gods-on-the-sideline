using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;
using System.Linq;
using Lodkod;
using System;
using BattleEngine;

public class BattleResponse
{
    public delegate void ResponseDelegate(ActionSourceData data);
    private ResponseDelegate _delegates;

    private static BattleResponse instance = null;
    public static void NewGame()
    {
        BattleResponse.instance = new BattleResponse();

        //BattleResponse.instance._listeners.Clear();
        //BattleResponse.instance._listeners = null;
        BattleResponse.instance._listeners = new Dictionary<BattleResponseActions, ResponseDelegate>();

        BattleResponse.instance._listeners.Add(BattleResponseActions.GetDamage, BattleResponse.instance._delegates);
        BattleResponse.instance._listeners.Add(BattleResponseActions.AppliedEffect, BattleResponse.instance._delegates);
    }

    private Dictionary<BattleResponseActions, ResponseDelegate> _listeners;

    public static void AddListener(BattleResponseActions type, ResponseDelegate del)
    {
        BattleResponse.instance._listeners[type] += del; 
    }

    public static void RemoveListener(BattleResponseActions type, ResponseDelegate del)
    {
        BattleResponse.instance._listeners[type] -= del;
    }

    public static void Notify(ActionSourceData data, BattleResponseActions type)
    {
        BattleResponse.instance._listeners[type]?.Invoke(data);
    }
}

public class BattleListener
{
    private BattleResponseActions ListenerType;
    private CardTargetSide Side = CardTargetSide.Any;
    private CardTargetType Type = CardTargetType.Any;
    private string ResponseID;

    public delegate void ResponseDelegate(ActionSourceData data);
    private ResponseDelegate _delegates;

    public static BattleListener Create(ResponseDelegate callback)
    {
        BattleListener temp = new BattleListener();

        temp._delegates += callback;

        return temp;
    }

    public void SetSide(CardTargetSide side)
    {
        this.Side = side;
    }

    public void SetType(CardTargetType type)
    {
        this.Type = type;
    }

    public void SpecialID(string id)
    {
        this.ResponseID = id;
    }

    public void CheckAndNotify(ActionSourceData data)
    {
        _delegates?.Invoke(data);
    }
}

public class BattleResponseData
{
    public int CountActiveSide;
    public int CountTargetSide;
    public string IDActiveSide;
    public string IDTargetSide;

    public int AddiotionalCount;

    public string EffectID;
}