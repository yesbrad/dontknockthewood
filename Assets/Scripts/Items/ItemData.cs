using Items;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Base Item")]
public class ItemData : ScriptableObject
{
    public string name;
    public Sprite slotImageSprite;
    public bool canGoInMouth;

    [Header("Spitball")] 
    public bool canGoInStraw;
    [Range(1, 20)]
    public int startAmmo;
    [Range(0.5f, 10)] public float scoreMultiplier;
    public Color BallColor;
}