using Items;
using UnityEngine;

[System.Serializable]
public class Spitball
{
    public SpitballData data;
    public int ammo;

    public Spitball(SpitballData data)
    {
        this.data = data;
        ammo = data.startAmmo;
    }

    public bool Use()
    {
        ammo--;

        if (ammo < 0)
        {
            return false;
        }

        return true;
    }
}