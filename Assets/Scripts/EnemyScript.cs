using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Player player;
    public float lifeTime = 5f;
    public int hp = 50;

    public event Action OnEnemyDestroyed;

    public void Init(Player playerRef)
    {
        player = playerRef;
    }

    private void OnTriggerEnter(Collider other)
    {
        Bullets bullet = other.GetComponent<Bullets>();

        int damages = bullet.damage;

        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            TakeDamage(damages);
        }
    }

    public void AttackPlayer()
    {
        player.TakeDamage(10);
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
        OnEnemyDestroyed?.Invoke();
        print("Destroyed");
        Destroy(gameObject);
    }
}
