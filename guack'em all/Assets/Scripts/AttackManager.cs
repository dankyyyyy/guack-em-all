using System.Collections;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private Animator weaponAnimator;
    private TrailRenderer currentTrailRenderer;
    private ParticleSystem currentSwipeParticles;
    public bool IsPlayerSwinging { get; private set; } = false;
    [SerializeField] private Transform weaponTransform; // Assign the arm/weapon transform in the Inspector
    [SerializeField] private WeaponData currentWeaponData; // Assign the current weapon's data in the Inspector
    [HideInInspector] public int damagedHealth;
    private AudioSource audioSource;

    void Start()
    {
        if (weaponTransform != null)
        {
            weaponAnimator = weaponTransform.GetComponent<Animator>();
            if (weaponAnimator == null)
            {
                Debug.LogError("Animator not found on the weapon child object!");
            }
            // Apply Animator Override if provided in the Weapon Data
            if (currentWeaponData != null && currentWeaponData.weaponAnimatorOverride != null && weaponAnimator != null)
            {
                weaponAnimator.runtimeAnimatorController = currentWeaponData.weaponAnimatorOverride;
            }
        }
        else
        {
            Debug.LogError("Weapon Transform not assigned in the Inspector!");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = 0.3f;

        // Instantiate visual effects if prefabs are provided
        if (currentWeaponData != null)
        {
            if (currentWeaponData.trailPrefab != null && weaponTransform != null)
            {
                GameObject trailObject = Instantiate(currentWeaponData.trailPrefab, weaponTransform);
                currentTrailRenderer = trailObject.GetComponent<TrailRenderer>();
                if (currentTrailRenderer != null)
                {
                    currentTrailRenderer.emitting = false;
                }
            }

            if (currentWeaponData.swipeParticlesPrefab != null && weaponTransform != null)
            {
                GameObject particlesObject = Instantiate(currentWeaponData.swipeParticlesPrefab, weaponTransform);
                currentSwipeParticles = particlesObject.GetComponent<ParticleSystem>();
                if (currentSwipeParticles != null)
                {
                    currentSwipeParticles.Stop();
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentWeaponData != null)
        {
            StartSwing();

            // Trigger the animation on the weapon's animator
            if (weaponAnimator != null)
            {
                weaponAnimator.SetTrigger("Attack");
            }

            // Enable trail
            if (currentTrailRenderer != null)
            {
                currentTrailRenderer.emitting = true;
            }

            // Play particles
            if (currentSwipeParticles != null)
            {
                currentSwipeParticles.Play();
            }

            PlayAttackSound();

            // Disable trail shortly after
            Invoke("StopTrail", currentWeaponData.swingLength); // Use the swingLength from the current weapon data
        }
    }

    void StopTrail()
    {
        if (currentTrailRenderer != null)
        {
            currentTrailRenderer.emitting = false;
        }
    }

    public void StartSwing()
    {
        IsPlayerSwinging = true;
        StartCoroutine(EndSwingAfterDelay(currentWeaponData.swingLength)); // Use swingLength from weapon data
    }
    IEnumerator EndSwingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndSwing();
    }
    public void EndSwing()
    {
        IsPlayerSwinging = false;
    }

    public void DealDamage(int targetHealth)
    {
        if (currentWeaponData != null)
        {
            damagedHealth = targetHealth - currentWeaponData.damage; // Use damage from weapon data
        }
        else
        {
            Debug.LogError("No Weapon Data assigned to the Attack Manager!");
            damagedHealth = targetHealth; // Avoid potential errors
        }
    }

    void PlayAttackSound()
    {
        if (currentWeaponData != null && currentWeaponData.attackSounds.Length > 0)
        {
            AudioClip clip = currentWeaponData.attackSounds[Random.Range(0, currentWeaponData.attackSounds.Length)];
            audioSource.PlayOneShot(clip);
        }
    }
}