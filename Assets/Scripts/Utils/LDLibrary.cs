using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using SimpleJSON;
using System.Reflection;

namespace Lodkod
{
    using EventPackSystem;

    public enum CardActionType { Attack, ApplyEffect, Heal, RemoveEffect, Summon, Uniqe, MassiveAttack, MassiveHeal }
    public enum EnemyActionType { Attack, ApplyOnEnemy, ApplyOnPlayer, ApplyEffectAny,  Heal, RemoveEffect, Summon, MassiveAttack, MassiveHeal, AttackWithEffect, HealWithEffect }
    public enum UnitActionType { Attack, ApplyOnEnemy, ApplyOnPlayer, ApplyEffectAny, Heal, RemoveEffect, Summon, MassiveAttack, MassiveHeal, AttackWithEffect, HealWithEffect, Uniqe }
    public enum EnemyType { DPS, Healer, Combined, Summoner, Boss }
    public enum UnitAIType { DPS, Healer, Combined, Summoner, Boss }
    public enum CardTargetSide { Any, Player, Enemy }

    public enum ConditionUseType { ForAvaliable, ForSelection,  ForSelectionAndAvaliable }
    public enum CardTargetType { Any, Class, EnemyType, Summon, ID, Hero }
    public enum CardTarget { FirstSelect, FirstSelected, SelectTarget, SelectedTargets, SelectAnother, AllNotSelected, AllSelected, Group, Random, Player, FromCondition, AutoSelect }
    public enum EffectType { Empty, Fire, Ice, Poison, StormСlouds, Dodge, Power, Bleeding, Shock, Defense, Stunn, Blindless, UpSkill, Provocation, DamageBuff, DamageDebuff, DefendEffect, BySpecID }
    public enum EffectBuffType { Positive, Negative }
    public enum DeckType { Deck, Discard, Player }
    public enum AfterPlayEffect { Dech, Discard, Burn }
    public enum UnitType { Unknown, Hero, Enemy, Boss, Familiar }

    public enum PlayerTask { GoToIsland, GoToObject, Interact }

    public enum StatType { Wood=0, Stone=1, Food=2, Skystone=3, Unit=4 };

    public enum GameState { Game = 0, War = 1, EventWorking = 2, Action = 3, StartReacting = 4, Reacting = 5, ContextWorking = 6 };

    public enum PlatformType { PC = 0, Switch = 1}

    public enum ActionChoiceType { Simple = 0, Skillcheck = 1, AdditionalText = 2, Resource = 3, Condition = 4 }

    public enum EnemyFeatureResponse { Attack, Heal, EffectApplyOnEnemy, EffectApplied, Death, GetDamaged }

    public enum UnitFeatureResponse { Attack, Heal, EffectApplyOnEnemy, EffectApplied, Death, GetDamaged }

    public enum BattleResponseActions { Empty, Damage, GetDamage, Heal, GetHeal, ApplyEffect, AppliedEffect, Death, NextRound, PlayerTurn, EnemyTurn, Summoned }

    public enum ConditionType
    {
        NoType,
        Stat,
        Flag,
        Quest,
        Loot,
        Daypart,
        HeroRequire,
        EffectCondition,
        LandscapeCondition
    }

    public enum IconInteractType
    {
        TopLocation,
        SubLocation,
        Object,
        Player
    }

    public enum SkillType
    {
        Light,
        Dark,
        Elementals,
        Natural,
        Spirit,
        Craft
    }

    public enum EnemySubType
    {
        Any,
        Animal,
        Wizard,
        Small,
        Boss,
        NotBoss
    }

    /*
     *  Перечисления и типы
     * 
     */
    public enum MeshType
    {
        Nill = -1,
        Empty,
        OnIsland,
        ForBuild,
        Build,
        m_Wood,
        m_Skystone,
        m_Mine,
        m_Spike,
        ReserveForIsland,
        IslandBegining,
        IslandEnterLeft,
        IslandEnterRightEmpty
    }

    /*
    * Build Type
    */
    public enum BuildType
    {
        MainBuild,
        ProductBuild,
        DefenseBuild,
        SpecialBuild,
        MainSpecial
    }

    public enum Containers
    {
        Nill,
        Units,
        Enviroment,
        Islands,
        Centers,
        Bonfires,
        Uniqs,
        Resources
    }
    
    public enum TooltipObject
    {
        UI,
        Game
    }

    public enum TooltipFit
    {
        Auto,
        Left,
        Right,
        Up,
        Down,
        OnScreen

    }

    public enum TooltipTimeMode
    {
        Tootip,
        Dialog,
        MousOn,
        MouseOnClick,
        Click,
        ObjectManagment,
        ButtonClick
    }

    public enum TooltipFillMode
    {
        Type,
        Instantly

    }

    public enum DayPart
    {
        Morning,
        Afternoon,
        Evening,
        Night
    }

    public enum TriggerType
    {
        GameStateChanged,
        ChangedDaysPart,
        NewDay,
        ProductTick,
        FinishBuild,
        StatChanged,
        NewThreat,
        ActionEventEnd,
        IslandUpdate,
        IslandFirstExplore,
        IslandCompleteFly,
        EffectDestroyed,
        FlagChanged
    }

    /**
     *  LDMeshPoint
     */
    [System.Serializable]
    public struct LDMeshPoint
    {
        [SerializeField]
        public int x;
        public int y;
    }

    [System.Serializable]
    public struct CellPoint
    {
        [SerializeField]
        public int x;
        public int y;

        public static CellPoint zero
        {
            get
            {
                CellPoint temp = new CellPoint
                {
                    x = 0,
                    y = 0
                };
                return temp;
            }
        }

        public static CellPoint Make(int x, int y)
        {
            CellPoint temp = new CellPoint
            {
                x = x,
                y = y
            };
            return temp;
        }

        public static CellPoint Make(float x, float y)
        {
            CellPoint temp = new CellPoint
            {
                x = (int)x,
                y = (int)y
            };
            return temp;
        }

        /**
         * new points
         */
        public static CellPoint Make(Vector2 point)
        {
            CellPoint temp = new CellPoint
            {
                x = (int)point.x,
                y = (int)point.y
            };
            return temp;
        }

        /**
         * equal points
         */
        public static bool isEqual(CellPoint f, CellPoint s)
        {
            if (f.x == s.x && s.y == f.y)
                return true;

            return false;
        }

        public bool Equal(CellPoint f)
        {
            if (f.x == this.x && this.y == f.y)
                return true;

            return false;
        }
    }


    /**
     *  LDMesh
     */
    // структура точки
    [System.Serializable]
    public struct LDMesh
    {
        // size or position
        public int x;
        public int y;
        public float width;
        public float height;

        // costs
        public int g_cost;
        public int h_cost;
        public int f_cost;

        // type of node
        public int type;

        // special int type
        public int s_type;

        // pass or not
        public bool past;

        // parent
        public int p_x;
        public int p_y;


        // mesh numbder in massive

        public int n_x;
        public int n_y;
    };




    /*
     * Global Library Functions
     */ 
    public class ld
    {

        public static float GetDistance(Vector2 f_point, Vector2 s_point)
        {
            float x = s_point.x - f_point.x;
            float y = s_point.y - f_point.y;
            return Mathf.Sqrt(x * x + y * y);
        }

        public static float GetDistance(CellPoint f_point, CellPoint s_point)
        {
            float x = s_point.x - f_point.x;
            float y = s_point.y - f_point.y;
            return Mathf.Sqrt(x * x + y * y);
        }

        ////////////////////////////////////////////////////////
        // Lodkod Mesh for meshMap realization

        /**
        * nill points
        */
        public static CellPoint CellPointMake()
        {
            CellPoint temp = new CellPoint
            {
                x = 0,
                y = 0
            };
            return temp;
        }


        /// <summary>
        /// Chance int int from 0 to 100
        /// </summary>
        public static bool CheckChance(int number)
        {
            return UnityEngine.Random.Range(0, 100) <= number;
        }

        /**
        * new points
        */
        public static CellPoint CellPointMake(int x, int y)
        {
            CellPoint temp = new CellPoint
            {
                x = x,
                y = y
            };
            return temp;
        }

        /**
         * new points
         */
        public static CellPoint CellPointMake(Vector2 point)
        {
            CellPoint temp = new CellPoint
            {
                x = (int)point.x,
                y = (int)point.y
            };
            return temp;
        }

        /**
         * equal points
         */
        public static bool isCellEqual(CellPoint f, CellPoint s)
        {
            if (f.x == s.x && s.y == f.y)
                return true;

            return false;
        }

        ////////////////////////////////////////////////////////
        // Lodkod Mesh for meshMap realization

        public static float pxsInUnit = 65.0f;

        /**
        * nill points
        */
        public static LDMeshPoint MeshPointMake()
        {
            LDMeshPoint temp = new LDMeshPoint();
            temp.x = 0;
            temp.y = 0;
            return temp;
        }

        /**
        * new points
        */
        public static LDMeshPoint MeshPointMake(int x, int y)
        {
            LDMeshPoint temp;
            temp.x = x;
            temp.y = y;
            return temp;
        }

        /**
         * new points
         */
        public static LDMeshPoint MeshPointMake(Vector2 point)
        {
            LDMeshPoint temp;
            temp.x = (int)point.x;
            temp.y = (int)point.y;
            return temp;
        }

        /**
         * equal points
         */
        public static bool isMeshEqual(LDMeshPoint f, LDMeshPoint s)
        {
            if (f.x == s.x && s.y == f.y)
                return true;

            return false;
        }

        /**
         * create Mesh
         */
        public static LDMesh Mesh()
        {
            LDMesh temp;
    
            // size or position
            temp.x = 0;
            temp.y = 0;
            temp.width = 0;
            temp.height = 0;
    
            // type of node
            temp.type = -1;
    
            temp.s_type = -1;
    
            // costs
            temp.f_cost = 0;
            temp.h_cost = 0;
            temp.g_cost = 0;
    
            // pass or not
            temp.past = false;
    
            // parent
            temp.p_x = -1;
            temp.p_y = -1;
    
    
            // numbers
            temp.n_x = 0;
            temp.n_y = 0;
    
            return temp;
       }






        


        /**
         * unit to pxs
         */
        public static float pxsToUnis(float pxsPerUnit, float transletTo)
        {
            return (float)(transletTo/pxsPerUnit);
        }

        /**
         * unit to pxs
         */
        public static float pxsToUnis(float transletTo)
        {
            return (float)(transletTo / pxsInUnit);
        }


        /**
         * Рандомная точка на карте по х
         */
        public static Vector2 ranUnitPoint(LDMeshPoint point)
        {
            return new Vector2(UnityEngine.Random.Range(point.x, point.x+0.75f), point.y+0.43f);
        }

        public static System.Object GetJSONValue(FieldInfo type, JSONNode node)
        {
            string nameType = type.FieldType.Name;
            if (nameType.Equals("String"))
                return node.Value;
            else if(nameType.Equals("Int32"))
                return node.AsInt;
            else if (nameType.Equals("Boolean"))
                return node.AsBool;
            else if (nameType.Equals("Double"))
                return node.AsFloat;
            else 
                return null;
        }

        public static float DegreeBetweenPoints(CellPoint first, CellPoint second)
        {
            return RadiansToDegree(Mathf.Atan2(second.y - first.y, second.x - first.x));
        }

        public static float DegreeBetweenPoints(Vector2 first, Vector2 second)
        {
            return RadiansToDegree(Mathf.Atan2(second.y - first.y, second.x - first.x));
        }

        public static float RadiansToDegree(float rad)
        {
            return (Mathf.PI / 180) * rad;
        }

        public static float DegreeToRadians(float rad)
        {
            return (180 / Mathf.PI) * rad;
        }

    }



}
