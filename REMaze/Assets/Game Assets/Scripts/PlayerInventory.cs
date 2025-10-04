using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<WeaponItem> weapons = new List<WeaponItem>();
    [SerializeField] private Transform weaponsHolder;
    public ActiveObject activeObject;

    private void Start()
    {
        foreach(var weapon in weapons)
        {
            GameObject _weapon = Instantiate(weapon.weaponOBJ, weaponsHolder);
            _weapon.SetActive(false);
        }
    }

    private void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        for(int i = 0; i <= weapons.Count - 1; i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                UseItem(i);
            }
        }
    }
    
    void UseItem(int weapon)
    {
        CheckActiveObject(weapon);
        Debug.Log("Using " + weapons[weapon].weaponName);
        for (int i = 0; i < weaponsHolder.childCount; i++)
        {
            if(i == weapon)
            {
                weaponsHolder.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                weaponsHolder.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void CheckActiveObject(int index)
    {
        if(index > 0 && index < weapons.Count + 1)
        {
            if (weapons[index].weaponName == "Sword")
            {
                activeObject = ActiveObject.Sword;
                weaponsHolder.GetChild(index).GetComponent<SwordFunctionality>().InjectSwordAnimator();
            }
            else if(weapons[index].weaponName == "Gun")
            {
                activeObject = ActiveObject.Gun;
                weaponsHolder.GetChild(index).GetComponent<GunFunctionality>().InjectGunAnim();
            }
            else
            {
                activeObject = ActiveObject.None;
            }
        }
    }
}

public enum ActiveObject
{
    None,
    Sword,
    Gun
}