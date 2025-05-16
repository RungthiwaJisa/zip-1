using UnityEngine;

public class ShootControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Player player;
    [SerializeField] private AudioSource gunSound;

    [Header("Settings")]
    [SerializeField] private float bulletSpeed = 40f;
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

        gunSound.volume = 0.05f;
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

        if (gunSound != null && gunSound.clip != null)
        {
            gunSound.PlayOneShot(gunSound.clip);
        }

        if (rb != null)
        {
            rb.AddForce(cameraTransform.forward * bulletSpeed, ForceMode.Impulse);
        }

        Bullets bullets = bullet.GetComponent<Bullets>();
        if (bullets != null && player != null && player.currentWeapon != null)
        {
            bullets.damage = player.currentWeapon.damage;
        }

        Destroy(bullet, bulletLifetime);
    }
}