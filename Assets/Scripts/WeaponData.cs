using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Game/Weapon")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public int baseDamage = 10;
    public int basePrice = 100;
    public int baseCapacity = 10;
    public Sprite weaponIcon;
    public GameObject weaponModel;
}
