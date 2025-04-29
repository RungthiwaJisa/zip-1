using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int hp = 100;
    public int gold = 0;
    public List<Weapons> ownedWeapons = new List<Weapons>();
    public Weapons currentWeapon;

    public void TakeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Game Over");
    }

    public void BuyWeapon(Weapons weapon)
    {
        if (!ownedWeapons.Contains(weapon))
        {
            ownedWeapons.Add(weapon);
            Debug.Log("Bought: " + weapon.weaponName);
        }
    }

    public void UpgradeWeapon(Weapons weapon)
    {
        if (ownedWeapons.Contains(weapon))
        {
            weapon.Upgrade();
        }
    }

    public void SelectWeapon(Weapons weapon)
    {
        if (ownedWeapons.Contains(weapon))
        {
            currentWeapon = weapon;
            Debug.Log("Selected: " + weapon.weaponName);
        }
    }
}
