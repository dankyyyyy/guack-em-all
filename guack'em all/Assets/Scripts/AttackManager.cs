using System.Collections;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public bool IsPlayerSwinging { get; private set; } = false;
    [SerializeField] public WeaponSO currentWeapon;
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private AudioSource weaponAudioSource;
    [SerializeField] private AudioClip[] attackSounds;
    //[SerializeField] private TrailRenderer weaponTrail;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    public int damagedHealth;
    private string Cactus;
    private string Maracas;
    private string Chicken;

    public void EquipWeapon(WeaponSO newWeapon)
    {
        currentWeapon = newWeapon;
        UpdateWeaponVisuals();
        attackSounds = currentWeapon.attackSounds; // Update attack sounds when weapon changes
    }

    void Start()
    {
        weaponTransform = GetComponentInChildren<Transform>();
        weaponAnimator = GetComponentInChildren<Animator>();
        weaponAudioSource = GetComponentInChildren<AudioSource>();
        attackSounds = currentWeapon.attackSounds;
        //weaponTrail = currentWeapon.weaponTrail;
        weaponSpriteRenderer.sprite = currentWeapon.weaponSprite;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentWeapon != null && IsPlayerSwinging == false)
        {
            StartSwing();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WeaponSO loadedWeapon = Resources.Load<WeaponSO>("Cactus");
            if (loadedWeapon != null)
            {
                EquipWeapon(loadedWeapon);
                Debug.Log("Equipped Weapon 1: " + loadedWeapon.name);
            }
            else
            {
                Debug.LogError("Could not load weapon: " + Cactus + ". Make sure it's in a 'Resources' folder.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            WeaponSO loadedWeapon = Resources.Load<WeaponSO>("Maracas");
            if (loadedWeapon != null)
            {
                EquipWeapon(loadedWeapon);
                Debug.Log("Equipped Weapon 2: " + loadedWeapon.name);
            }
            else
            {
                Debug.LogError("Could not load weapon: " + Maracas + ". Make sure it's in a 'Resources' folder.");
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            WeaponSO loadedWeapon = Resources.Load<WeaponSO>("Chicken");
            if (loadedWeapon != null)
            {
                EquipWeapon(loadedWeapon);
                Debug.Log("Equipped Weapon 3: " + loadedWeapon.name);
            }
            else
            {
                Debug.LogError("Could not load weapon: " + Chicken + ". Make sure it's in a 'Resources' folder.");
            }
        }
    }

    public void StartSwing()
    {
        IsPlayerSwinging = true;

        // // Enable trail emission
        // if (currentTrailRenderer != null)
        // {
        //     currentTrailRenderer.Clear(); // optional: avoids trail residue
        //     currentTrailRenderer.emitting = true;
        // }

        // // Play particles
        // if (currentSwipeParticles != null)
        // {
        //     currentSwipeParticles.Play();
        // }

        PlayAttackSound();

        // Trigger the animation on the weapon's animator
        if (weaponAnimator != null)
        {
            weaponAnimator.SetTrigger("Attack");
        }

        // Disable trail shortly after
        Invoke("StopTrail", currentWeapon.swingLength); // Use the swingLength from the current weapon data

        StartCoroutine(EndSwingAfterDelay(currentWeapon.swingLength));
    }

    public void EndSwing()
    {
        IsPlayerSwinging = false;

        // Disable trail emission
        // if (currentTrailRenderer != null)
        // {
        //     currentTrailRenderer.emitting = false;
        // }

        // // Stop particles
        // if (currentSwipeParticles != null)
        // {
        //     currentSwipeParticles.Stop();
        // }
    }

    IEnumerator EndSwingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndSwing();
    }

    // private void ApplyWeaponVisuals()
    // {
    //     if (currentWeapon == null) return;

    //     if (weaponSpriteRenderer != null && currentWeapon.weaponSprite != null)
    //     {
    //         weaponSpriteRenderer.sprite = currentWeapon.weaponSprite;
    //     }
    // }

    public void DealDamage(int targetHealth)
    {
        if (currentWeapon != null)
        {
            damagedHealth = targetHealth - currentWeapon.damage; // Use damage from weapon data
        }
        else
        {
            Debug.LogError("No Weapon Data assigned to the Attack Manager!");
            damagedHealth = targetHealth; // Avoid potential errors
        }
    }

    void PlayAttackSound()
    {
        if (attackSounds.Length > 0)
        {
            AudioClip clip = attackSounds[Random.Range(0, attackSounds.Length)];
            weaponAudioSource.PlayOneShot(clip);
        }
    }

    private void UpdateWeaponVisuals()
    {
        if (currentWeapon == null) return;

        if (weaponSpriteRenderer != null && currentWeapon.weaponSprite != null)
        {
            weaponSpriteRenderer.sprite = currentWeapon.weaponSprite;
        }

        if (weaponAudioSource != null && currentWeapon.attackSounds != null)
        {
            attackSounds = currentWeapon.attackSounds;
        }
        //weaponTrail = currentWeapon.weaponTrail;
    }
}