using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private Player player;
    private UIManager uiManager;
    public int hp = 50;
    public int goldReward = 10; // เพิ่มรางวัลทอง
    public event Action OnEnemyDestroyed;
    [SerializeField] private AudioSource takeDamagesSound;

    private void Start()
    {
        var go = GameObject.Find("Player");
        player = go.GetComponent<Player>();
        var ge = GameObject.Find("GameManager");
        uiManager = ge.GetComponent<UIManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Bullets bullet = other.gameObject.GetComponent<Bullets>();
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

        if (takeDamagesSound != null && takeDamagesSound.clip != null)
        {
            takeDamagesSound.PlayOneShot(takeDamagesSound.clip);
        }

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
