using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Player player;
    public int hp = 50;
    public int goldReward = 10; // เพิ่มรางวัลทอง
    public event Action OnEnemyDestroyed;

    private void Start()
    {
        var go = GameObject.Find("Player");
        player = go.GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Bullets bullet = other.GetComponent<Bullets>();
            int damages = bullet != null ? bullet.damage : 10;

            Destroy(other.gameObject);
            TakeDamage(damages);
        }
    }

    public void AttackPlayer()
    {
        if (player != null)
        {
            player.TakeDamage(10);
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
        if (player != null)
        {
            player.AddGold(goldReward);
        }

        OnEnemyDestroyed?.Invoke();
        Debug.Log("Enemy Destroyed");
        Destroy(gameObject);
    }
}
