using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	Handles all functions related to user score
	Can load score from user data and restart
	add, and overwrite high score.
*/
public static class ScoreManager
{

    // Enum to identify the different minigames
    public enum MinigameType
    {
        SEE,
        HEAR,
        WHERE
    };

    private static string currentGame;									// Used to identify which game is currently playing
    private static int score;                                           // Stores the current game's score
    private static int highScore;                                       // Store the current game's high score
    private static bool highScoreChanged;                               // Keeps track of high score change


    // Accesible properties to other scripts
    public static int Score { get { return score; } }
    public static int HighScore { get { return highScore; } }

    public static void SetGameType(MinigameType gameType)
    {
        // Set the current game based on the game type.
        switch (gameType)
        {
            case MinigameType.SEE:
                currentGame = "see";
                break;

            case MinigameType.HEAR:
                currentGame = "hear";
                break;
            case MinigameType.WHERE:
                currentGame = "where";
                break;
            default:
                Debug.Log("Invalid GameType");
                break;

        }

        //Restart();
    }

    public static void Restart()
    {
        // Reset the current score and get the highscore from the score manager.
        score = 0;
        highScore = GetHighScore();
        highScoreChanged = false;
    }

    public static int GetHighScore()
    {
        // Get the value of the highscore from the user data.
        return UserInfo.GetUserHighScore(currentGame);
    }

    public static void AddScore(int add)
    {
        // Add to the current score and check if the high score needs to be set.
        score += add;
        // Check if high score changed
        CheckHighScore();
    }

    private static void CheckHighScore()
    {
        // If the current score is greater than the high score then set the high score.
        if (score > highScore)
        {
            SetHighScore();
            highScoreChanged = true;
        }
    }

    private static void SetHighScore()
    {
        // Make sure the name of the current game has been set.
        if (string.IsNullOrEmpty(currentGame))
            Debug.LogError("currentGame not set");

        // The high score is now equal to the current score.
        highScore = score;
    }

    public static void SaveHighScore()
    {
        // Handle saving of high score if changed
        if (highScoreChanged)
            UserInfo.SaveUserHighScore(currentGame, highScore);
    }
}
