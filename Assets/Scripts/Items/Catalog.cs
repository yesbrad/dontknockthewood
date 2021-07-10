using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "ItemCatalog", menuName = "Item Catalog", order = 0)]
    public class Catalog : ScriptableObject
    {
        public List<ItemCombo> Combos;
    }
}