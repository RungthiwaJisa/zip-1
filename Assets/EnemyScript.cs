using System;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public event Action OnEnemyDestroyed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            OnEnemyDestroyed?.Invoke();
            print("Destroyed");
            Destroy(gameObject);
        }
    }
}
