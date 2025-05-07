using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;
    public AudioClip deathSound; // Assign in Inspector
    private AudioSource audioSource;
     private SpriteRenderer spriteRenderer;
    private bool isDead = false; // Prevent multiple calls
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ✅ add this line
    }

    public void TakeDamage(int damage) {
        if (isDead) return;
        health -= damage;
        if(health <= 0)
        {
            isDead = true;
            ScoreManager.instance.AddPoint(4);
            PlayDeathEffect();
        }
        void PlayDeathEffect()
    {
        if (deathSound != null)
    {
        GameObject tempGO = new GameObject("TempAudio"); // create the temp object
        tempGO.transform.position = transform.position; // set position

        AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add AudioSource
        aSource.clip = deathSound;
        aSource.volume = 20.0f; // LOUDER (can even set >1.0 if AudioSource allows)
        aSource.Play();

        Destroy(tempGO, deathSound.length); // destroy object after clip finishes playing
    }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red; // Change color to red
        }

        
    
        Destroy(gameObject);
    
    }
    
    }
}
