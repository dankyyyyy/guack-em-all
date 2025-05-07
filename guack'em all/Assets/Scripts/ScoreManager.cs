using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public Text scoreText;

    int score = 0;

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        scoreText.text = score.ToString() + " Pesos";
    }

    public void AddPoint(int amount) {
    score += amount;
    scoreText.text = score.ToString() + " Pesos";
}
}
