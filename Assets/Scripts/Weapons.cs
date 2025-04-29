using UnityEngine;

public class Weapons : MonoBehaviour
{
    public string weaponName;
    public int damage = 10;
    public int level = 1;

    public void Upgrade()
    {
        level++;
        damage += 5;
        Debug.Log($"{weaponName} upgraded to Level {level}, Damage {damage}");
    }
}
