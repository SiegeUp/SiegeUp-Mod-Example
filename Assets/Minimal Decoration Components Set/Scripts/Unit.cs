using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool detectable = false;

    public int populationSlots = 0;

    public enum Tag
    {
        Melee = 0,
        Archer = 1,
        Cavalry = 2,
        Naval = 3,
        SiegeWeapon = 4,
        Animal = 5,
        Wall = 40,
        SeaBuilding = 41
    }

    public Tag[] tags;

    public bool hasTag(Tag tag)
    {
        return System.Array.IndexOf(tags, tag) != -1;
    }
}
