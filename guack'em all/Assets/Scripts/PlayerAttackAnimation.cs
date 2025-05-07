using UnityEngine;

public class PlayerAttackAnimation : MonoBehaviour
{
    private Animator animator;
    public TrailRenderer trailRenderer;
    public ParticleSystem swipeParticles; // <-- Add this
    [SerializeField] private PlayerAttack playerAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
        trailRenderer.emitting = false;

        // Attach PlayerAttack to the same GameObject
        playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack == null)
        {
            Debug.LogError("PlayerAttack component not found on the same GameObject as PlayerAttackAnimation!");
        }

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

            //Making sure the attack is registered as a swing
            if (playerAttack != null)
            {
                playerAttack.StartSwing();
            }

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
