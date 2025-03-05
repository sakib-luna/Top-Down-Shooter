using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float bulletForce = 20f;
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private float reloadTime = 1.5f;
    
    private int currentAmmo;
    private bool isReloading = false;
    private float nextFireTime = 0f;
    
    public delegate void AmmoChanged(int current, int max);
    public static event AmmoChanged OnAmmoChanged;
    
    public delegate void WeaponReloading(float reloadTime);
    public static event WeaponReloading OnReloading;
    
    private void Start()
    {
        currentAmmo = maxAmmo;
        
        if (OnAmmoChanged != null)
            OnAmmoChanged(currentAmmo, maxAmmo);
    }
    
    private void Update()
    {
        if (isReloading)
            return;
            
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }
        
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
        
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
        }
    }
    
    private void Shoot()
    {
        currentAmmo--;
        
        if (OnAmmoChanged != null)
            OnAmmoChanged(currentAmmo, maxAmmo);
            
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }
    
    private IEnumerator Reload()
    {
        isReloading = true;
        
        if (OnReloading != null)
            OnReloading(reloadTime);
            
        yield return new WaitForSeconds(reloadTime);
        
        currentAmmo = maxAmmo;
        
        if (OnAmmoChanged != null)
            OnAmmoChanged(currentAmmo, maxAmmo);
            
        isReloading = false;
    }
} 