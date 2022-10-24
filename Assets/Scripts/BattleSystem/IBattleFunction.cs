using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattleEngine
{
    public interface IBattleFunction
    {
        public void StartFunction(Action trigger);

        public void Complete();
    }
}