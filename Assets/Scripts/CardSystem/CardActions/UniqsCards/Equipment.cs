using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lodkod;
using BattleActions;
using CardTargetSelection;

namespace BattleEngine
{
    public class Equipment : CardAction
    {
        public override string ActionType => "Equipment";

        public override void Init(CardActionTypes data, string heroID)
        {
            this.HeroID = heroID;
            this.TargetSetup(data);
        }
        public override bool CanStart()
        {
            return true;
        }

        protected override void Play()
        {
            int cardSelect = Random.Range(0, 121);
            string cardID = string.Empty;

            if (cardSelect <= 40)
                cardID = "ForgeHammer";
            else if (cardSelect > 40 && cardSelect <= 80)
                cardID = "ArmorOfSvarog";
            else
                cardID = "MurdereKoschey";



            Card temp = Card.GetCard(cardID);

            if (temp == null)
            {
                Debug.LogError("Equipment card not found: " + cardID);
                CompleteAction();
                return;
            }

            BattleSystem.Menu.PlayerInteract(false);
            BattleSystem.AddFunctionToQuery(CallbackAction.Get(BattleSystem.Menu.ClearOnPlayerTurn));
            BattleSystem.AddFunctionToQuery(CallbackAction.Get(temp.PlayCard));
            BattleSystem.AddFunctionToQuery(CallbackAction.Get(BattleSystem.Menu.ClearOnPlayerTurn));
            CompleteAction();
        }

        public override string GetDescription()
        {
            return LocalizationManager.Get("EquipmentCardDescription");
        }

        public override string GetDescription(CardActionTypes data)
        {
            return LocalizationManager.Get("EquipmentCardDescription") + " " + CardActionTarget.GetTargetDescription(data.dTarget, data.dTargetSide, data.dTargetIncludes);
        }
    }
}