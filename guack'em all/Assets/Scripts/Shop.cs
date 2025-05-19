using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip purchaseSound;

    public int Maracas;
    public int Cactus;
    public int Chicken;

    public TextMeshProUGUI Coin_text;
    public TextMeshProUGUI Maracas_text;
    public TextMeshProUGUI Cactus_text;
    public TextMeshProUGUI Chicken_text;

    public Button BuyChickenButton;
    public Button BuyCactusButton;
    public Button BuyMaracasButton;

    public TextMeshProUGUI BuyChickenButtonText;
    public TextMeshProUGUI BuyCactusButtonText;
    public TextMeshProUGUI BuyMaracasButtonText;

    private bool hasPurchased = false;

    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // Update the coin display every frame
        Coin_text.text = $"{gameManager.GetScore()}";
    }

    public void BuyChicken()
    {
        if (hasPurchased) return;

        if (gameManager.GetScore() >= 50)
        {
            gameManager.SubtractScore(50);
            Chicken += 1;
            Chicken_text.text = "Equipped!";
            BuyChickenButtonText.text = "Purchased!";
            BuyChickenButton.interactable = false;
            audioSource.PlayOneShot(purchaseSound);
            DisableOtherButtons();
            hasPurchased = true;
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    public void BuyCactus()
    {
        if (hasPurchased) return;

        if (gameManager.GetScore() >= 35)
        {
            gameManager.SubtractScore(35);
            Cactus += 1;
            Cactus_text.text = "Equipped!";
            BuyCactusButtonText.text = "Purchased!";
            BuyCactusButton.interactable = false;
            audioSource.PlayOneShot(purchaseSound);
            DisableOtherButtons();
            hasPurchased = true;
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    public void BuyMaracas()
    {
        if (hasPurchased) return;

        if (gameManager.GetScore() >= 20)
        {
            gameManager.SubtractScore(20);
            Maracas += 1;
            Maracas_text.text = "Equipped!";
            BuyMaracasButtonText.text = "Purchased!";
            BuyMaracasButton.interactable = false;
            audioSource.PlayOneShot(purchaseSound);
            DisableOtherButtons();
            hasPurchased = true;
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    private void DisableOtherButtons()
    {
        BuyChickenButton.interactable = false;
        BuyCactusButton.interactable = false;
        BuyMaracasButton.interactable = false;
    }
}
