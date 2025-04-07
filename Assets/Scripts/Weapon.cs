using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum Category {primary, secondary, melee}

    [Header("CATEGORY")]
    public Category category;

    [Header("AUDIO")]
    [SerializeField] AudioClip gunShotAudio; // Reference to the audio clip
    [SerializeField] AudioSource audioSource; // Reference to the audio source

    [Header("DATA")]
    [SerializeField] public WeaponData weaponData; // Reference to the weapon data

    [HideInInspector] protected int currentAmmo; // Tracks the current ammo of the weapon
    protected float m_lastFireTime; // Tracks when the weapon can be fire again

    public void InitializeWeapon()
    {
        currentAmmo = weaponData.maxAmmo;
    }

    // Virtual for firing projectiles
    public virtual void Fire()
    {
        UpdateNextFire();
        PlayGunShot();
        ReloadWeapon();
    }

    private void UpdateNextFire()
    {
        currentAmmo--;
        m_lastFireTime = Time.time;
    }

    private void ReloadWeapon()
    {
        if (currentAmmo == 0)
        {
            StartCoroutine(StartReload());
        }
    }

    public bool CanFire()
    {
        // Check if the weapon can be fire
        return currentAmmo > 0 && Time.time >= m_lastFireTime + weaponData.firerate;
    }

    public bool IsFiring()
    {
        // Returns true if the weapon is still in cooldown
        return Time.time < m_lastFireTime + weaponData.firerate;
    }


    protected IEnumerator StartReload()
    {
        // Track the time elapsed during the reload process
        float timeElapsed = 0f;

        while (timeElapsed <= weaponData.reloadSpeed)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Reset the current ammo
        currentAmmo = weaponData.maxAmmo;
    }

    public void PlayGunShot()
    {
        if (gunShotAudio != null && audioSource != null)
        {
            AudioManager.instance.PlayAudioOnce(audioSource, gunShotAudio);
        }
    }
}