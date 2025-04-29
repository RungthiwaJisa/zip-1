using UnityEngine;

public class Weapons : MonoBehaviour
{
    public string weaponName;
    public int damage = 10;
    public int level = 1;
    public int price = 100;
    public int capacity = 10;

    public void Upgrade()
    {
        level++;
        damage += 5;
        capacity += 5;
        Debug.Log($"{weaponName} upgraded to Level {level}, Damage {damage}");
    }
}
