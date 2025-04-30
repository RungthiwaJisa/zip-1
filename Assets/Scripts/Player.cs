using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int hp = 100;
    public int gold = 0;
    public List<Weapons> ownedWeapons = new List<Weapons>();
    public Weapons currentWeapon;
    public Image imageWeapon;

    private void Start()
    {
        if (ownedWeapons.Count > 0)
        {
            currentWeapon = ownedWeapons[0];
            UpdateUiWeapon();
        }
        else
        {
            Debug.LogWarning("No weapons assigned to player!");
        }
    }

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

    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log($"Added {amount} gold. Total: {gold}");
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
            UpdateUiWeapon();
        }
    }

    public void UpdateUiWeapon()
    {
        if (imageWeapon == null) return;

        if (currentWeapon != null)
        {
            imageWeapon.sprite = currentWeapon.weaponIcon;
            imageWeapon.enabled = true;
        }
        else
        {
            imageWeapon.enabled = false;
        }
    }
}
