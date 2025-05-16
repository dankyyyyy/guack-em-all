using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public int Coin;
    public int Maracas;
    public int Cactus;
    public int Chicken;

    public TextMeshProUGUI Coin_text;
    public TextMeshProUGUI Maracas_text;
    public TextMeshProUGUI Cactus_text;
    public TextMeshProUGUI Chicken_text;

       public TextMeshProUGUI BuyChickenButtonText;
    public TextMeshProUGUI BuyCactusButtonText;
    public TextMeshProUGUI BuyMaracasButtonText;

    void Start()
    {
        Coin = 2000;
        Coin_text.text = Coin.ToString();
    }

    public void BuyChicken()
    {
        if (Coin >= 500)
        {
            Coin -= 500;
            Coin_text.text = Coin.ToString();

            Chicken += 1;
            Chicken_text.text = "Equipped!";
            BuyChickenButtonText.text = "Purchased!";
        }
        else
        {
            print("Not enough coins!");
        }
    }

    public void BuyCactus()
    {
        if (Coin >= 350)
        {
            Coin -= 350;
            Coin_text.text = Coin.ToString();

            Cactus += 1;
            Cactus_text.text = "Purchased!";
        }
        else
        {
            print("Not enough coins!");
        }
    }

    public void BuyMaracas()
    {
        if (Coin >= 200)
        {
            Coin -= 200;
            Coin_text.text = Coin.ToString();

            Maracas += 1;
            Maracas_text.text = "Purchased!";
        }
        else
        {
            print("Not enough coins!");
        }
    }
}
