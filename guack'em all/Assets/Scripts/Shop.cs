using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
[SerializeField] private AudioClip purchaseSound;
    public int Coin;
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

    private bool hasPurchased = false; // 🆕 only allow one purchase

    void Start()
    {
        Coin = 2000;
        Coin_text.text = Coin.ToString();
    }

    public void BuyChicken()
    {
        if (hasPurchased) return;

        if (Coin >= 500)
        {
            Coin -= 500;
            Coin_text.text = Coin.ToString();

            Chicken += 1;
            Chicken_text.text = "Equipped!";
            BuyChickenButtonText.text = "Purchased!";
            BuyChickenButton.interactable = false;
            audioSource.PlayOneShot(purchaseSound);
            DisableOtherButtons(); // 🆕
            hasPurchased = true;
        }
        else
        {
            print("Not enough coins!");
        }
    }

    public void BuyCactus()
    {
        if (hasPurchased) return;

        if (Coin >= 350)
        {
            Coin -= 350;
            Coin_text.text = Coin.ToString();

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
            print("Not enough coins!");
        }
    }

    public void BuyMaracas()
    {
        if (hasPurchased) return;

        if (Coin >= 200)
        {
            Coin -= 200;
            Coin_text.text = Coin.ToString();

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
            print("Not enough coins!");
        }
    }

    // 🆕 Disable all unpurchased buttons
    void DisableOtherButtons()
    {
        BuyChickenButton.interactable = false;
        BuyCactusButton.interactable = false;
        BuyMaracasButton.interactable = false;
    }
}
