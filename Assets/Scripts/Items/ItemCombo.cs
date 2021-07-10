using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "ItemCombo", menuName = "Item Combo", order = 0)]
    public class ItemCombo : ScriptableObject
    {
        public ItemData firstIngredient;
        public ItemData secondIngredient;
        
        [Space]
        
        public ItemData comboItem;
    }
}