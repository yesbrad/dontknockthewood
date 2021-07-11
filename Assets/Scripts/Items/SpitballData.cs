using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "SpitBall", menuName = "Spit Ball", order = 0)]
    public class SpitballData : ScriptableObject
    {
        public string name;
        
        [Range(1, 20)]
        public int startAmmo;

        [Range(0.5f, 10)] public float scoreMultiplier;
        
        public Color BallColor;
    }
}