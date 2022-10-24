using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CardTargetSelection
{
    public interface iTargetSelection 
    {
        public List<iBattleUnit> CanChooseTarget(List<iBattleUnit> targets);
        public List<iBattleUnit> SetTarget(List<iBattleUnit> targets);
        public iBattleUnit SelectedTarget(iBattleUnit unit);
        public bool NeedSelection();
    }
}

