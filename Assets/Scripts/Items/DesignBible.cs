using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "DesignBible", menuName = "Design Bibble", order = 0)]
    public class DesignBible : ScriptableObject
    {
        public int scoreToBeat = 1000;
        public int baseHitScore = 10;

        [Header("Strings")] 
        public string eatNoCombo = "I knew that would be gross";
        public string eatCombo = "YUMMM";
        public string dragNoCombo = "YUCK! That's Not Gonna Work..";
    }
}