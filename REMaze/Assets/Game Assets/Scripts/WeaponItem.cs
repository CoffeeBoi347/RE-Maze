using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon", order = 1)]
public class WeaponItem : ScriptableObject
{
    public GameObject weaponOBJ;
    public string weaponName;
    public float weaponDamage;
}
