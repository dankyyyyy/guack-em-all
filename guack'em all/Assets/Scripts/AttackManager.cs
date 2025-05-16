using System.Collections;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public bool IsPlayerSwinging { get; private set; } = false;
    [SerializeField] private WeaponSO currentWeapon; // Assign the current weapon's data in the Inspector
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private AudioSource weaponAudioSource;
    [SerializeField] private AudioClip[] attackSounds;
    //[SerializeField] private TrailRenderer weaponTrail;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;


    public int damagedHealth;

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
}