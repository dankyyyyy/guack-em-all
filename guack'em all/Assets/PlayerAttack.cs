using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    public TrailRenderer trailRenderer;
    public ParticleSystem swipeParticles; // <-- Add this

    void Start()
    {
        animator = GetComponent<Animator>();
        trailRenderer.emitting = false;

        // Optional: Stop particle system if it loops
        if (swipeParticles != null)
        {
            swipeParticles.Stop();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Attack");

            // Enable trail
            trailRenderer.emitting = true;

            // Play particles
            if (swipeParticles != null)
            {
                swipeParticles.Play();
            }

            // Disable trail shortly after (can be timed with animation or use coroutine)
            Invoke("StopTrail", 0.3f); // Adjust delay based on animation
        }
    }

    void StopTrail()
    {
        trailRenderer.emitting = false;
    }
}
