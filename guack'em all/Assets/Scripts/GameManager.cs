using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
  [SerializeField] private List<Mole> moles;

  [Header("UI objects")]
  [SerializeField] private GameObject gameUI;
  [SerializeField] private GameObject outOfTimeText;
  [SerializeField] private GameObject bombText;
  [SerializeField] private TMPro.TextMeshProUGUI timeText;
  [SerializeField] private TMPro.TextMeshProUGUI scoreText;

  [SerializeField] private TMPro.TextMeshProUGUI waveText;
  [SerializeField] private TMPro.TextMeshProUGUI nextWaveCountdownText;
  [SerializeField] private List<int> waveScoreThresholds = new List<int> { 10, 20, 30 };
  [SerializeField] private TMPro.TextMeshProUGUI scoreProgressText;

  [Header("Shop UI")]
[SerializeField] private GameObject shopUI;
[SerializeField] private Button buyChickenButton;
[SerializeField] private Button buyCactusButton;
[SerializeField] private Button buyMaracasButton;

[SerializeField] private TextMeshProUGUI chickenText;
[SerializeField] private TextMeshProUGUI cactusText;
[SerializeField] private TextMeshProUGUI maracasText;

[SerializeField] private AudioSource audioSource;
  [SerializeField] private AudioClip purchaseSound;
  [SerializeField] private Button skipButton;
private Coroutine nextWaveCoroutine;

public void SkipShop()
{
    if (nextWaveCoroutine != null)
    {
        StopCoroutine(nextWaveCoroutine);
    }
    shopUI.SetActive(false);
    nextWaveCountdownText.gameObject.SetActive(false);
    nextWaveCoroutine = StartCoroutine(WaveRoutine());
}



  private bool hasPurchased = false;
private int chicken = 0;
private int cactus = 0;
private int maracas = 0;



  // Hardcoded - can be tuned in the inspector.
  [SerializeField] private float startingTime = 30f;

  // Global variables
  private float timeRemaining;
  private HashSet<Mole> currentMoles = new HashSet<Mole>();
  private int score;
  private bool playing = false;
  private int currentWave = 0;
  private int maxWaves = 3;
  // Multiplier variables
  private int streakCount = 0;
  private int multiplier = 1;
  // Start multiplier after 4 hits
  private const int streakThreshold = 2;
  // Optional cap on multiplier
  private const int maxMultiplier = 5;



  // Delayed start to allow for other objects to Awake -
  // TODO Remove this logic when proper Scene Management has been implemented
  public void StartGameWithDelay()
  {
    StartCoroutine(DelayedCall(0.5f, StartGame));
  }

  private IEnumerator DelayedCall(float delaySeconds, System.Action methodToCall)
  {
    yield return new WaitForSeconds(delaySeconds);
    methodToCall?.Invoke();
  }

  void Start()
  {
    waveText.gameObject.SetActive(false);
    nextWaveCountdownText.gameObject.SetActive(false);
    shopUI.SetActive(false);
    StartGameWithDelay();
  }
  //----------------------------------------------------------------------------

  public void StartGame()
{
    outOfTimeText.SetActive(false);
    bombText.SetActive(false);
    gameUI.SetActive(true);
    waveText.gameObject.SetActive(true);
    nextWaveCountdownText.gameObject.SetActive(false);

    for (int i = 0; i < moles.Count; i++)
    {
        moles[i].Hide();
        moles[i].SetIndex(i);
    }

    currentMoles.Clear();
    score = 0;
    scoreText.text = "0";
    currentWave = 0;
    StartCoroutine(WaveRoutine());
}

  private IEnumerator WaveRoutine()
  {
    while (currentWave < maxWaves)
    {
      // Set progress text before incrementing currentWave
      int waveGoal = waveScoreThresholds.Count >= currentWave + 1
          ? waveScoreThresholds[currentWave]
          : waveScoreThresholds[waveScoreThresholds.Count - 1];

      scoreProgressText.text = $"Wave Score: 0 / {waveGoal}";

      // Increment wave after setting the UI
      currentWave++;
      waveText.text = $"Wave {currentWave} of {maxWaves}";
      Debug.Log($"Wave {currentWave} started!");

      nextWaveCountdownText.gameObject.SetActive(false);
      playing = true;
      timeRemaining = 30f;
      int scoreAtWaveStart = score;

      // Run the wave
      while (timeRemaining > 0f)
      {
        timeRemaining -= Time.deltaTime;
        timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";

        if (currentMoles.Count <= (score / 10))
        {
          int index = Random.Range(0, moles.Count);
          if (!currentMoles.Contains(moles[index]))
          {
            currentMoles.Add(moles[index]);
            moles[index].Activate(score / 10);
          }
        }

        yield return null;
      }

      // Handle the wave score and progress
      int waveScore = score - scoreAtWaveStart;
      if (waveScore < waveGoal)
      {
        Debug.Log($"Wave {currentWave} failed. Needed {waveGoal}, got {waveScore}.");
        GameOver(0);
        yield break;
      }

      // Wave finished
      playing = false;
      timeRemaining = 0f; // <-- Set this explicitly to 0
      timeText.text = "0:00"; // <-- Set the timer text to a clean value
      currentMoles.Clear();

      // Hide all moles at the end of the wave
      foreach (Mole mole in moles)
      {
        mole.StopGame();
        mole.Hide();
      }

      // Start the interval between waves
      if (currentWave < maxWaves)
      {
        // Show the shop UI
        shopUI.SetActive(true);
        hasPurchased = false;

        buyChickenButton.interactable = true;
        buyCactusButton.interactable = true;
        buyMaracasButton.interactable = true;

        Debug.Log("Shop is open for 10 seconds!");

        nextWaveCoroutine = StartCoroutine(NextWaveCountdown());
        yield return nextWaveCoroutine;

        shopUI.SetActive(false);
        nextWaveCountdownText.gameObject.SetActive(false);
      }
    }

    // All waves are done
    GameOver(0);

  }
private IEnumerator NextWaveCountdown()
{
    for (int i = 10; i > 0; i--)
    {
        nextWaveCountdownText.gameObject.SetActive(true);
        nextWaveCountdownText.text = $"Next wave in: {i}";
        yield return new WaitForSeconds(1f);
    }

    shopUI.SetActive(false);
    nextWaveCountdownText.gameObject.SetActive(false);
}
private void UpdateScoreUI()
{
    scoreText.text = $"{score}";
}
public void BuyChicken()
{
    if (hasPurchased || score < 500) return;

    score -= 50;
    chicken++;
    chickenText.text = "Equipped!";
    buyChickenButton.interactable = false;
    hasPurchased = true;
    audioSource.PlayOneShot(purchaseSound);
    UpdateScoreUI();
}

public void BuyCactus()
{
    if (hasPurchased || score < 350) return;

    score -= 35;
    cactus++;
    cactusText.text = "Equipped!";
    buyCactusButton.interactable = false;
    hasPurchased = true;
    audioSource.PlayOneShot(purchaseSound);
    UpdateScoreUI();
}

public void BuyMaracas()
{
    if (hasPurchased || score < 200) return;

    score -= 20;
    maracas++;
    maracasText.text = "Equipped!";
    buyMaracasButton.interactable = false;
    hasPurchased = true;
    audioSource.PlayOneShot(purchaseSound);
    UpdateScoreUI();
}

  public void GameOver(int type)
  {
    // Show the message.
    if (type == 0)
    {
      outOfTimeText.SetActive(true);
    }
    else
    {
      bombText.SetActive(true);
    }
    // Hide all moles.
    foreach (Mole mole in moles)
    {
      mole.StopGame();
    }
    // Stop the game and show the start UI.
    playing = false;
  }

  // Update is called once per frame
  void Update()
{
    if (playing)
    {
        timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";
        if (currentMoles.Count <= (score / 10))
        {
            int index = Random.Range(0, moles.Count);
            if (!currentMoles.Contains(moles[index]))
            {
                currentMoles.Add(moles[index]);
                moles[index].Activate(score / 10);
            }
        }
    }
}


  public void AddScore(int moleIndex)
  {
    // Add and update score.
    streakCount++;
    if (streakCount >= streakThreshold)
    {
      multiplier = 1 + (streakCount - streakThreshold + 1);
      multiplier = Mathf.Min(multiplier, maxMultiplier);
    }
    // Add and update score.
    score += 1 * multiplier;
    scoreText.text = $"{score}";

    // Increase time by a little bit.
    timeRemaining += 1;
    // Remove from active moles.
    currentMoles.Remove(moles[moleIndex]);
    // Calculate wave progress
    int waveScore = score;
    if (currentWave > 1)
    {
        for (int i = 0; i < currentWave - 1; i++)
        {
            waveScore -= waveScoreThresholds[i];
        }
    }

    int waveGoal = waveScoreThresholds.Count >= currentWave
        ? waveScoreThresholds[currentWave - 1]
        : waveScoreThresholds[waveScoreThresholds.Count - 1];

    // Update UI
   scoreProgressText.text = $"<color=green>Wave Score: {waveScore}</color> / {waveGoal}";
  }

  public void Missed(int moleIndex, bool isMole)
  {
    streakCount = 0;
    multiplier = 1;
    // Remove from active moles.
    currentMoles.Remove(moles[moleIndex]);
  }
}
