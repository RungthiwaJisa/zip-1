using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Player player;
    private UIManager uiManager;
    public int hp = 50;
    public int goldReward = 10; // เพิ่มรางวัลทอง
    public event Action OnEnemyDestroyed;

    private void Start()
    {
        var go = GameObject.Find("Player");
        player = go.GetComponent<Player>();
        var ge = GameObject.Find("GameManager");
        uiManager = ge.GetComponent<UIManager>();
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
            uiManager.UpdateHP();
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
            uiManager.UpdateCoin();
        }

        OnEnemyDestroyed?.Invoke();
        Debug.Log("Enemy Destroyed");
        Destroy(gameObject);
    }
}
