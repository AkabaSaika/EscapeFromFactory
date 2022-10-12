using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Weapon
}

public class Item : ScriptableObject
{
    [SerializeField]
    private int id;
    [SerializeField]
    private int amount;
    [SerializeField]
    private ItemType itemType;

}
