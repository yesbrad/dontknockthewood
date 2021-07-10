using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Base Item")]
public class ItemData : ScriptableObject
{
    public string name;
    public Sprite slotImageSprite;
}