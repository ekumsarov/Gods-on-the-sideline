using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lodkod;
using System;
using System.Linq;
using GameEvents;
using SimpleJSON;

namespace BattleEngine
{
    public delegate void PlayCardAction(Card card);

    public enum RoundStructure
    {
        StartRound,
        StartTurn,
        EndTurn,
        EndRound,
        StartAction,
        EndAction
    }

    public enum GroupSetup
    {
        Simple,
        Special
    }

    public class BattleSystem
    {
        private static BattleSystem _instance;
        private PlayerLogic _playerLogic;
        public static void NewGame()
        {
            if (BattleSystem._instance != null)
                BattleSystem._instance = null;

            BattleSystem._instance = new BattleSystem();
            BattleSystem._instance._heroes = new List<Hero>();
            BattleSystem._instance._units = new List<AIUnit>();
            BattleSystem._instance._heroesFamiliars = new List<Familiar>();
            BattleSystem._instance._enemiesFamiliars = new List<Familiar>();
            BattleSystem._instance._selectedTargets = new List<iBattleUnit>();
            BattleSystem._instance._players = new List<TurnLogic>();
            BattleSystem._instance._checkinNames = new Dictionary<string, int>();
            BattleSystem._instance._battleCoroutine = GameObject.Find("BattleCoroutine").GetComponent<MonoBehaviour>();

            BattleEndCondition.initAllPacks();
            BattleResponse.NewGame();
        }

        #region parameters

        private int _currentRound = 0;
        public static int CurrentRound => BattleSystem._instance._currentRound;

        private int _currentTurn = -1;

        private List<TurnLogic> _players;

        private MonoBehaviour _battleCoroutine;

        private bool _playerTurn = false;
        public static bool PlayerTurn => BattleSystem._instance._playerTurn;

        private List<Hero> _heroes;
        public static List<Hero> Heroes => BattleSystem._instance._heroes;

        private List<AIUnit> _units;
        public static List<AIUnit> Enemies => BattleSystem._instance._units;

        private List<Familiar> _heroesFamiliars;
        public static List<Familiar> HeroesFamiliars => BattleSystem._instance._heroesFamiliars;

        private List<Familiar> _enemiesFamiliars;
        public static List<Familiar> EnemiesFamiliars => BattleSystem._instance._enemiesFamiliars;

        private BattleEvent _event;
        private BattleMenu _menu;
        public static void SetupMenu(BattleMenu menu)
        {
            BattleSystem._instance._menu = menu;
        }

        public static BattleMenu Menu => BattleSystem._instance._menu;

        #endregion

        public static void SetupBattle(BattleDescriptor descriptor, BattleEvent ev)
        {
            BattleSystem._instance._currentRound = 0;
            BattleSystem._instance._playerTurn = false;
            BattleSystem._instance._event = ev;

            BattleSystem._instance._selectedTargets.Clear();

            BattleSystem._instance._heroes.Clear();
            BattleSystem._instance._units.Clear();
            BattleSystem._instance._heroesFamiliars.Clear();
            BattleSystem._instance._enemiesFamiliars.Clear();

            BattleSystem._instance._currentTurn = -1;
            BattleSystem._instance._players.Add(TurnLogic.Create(true, BattleSystem._instance._battleCoroutine));
            BattleSystem._instance._players.Add(TurnLogic.Create(false, BattleSystem._instance._battleCoroutine));

            BattleSystem._instance._playerLogic = BattleSystem._instance._players[0] as PlayerLogic;

            if (descriptor.PlayerSetup == GroupSetup.Simple)
            {
                BattleSystem._instance._heroes.AddRange(GM.PlayerIcon.Group.GetUnits().Cast<Hero>());
            }
            else
            {
                for(int i = 0; i < descriptor.PlayerSetups.Count; i++)
                {
                    Hero temp = Hero.Make(IOM.GetHeroData(descriptor.PlayerSetups[i]));
                    try
                    {
                        BattleSystem._instance._heroes.Add(temp);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
            }

            foreach(var hero in BattleSystem._instance._heroes)
            {
                hero.PrepareForBattle();
            }

            if (descriptor.EnemySetup == GroupSetup.Simple)
            {
                BattleSystem._instance._units.AddRange(BattleSystem._instance._event.Object.Group.GetUnits().Cast<AIUnit>());
            }
            else
            {
                for (int i = 0; i < descriptor.EnemySetups.Count; i++)
                {
                    AIUnit temp = Unit.Make(IOM.GetUnitData(descriptor.EnemySetups[i]));
                    try
                    {
                        BattleSystem._instance._units.Add(temp);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.Message);
                    }
                }
            }

            foreach (var hero in BattleSystem._instance._heroes)
            {
                hero.PrepareForBattle();
            }

            foreach (var enemy in BattleSystem._instance._units)
            {
                enemy.PrepareForBattle();
            }
            // for test to git
            DeckSystem.PrepareForBattle();
            BattleSystem._instance._menu.Prepare();
        }

        public static void StartBattle()
        {
            BattleSystem._instance._currentRound = -1;
            UIM.OpenMenu("BattleMenu");
            UIM.Fade(false, callF: NextRound);
        }

        private static void NextTurn()
        {
            BattleSystem._instance._currentTurn += 1;
            if (BattleSystem._instance._currentTurn >= 2)
            {
                NextRound();
                return;
            }

            BattleSystem._instance._players[BattleSystem._instance._currentTurn].Start(NextTurn);
        }

        private static void NextRound()
        {
            if(BattleSystem.IsBattleEnd())
            {
                BattleLog.PushLog("Battle End");
                BattleSystem._instance._menu.Close();
                return;
            }

            BattleSystem._instance._currentRound += 1;

            BattleLog.PushLog("Next round: " + BattleSystem._instance._currentRound);

            /*for (int i = 0; i < BattleSystem.Enemies.Count; i++)
            {
                if (BattleSystem.Enemies[i].CurrentHP <= 0 || BattleSystem.Enemies[i].IsActive == false)
                    continue;

                BattleSystem.Enemies[i].NextRound();
            }*/

            BattleSystem._instance._currentTurn = -1;
            BattleSystem.SelectedTargets.Clear();
            BattleSystem._instance._playerTurn = true;
            BattleSystem.NextTurn();
        }

        // Bad hack
        // TODO : remove this hack
        public static void CompletePlayerTurn()
        {
            BattleSystem._instance._players[0].Complete();
        }

        public static void SetPlayerActionCount(int count)
        {
            PlayerLogic temp = BattleSystem._instance._players[0] as PlayerLogic;
            if (temp != null)
                temp.SetActionCount(count);
        }

        private static bool IsBattleEnd()
        {
            return BattleSystem.Heroes.All(unit => unit.CurrentHP <= 0 || unit.IsActive == false) || BattleSystem.Enemies.All(unit => unit.CurrentHP <= 0 || unit.IsActive == false);
        }

        public static void PointedUnit(iBattleUnit unit)
        {

        }

        public static void AddFunctionToQuery(IBattleFunction function)
        {
            BattleSystem._instance._players[BattleSystem._instance._currentTurn].PlayAction(function);
        }

        public static void OnUnitPoint(iBattleUnit unit)
        {
            if (BattleSystem._instance._playerTurn == false || BattleSystem._instance._needToSelectUnit == false)
                return;

            BattleSystem._instance._onUnitSelect?.Invoke(unit);
            //BattleSystem.ExecuteActions();
            //BattleSystem._instance._needToSelectUnit = false;
        }

        public delegate void OnUnitSelect(iBattleUnit unit);
        private OnUnitSelect _onUnitSelect;
        public static void SubscribeOnUnitPoint(OnUnitSelect subscriber)
        {
            BattleSystem._instance._needToSelectUnit = true;
            BattleSystem._instance._onUnitSelect += subscriber;
        }

        public static void UnsubscribeOnUnitPoint(OnUnitSelect subscriber)
        {
            BattleSystem._instance._needToSelectUnit = false;
            BattleSystem._instance._onUnitSelect -= subscriber;
        }

        private bool _needToSelectUnit = false;
        public static void StartChooseTarget()
        {
            BattleSystem._instance._needToSelectUnit = true;
        }

        public static List<iBattleUnit> GetAllAlive()
        {
            List<iBattleUnit> alives = BattleSystem._instance._heroes.Where(unit => unit.IsAlive).ToList<iBattleUnit>();
            alives.AddRange(BattleSystem._instance._units.Where(unit => unit.IsAlive).ToList<iBattleUnit>());
            alives.AddRange(BattleSystem._instance._enemiesFamiliars.Where(unit => unit.IsAlive));
            alives.AddRange(BattleSystem._instance._heroesFamiliars.Where(unit => unit.IsAlive));
            return alives;
        }

        public static List<Hero> GetAliveHeroes()
        {
            return BattleSystem._instance._heroes.Where(unit => unit.IsAlive).ToList();
        }

        public static List<AIUnit> GetAliveEnemies()
        {
            return BattleSystem._instance._units.Where(unit => unit.IsAlive).ToList();
        }

        private List<iBattleUnit> _selectedTargets;
        public static List<iBattleUnit> SelectedTargets => BattleSystem._instance._selectedTargets;
        public static void AddSelectedTargets(List<iBattleUnit> units)
        {
            for(int i = 0; i < units.Count; i++)
            {
                if (BattleSystem._instance._selectedTargets.Any(unit => unit == units[i]))
                    continue;

                BattleSystem._instance._selectedTargets.Add(units[i]);
            }
        }

        public static void AddSelectedTargets(iBattleUnit unit)
        {
            if (BattleSystem._instance._selectedTargets.Any(sunit => sunit == unit))
                return;

            BattleSystem._instance._selectedTargets.Add(unit);
        }

        public static void AddUnit(Unit unit, bool isPlayerSide)
        {
            // Familiar add

            Familiar temp = unit as Familiar;

            if(temp == null)
            {
                if (isPlayerSide)
                {
                    Hero hero = unit as Hero;
                    if(hero != null)
                    {
                        BattleSystem._instance._heroes.Add(hero);
                        BattleSystem._instance._menu.AcivateUnit(hero, isPlayerSide);
                    }
                        
                }
                else
                {
                    AIUnit aiunit = unit as AIUnit;
                    if(aiunit != null)
                    {
                        BattleSystem._instance._units.Add(aiunit);
                        BattleSystem._instance._menu.AcivateUnit(aiunit, isPlayerSide);
                    }
                }

                return;
            }

            if (isPlayerSide)
                BattleSystem._instance._heroesFamiliars.Add(temp as Familiar);
            else
                BattleSystem._instance._enemiesFamiliars.Add(temp as Familiar);

            BattleSystem._instance._menu.AcivateUnit(temp, isPlayerSide);
            BattleActions.FamiliarActivation.Create(temp);
        }

        public static bool CanAddUnit(bool PlayerSide)
        {
            return BattleSystem._instance._menu.CannActivateUnit(PlayerSide);
        }

        private Dictionary<string, int> _checkinNames;
        public static string CheckNames(string name)
        {
            if (BattleSystem._instance._checkinNames == null)
                BattleSystem._instance._checkinNames = new Dictionary<string, int>();

            string changedName = name;

            if (BattleSystem._instance._checkinNames.ContainsKey(name))
            {
                BattleSystem._instance._checkinNames[name] += 1;
                changedName = name + BattleSystem._instance._checkinNames[name];
            }
            else
                BattleSystem._instance._checkinNames.Add(name, 0);

            return changedName;
        }
    }
}

