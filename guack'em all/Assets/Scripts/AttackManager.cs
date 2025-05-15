using System.Collections;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private Animator weaponAnimator;
    private TrailRenderer currentTrailRenderer;
    private ParticleSystem currentSwipeParticles;
    public bool IsPlayerSwinging { get; private set; } = false;
    [SerializeField] private Transform weaponTransform; // The attachment point, not necessarily the weapon itself anymore
    [SerializeField] private WeaponData currentWeaponData; // Assign the current weapon's data in the Inspector (can be managed by WeaponEquipper)
    [HideInInspector] public int damagedHealth;
    private AudioSource audioSource;

    private WeaponEquipper weaponEquipper; // Reference to the WeaponEquipper script
    

    void Start()
    {
        weaponEquipper = FindFirstObjectByType<WeaponEquipper>(); // Assuming WeaponEquipper is on the player or a parent

        if (weaponEquipper == null)
        {
            Debug.LogError("WeaponEquipper not found on the player or parent!");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = 0.3f;

        // Initialize based on the currently equipped weapon (if any at start)
        UpdateWeaponComponents();
    }

    // Call this method whenever the equipped weapon changes
    public void UpdateWeaponComponents()
    {
        if (weaponEquipper != null && weaponEquipper.currentWeaponInstance != null)
        {
            weaponAnimator = weaponEquipper.currentWeaponInstance.GetComponent<Animator>();
            currentTrailRenderer = weaponEquipper.currentWeaponInstance.GetComponent<TrailRenderer>();
            currentSwipeParticles = weaponEquipper.currentWeaponInstance.GetComponent<ParticleSystem>(); // Assuming particles are on the weapon prefab too

            if (weaponAnimator == null)
            {
                Debug.LogWarning("Animator not found on the equipped weapon!");
            }
            if (currentTrailRenderer != null)
            {
                currentTrailRenderer.emitting = false;
            }
            if (currentSwipeParticles != null)
            {
                currentSwipeParticles.Stop();
            }
            if (currentWeaponData != null && currentWeaponData.weaponAnimatorOverride != null && weaponAnimator != null)
            {
                weaponAnimator.runtimeAnimatorController = currentWeaponData.weaponAnimatorOverride;
            }
        }
        else
        {
            weaponAnimator = null;
            currentTrailRenderer = null;
            currentSwipeParticles = null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
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
            Invoke("StopTrail", currentWeaponData.swingLength);
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
        StartCoroutine(EndSwingAfterDelay(currentWeaponData.swingLength));
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
            damagedHealth = targetHealth - currentWeaponData.damage;
        }
        else
        {
            Debug.LogError("No Weapon Data assigned to the Attack Manager!");
            damagedHealth = targetHealth;
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