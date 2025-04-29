using UnityEngine;

public class ShootControl : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Player player;

    [Header("Settings")]
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletLifetime = 3f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var go = GameObject.Find("Player");
        player = go.GetComponent<Player>();

        UIManager.OnUIShootButton += Shoot;
    }

    // Update is called once per frame
    void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, cameraTransform.position, Quaternion.identity);
        var rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(cameraTransform.forward * bulletSpeed, ForceMode.Impulse);

        Bullets bullets = bullet.GetComponent<Bullets>();
        bullets.damage = player.currentWeapon.damage;

        Destroy(bullet, bulletLifetime);
    }
}
