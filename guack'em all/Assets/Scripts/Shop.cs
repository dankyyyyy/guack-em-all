using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    
        public int Coin;
        public int Avocado;
        public Text Coin_text;
        public Text Avocado_text;
    void Start()
    {
            Coin = 2000;
            Coin_text.text = Coin.ToString();
    }

 public void BuyAvocado() 
 {
        if(Coin >= 200)
        {
            Coin -= 200;
            Coin_text.text = Coin.ToString();

            Avocado += 1;
            Avocado_text.text = Avocado.ToString();
        }
        else 
        {
            print("Not enough coins!");
        }
 }
}
