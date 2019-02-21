using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;
using VRStandardAssets.Common;

// This script handles all Game functions with
// regards to the different minigames. It will handle
// the flow and core mechanics of each game.
public class GameManager : Singleton<GameManager>
{
	[SerializeField] private Text currentScoreTxt;					// Current player's score
    [SerializeField] private Text currentTimeTxt;					// Current time left
    [SerializeField] private Text scoreEndTxt, highScoreTxt;		// This is used in the outro as final score and high score.
	[SerializeField] private SelectionSlider m_SelectionSlider;     // Used to confirm the user has understood the intro UI.
    [SerializeField] private SelectionRadial m_SelectionRadial;     // Used to continue past the outro.
    [SerializeField] private Reticle m_Reticle;                     // This is turned on and off when it is required and not.
	[SerializeField] private float roundTime;                       // This is the time per round.
	[SerializeField] private ScoreManager.MinigameType gameType;    // Used to identify the minigame type
    [SerializeField] private SpawnManager spawn;					// Used in per round spawning    
    [SerializeField] private AudioSource AudioPlayer;               // Reference to Audio source

    
    public bool IsPlaying { get; set; }                             // Helps identifies wether the game is currently playing
    private GameObject correct;                                     // The correct game object
    private float gameTimer;                                        // Used in actual manipulation of game timer
    private bool round, alive;                                      // Used in validating game status

    // Generic function call, infinite loops until player decides to leave
    private IEnumerator Start()
    {
        // Set the game type for the score to be recorded correctly.
        ScoreManager.SetGameType(gameType);

        // Continue looping throuh all phases
        while (true)
        {
            yield return StartCoroutine(StartPhase());
            yield return StartCoroutine(PlayPhase());
            yield return StartCoroutine(EndPhase());
        }
    }

    private IEnumerator StartPhase()
    {
        // Wait for the intro UI to fade in.
        yield return StartCoroutine(UIController.Instance.ShowIntroUI());

        // Show the reticle (since there is now a selection slider) and hide the radial.
        m_Reticle.Show();
        m_SelectionRadial.Hide();

        // Wait for the selection slider to finish filling.
        yield return StartCoroutine(m_SelectionSlider.WaitForBarToFill());

        // Wait for the intro UI to fade out.
        yield return StartCoroutine(UIController.Instance.HideIntroUI());
    }

    private IEnumerator PlayPhase()
    {
        // Setup UI
        currentScoreTxt.text = "0";
        currentTimeTxt.text = roundTime.ToString();

        // Wait for the time and score UI to fade in
        yield return StartCoroutine(UIController.Instance.ShowPlayerUI());

        // The game is now playing.
        IsPlaying = true;

        // Make sure the reticle is being shown.
        m_Reticle.Show();

        // Reset the score.
        ScoreManager.Restart();

        // Wait for the play updates to finish.
        yield return StartCoroutine(PlayUpdate());

        // Wait for the time and score UI to fade.
        yield return StartCoroutine(UIController.Instance.HidePlayerUI());

        // Save the highscore if necessarry
        ScoreManager.SaveHighScore();

        // The game is no longer playing.
        IsPlaying = false;
    }

    private IEnumerator EndPhase()
    {   
        // Set up Outro UI
        scoreEndTxt.text = ScoreManager.Score.ToString();
        highScoreTxt.text = ScoreManager.HighScore.ToString();

        // Hide the reticle since the radial is about to be used.
        m_Reticle.Hide();

        // In order, wait for the outro UI to fade in then wait for an additional delay.
        yield return StartCoroutine(UIController.Instance.ShowOutroUI());

        // Show the reticle (since there is now a selection slider) and hide the radial.
        m_Reticle.Show();
        m_SelectionRadial.Hide();

        // Wait for the selection slider to finish filling.
        yield return StartCoroutine(m_SelectionSlider.WaitForBarToFill());

        // Wait for the outro UI to fade out.
        yield return StartCoroutine(UIController.Instance.HideOutroUI());
    }

    private IEnumerator PlayUpdate()
    {
        gameTimer = roundTime;      // Set the time remaining to the full length of the game length.

        alive = true;               // The player is alive
        while (alive)
        {
            correct = spawn.SpawnGameAnimals();             // Spawn the Animals and get the correct answer

            // If the game type is set to hear, play sounds
            if (gameType == ScoreManager.MinigameType.HEAR)
            {
                AudioPlayer.clip = correct.GetComponent<Animal>().Sound;        // Get the sound of the correct animal
                AudioPlayer.Play();
            }

            round = true;                               // Start of round
            while (round && gameTimer > 0f)
            {
                // Wait for the next frame.
                yield return null;

                // Decrease the timers by the time that was waited.
                gameTimer -= Time.deltaTime;
                currentTimeTxt.text = Mathf.RoundToInt(gameTimer).ToString();
            }

            // Gametimer expired, player is now dead
            if (round)
                alive = false;
            
            // Stop audio from playing
            if (gameType == ScoreManager.MinigameType.HEAR)
                AudioPlayer.Stop();

            // Destroy existing animals
            SpawnManager.Instance.Clear();
        }
    }

    // Chekcs if the provided game object is the correct answer
    public void Check(GameObject obj)
    {
        obj.GetComponent<Gaze>().GazeOver = false;
        if (obj.Equals(correct))
        {
            gameTimer = roundTime;                                              // Reset the timer
            ScoreManager.AddScore(1);                                           // Add score in ScoreManager
            currentScoreTxt.text = ScoreManager.Score.ToString();               // Change score in UI
            round = false;                                                      // End the round
        }
        else
        {
            // Player was wrong, end the game
            alive = false;
            round = false;
        }
    }
    
    // Handles Over event of gaze function
    public void AnimalGazeOver()
    {
        
    }
}