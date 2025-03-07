﻿using UnityEngine;
using UnityEngine.UI;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using EasyButtons;
#endif

namespace ThunderRoad
{
    public class UITextScores : UIText
    {
        public Value value;

        public enum Value
        {
            None,
            Wave,
            Kills,
            Experience,
            Honor,
            Hits,
            Score,
            DetailCreatureName,
            DetailActionName,
            DetailModifierName,
            DetailCreatureBaseXP,
            DetailCreatureAiXP,
            DetailCreatureEquipementXP,
            DetailCreatureTotalXP,
            DetailActionModifierXP,
            DetailActionModifierTotalXP,
            DetailTotalXP,
            DetailTotalHonor,
            DetailActionModifierName,
        }

    }
}
