using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tzdevil.GameRelated
{
    [System.Serializable]
    public class GameData
    {
        public float Score = 0;

        public float RemainingFood = 100f;
        public float Tastiness = 100f;
        public float IngredientRatio = 50f; // this should be close to 50 all times. If it's <50 or >50, then it's bad. (it gets really worse in <35 and >65 though)
        public float BurntRatio = 0f;
        public float CustomerSatisfaction = 100f; // in % (%100)
    }
}