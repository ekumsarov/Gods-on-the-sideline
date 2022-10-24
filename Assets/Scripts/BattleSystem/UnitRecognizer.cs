using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using System.Linq;

namespace BattleEngine
{
    public class UnitRecognizer 
    {
        public static RecognizerData RecogniseUnit(string id)
        {
            RecognizerData temp = new RecognizerData();
            temp.UnitID = id;

            if (BattleSystem.Enemies.Any(unit => unit.Name.Equals(id)))
            {
                temp.UnitType = CardTargetType.EnemyType;
                temp.UnitSide = CardTargetSide.Enemy;
                return temp;
            }

            if(BattleSystem.Heroes.Any(unit => unit.Name.Equals(id)))
            {
                temp.UnitType = CardTargetType.Hero;
                temp.UnitSide = CardTargetSide.Player;
                return temp;
            }

            if(BattleSystem.HeroesFamiliars.Any(unit => unit.Name.Equals(id)))
            {
                temp.UnitType = CardTargetType.Summon;
                temp.UnitSide = CardTargetSide.Player;
                return temp;
            }

            if (BattleSystem.EnemiesFamiliars.Any(unit => unit.Name.Equals(id)))
            {
                temp.UnitType = CardTargetType.Summon;
                temp.UnitSide = CardTargetSide.Enemy;
                return temp;
            }

            return temp;
        }

        public static Unit RecogniseAndGetUnit(string id)
        {
            if (BattleSystem.Enemies.Any(unit => unit.Name.Equals(id)))
            {
                return BattleSystem.Enemies.FirstOrDefault(unit => unit.Name.Equals(id));
            }

            if (BattleSystem.Heroes.Any(unit => unit.Name.Equals(id)))
            {
                return BattleSystem.Heroes.FirstOrDefault(unit => unit.Name.Equals(id));
            }

            if (BattleSystem.HeroesFamiliars.Any(unit => unit.Name.Equals(id)))
            {
                return BattleSystem.HeroesFamiliars.FirstOrDefault(unit => unit.Name.Equals(id));
            }

            if (BattleSystem.EnemiesFamiliars.Any(unit => unit.Name.Equals(id)))
            {
                return BattleSystem.EnemiesFamiliars.FirstOrDefault(unit => unit.Name.Equals(id));
            }

            return null;
        }

        public static string GetHeroIDByCard(string cardID)
        {
            return IOM.GetHeroIDByCard(cardID);
        }
    }

    public class RecognizerData
    {
        public string UnitID;
        public CardTargetType UnitType;
        public CardTargetSide UnitSide;
    }
}

