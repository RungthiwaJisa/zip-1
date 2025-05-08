using UnityEngine;

public class ShootControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Player player;

    [Header("Settings")]
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float bulletLifetime = 3f;

    void Start()
    {
        if (player == null)
        {
            var go = GameObject.Find("Player");
            if (go != null)
            {
                player = go.GetComponent<Player>();
            }
        }

        UIManager.OnUIShootButton += Shoot;
    }

    void Shoot()
    {
        if (bulletPrefab == null || cameraTransform == null)
        {
            Debug.LogError("Missing bullet prefab or camera reference!");
            return;
        }

        var bullet = Instantiate(bulletPrefab, cameraTransform.position, Quaternion.identity);
        var rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(cameraTransform.forward * bulletSpeed, ForceMode.Impulse);
        }

        Bullets bullets = bullet.GetComponent<Bullets>();
        if (bullets != null && player != null && player.currentWeapon != null)
        {
            bullets.damage = player.currentWeapon.damage;
        }

        var parti = Instantiate(bullets.particals,bullet.transform.position, Quaternion.identity);
        Destroy(bullet, bulletLifetime);
        Destroy(parti, bulletLifetime - 2);
    }
}