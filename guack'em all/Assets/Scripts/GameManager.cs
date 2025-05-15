using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



  // Hardcoded - can be tuned in the inspector.
  [SerializeField] private float startingTime = 30f;

  // Global variables
  private float timeRemaining;
  private HashSet<Mole> currentMoles = new HashSet<Mole>();
  private int score;
  private bool playing = false;
  private int currentWave = 0;
private int maxWaves = 3;



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
            Debug.Log("Wave ended! Next wave starting in 10 seconds...");

            // Show the countdown text only during the interval
            nextWaveCountdownText.gameObject.SetActive(true);
            for (int i = 10; i > 0; i--)
            {
                nextWaveCountdownText.text = $"Next wave in: {i}";
                yield return new WaitForSeconds(1f);
            }

            // Hide the countdown text after the interval
            nextWaveCountdownText.gameObject.SetActive(false);
        }
    }

    // All waves are done
    GameOver(0);
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
    score += 1;
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
    
    // Remove from active moles.
    currentMoles.Remove(moles[moleIndex]);
  }
}