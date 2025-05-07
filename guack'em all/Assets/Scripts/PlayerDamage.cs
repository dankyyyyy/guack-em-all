using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public int damage;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AvocadoHealth avocadoHealth = collision.gameObject.GetComponent<AvocadoHealth>();
            if (avocadoHealth != null)
            {
                avocadoHealth.TakeDamage(damage);
            }
        }
    }
}
