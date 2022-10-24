using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleEngine
{
    public enum ActionSourceType { Card, Hero, Familiar, Enemy, Effect }

    public class ActionSourceData 
    {
        private string _actionID = "Unknown";
        public string ActionID => _actionID;

        private string _additionalActionID = "Unknown";
        public string AdditionalActionID => _additionalActionID;

        private ActionSourceType _sourceType = ActionSourceType.Card;
        public ActionSourceType SourceType => _sourceType;

        private string _sourceID = "Unknown";
        public string SourceID => _sourceID;

        private string _additionalSourceID = "Unknown";
        public string AdditionalSourceID => _additionalSourceID;

        private int _actionAmount = 0;
        public int ActionAmount => _actionAmount;

        public static ActionSourceData Create()
        {
            return new ActionSourceData();
        }

        public ActionSourceData SetActionID(string actionID)
        {
            this._actionID = actionID;
            return this;
        }

        public ActionSourceData SetAdditionalActionID(string additionalActionID)
        {
            this._additionalActionID = additionalActionID;
            return this;
        }

        public ActionSourceData SetAdditionalSourceID(string additioanlSourceID)
        {
            this._additionalSourceID = additioanlSourceID;
            return this;
        }

        public ActionSourceData SetSourceID(string sourceID)
        {
            this._sourceID = sourceID;
            return this;
        }

        public ActionSourceData SetSourceType(ActionSourceType sourceType)
        {
            this._sourceType = sourceType;
            return this;
        }

        public ActionSourceData SetActionAmount(int actionAmount)
        {
            this._actionAmount = actionAmount;
            return this;
        }
    }
}

