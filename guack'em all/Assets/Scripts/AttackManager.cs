using System.Collections;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private Animator weaponAnimator;
    public TrailRenderer trailRenderer;
    public ParticleSystem swipeParticles;
    private float swipeLength = 0.3f; // Length of the attack swipe
    public bool IsPlayerSwinging { get; private set; } = false;
    [SerializeField] private int weaponDamage = 5; // Set the damage value for the weapon; TODO Replace with a dynamic variable, pulling from the currently held weapon.
    [HideInInspector] public int damagedHealth;
    [SerializeField] private Transform weaponTransform; // Assign the arm/weapon transform in the Inspector

    void Start()
    {
        if (weaponTransform != null)
        {
            weaponAnimator = weaponTransform.GetComponent<Animator>();
            if (weaponAnimator == null)
            {
                Debug.LogError("Animator not found on the weapon child object!");
            }
        }
        else
        {
            Debug.LogError("Weapon Transform not assigned in the Inspector!");
        }

        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }

        if (swipeParticles != null)
        {
            swipeParticles.Stop();
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
            if (trailRenderer != null)
            {
                trailRenderer.emitting = true;
            }

            // Play particles
            if (swipeParticles != null)
            {
                swipeParticles.Play();
            }

            // Disable trail shortly after
            Invoke("StopTrail", swipeLength); // Adjust delay based on animation length
        }
    }

    void StopTrail()
    {
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }
    }

    public void StartSwing()
    {
        IsPlayerSwinging = true;
        //Debug.Log("Swing started!");
        StartCoroutine(EndSwingAfterDelay(swipeLength));
    }
        IEnumerator EndSwingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EndSwing();
    }
    public void EndSwing()
    {
        IsPlayerSwinging = false;
        //Debug.Log("Swing ended!");
    }

    public void DealDamage(int targetHealth)
    {
        //Debug.Log("Dealing damage!");
        damagedHealth = targetHealth - weaponDamage;
    }
}