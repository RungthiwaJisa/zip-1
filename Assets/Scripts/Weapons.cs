using UnityEngine;

public class Weapons : MonoBehaviour
{
    public WeaponData weaponData;
    public string weaponName;
    public int damage = 10;
    public int level = 1;
    public int price = 100;
    public int capacity = 10;
    public Sprite weaponIcon;

    private void Awake()
    {
        if (weaponData != null)
        {
            weaponName = weaponData.weaponName;
            damage = weaponData.baseDamage;
            price = weaponData.basePrice;
            capacity = weaponData.baseCapacity;
            weaponIcon = weaponData.weaponIcon;
        }
    }

    public void Upgrade()
    {
        level++;
        damage += 5;
        capacity += 5;
        Debug.Log($"{weaponName} upgraded to Level {level}, Damage {damage}");
    }

    public int GetUpgradePrice()
    {
        return price / 2 * level;
    }
}
