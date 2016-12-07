using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    // Declare a bunch of variables
    public static GameManager instance = null;
    public Text scoreText;
    public Text instructionsText;
    public int playerScore;
    public int opponentScore;
    public bool gameIsRunning;
    public bool rallyStart;

    void Awake () {
        // Enforce singleton
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        // Keep when reloading scene
        DontDestroyOnLoad(gameObject);

        // Tell everyone that the game is running
        gameIsRunning = true;
    }

    void Update () {
        StartCoroutine(CheckScore());
    }

    IEnumerator CheckScore () {
        if (playerScore == 21 || opponentScore == 21) {
            gameIsRunning = false;
            yield return new WaitForSeconds(1f);
            UpdateScore();
        }
        yield return null;
    }

    // Public function to increment player score
    public void IncrementPlayerScore () {
        playerScore++;
        UpdateScore();
    }

    // Public function to increment opponent score
    public void IncrementOpponentScore () {
        opponentScore++;
        UpdateScore();
    }

    // Update the score
    void UpdateScore () {
        scoreText.text = opponentScore + " - " + playerScore;
    }

    public IEnumerator RallyIsStarting() {
        // For one frame, let the rally be starting
        rallyStart = true;
        yield return null;
        rallyStart = false;
    }

    public IEnumerator ReadyGo() {
        // Pause the game
        gameIsRunning = false;
        instructionsText.text = "Ready?\n3";
        yield return new WaitForSeconds(.5f);
        instructionsText.text = "Ready?\n2";
        yield return new WaitForSeconds(.5f);
        instructionsText.text = "Ready?\n1";
        yield return new WaitForSeconds(.5f);
        instructionsText.text = "Go!";
        gameIsRunning = true;
        yield return new WaitForSeconds(.5f);
        instructionsText.text = "";
    }
}
