using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public bool IsSwinging { get; private set; } = false;

    // Call this to start a swing
    public void StartSwing()
    {
        IsSwinging = true;
        Debug.Log("Swing started!");
        // StartCoroutine(EndSwingAfterDelay(1f)); // Example duration
    }

    private System.Collections.IEnumerator EndSwingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        IsSwinging = false;
    }

    // You'll need to call StartSwing() from your Player's input handling
    // whenever the player initiates an attack. For example:
    //
    // In your Player.cs (or a separate attack input script):
    //
    // public PlayerAttack attackController;
    //
    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0)) // Example: Left mouse button for attack
    //     {
    //         attackController.StartSwing();
    //         // Potentially trigger attack animation or other effects
    //     }
    // }
}