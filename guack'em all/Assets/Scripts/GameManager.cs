using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
[SerializeField] private List<Cado> moles;

[Header("UI objects")]
[SerializeField] private GameObject playButton;
[SerializeField] private GameObject gameUI;
[SerializeField] private GameObject outOfTimeText;
[SerializeField] private GameObject bombText;
[SerializeField] private TMPro.TextMeshProUGUI timeText;
[SerializeField] private TMPro.TextMeshProUGUI scoreText;



// Hardcoded variables you may want to tune. 
private float startingTime = 30f;

// Global variables 
private float timeRemaining;
private HashSet<Cado> currentMoles = new HashSet<Cado>();
private int score;
private bool playing = false; 

// This is public so the play button can see it. 
public void StartGame(){
    // Hide/show the UI elements we dont/do want to see.
    playButton.SetActive(false);
    outOfTimeText.SetActive(false);
    bombText.SetActive(false);
    gameUI.SetActive(true);
    // Hide all the visible moles. 
    for (int i = 0; i < moles.Count; i++) {
        moles[i].Hide();
        moles[i].SetIndex(i);
    }
    //Remove any old game state. 
    currentMoles.Clear();
    // start with 30 seconds. 
    timeRemaining = startingTime;
    score = 0;
    scoreText.text = "0";
    playing = true;
}

public void GameOver(int type) {
    // show the message 
    if (type == 0) {
        outOfTimeText.SetActive(true);
    } else {
        bombText.SetActive(true);
    }
    
    //hide all moles 
    foreach (Cado mole in moles) {
        mole.StopGame();
    }
    // Stop the game and show the start ui.
    playing = false;
    playButton.SetActive(true);
}


// update is called once per grame
void Update()
{ if (playing) {
    // Update time.
    timeRemaining -= Time.deltaTime;
    if (timeRemaining <= 0) {
        timeRemaining = 0;
        GameOver(0);
        }
        timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";
    // check if we need to start any moles. 
    if (currentMoles.Count <= (score / 10)) {
        // Chose a random mole 
        int index = Random.Range(0, moles.Count);
        // Doesn't matter if its already doing something, we'll just try again next frame
        if (!currentMoles.Contains(moles[index])) {
            currentMoles.Add(moles[index]);
            moles[index].Activate(score / 10);
            }
        }
    }    
}

public void AddScore(int moleIndex) {
    // add an update score.
    score += 1;
    scoreText.text = $"{score}";
    // increase time by a little bit. 
    timeRemaining += 1;
    // remove from active moles 
    currentMoles.Remove(moles[moleIndex]);

}


public void Missed (int moleIndex, bool isMole) {
    if(isMole) {
        //decrease time by a little bit. 
        timeRemaining -= 2;
    }
    // remove from active moles. 
    currentMoles.Remove(moles[moleIndex]);
}

}
