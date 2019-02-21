using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

/*
	Handle all functions regarding user information
	including loading and saving
	IMPORTANT: Always set username
*/
public static class UserInfo {
	
	private static string username;					// The player's username

	// TODO: Transfer to a model to make script more decoupled and dynamic
	// Default values
	private static string[] phyla = new string[] {
		"Annelida", "Anthropoda", "Chordata", "Cnidaria",
		"Echinodermata", "Mollusca", "Nematoda", "Platyhelminthes",
		"Porifera"};
	private static string[] games = new string[] {
		"hear", "see", "where"
	};

	// Accesible properties to other scripts
	public static string Username { 
		get { 
			if (username != null)
				return username;
			else
				return "Eduard";			// TODO: Remove this, only for fail safe
		} set {
			username = value;
		}
	}
	
	// Get the user highscore using game type and user name
	public static int GetUserHighScore(string minigameType)
	{
		// Load data file using JSONParser
		JArray jArray = (JSONParser.ReadJson("user")).ToObject<JArray>();

		// Get user info belonging to the given username
		var userInfo =
			from p in jArray
			where p["name"].ToString() == Username
			select (JArray)p["highscore"];
	
		
		var highscores = new JArray(userInfo);
		// For each highscore result in highscores
		foreach (var result in highscores[0])
		{
			// Look for the specific game type
			if(result["game"].ToString() == minigameType)
			{
				// Return the high score
				return int.Parse(result["score"].ToString());
			}
		}
		
		// Fail safe return
		return 0;
	}

	public static void SaveUserHighScore(string minigameType, int newHighScore)
	{
		// Load data file using JSONParser
		JArray jArray = (JSONParser.ReadJson("user")).ToObject<JArray>();

		// Get user info belonging to the given username
		var userInfo =
			from p in jArray
			where p["name"].ToString() == Username
			select (JArray)p["highscore"];

		foreach (var highscores in userInfo)
		{
			// For each highscore result in highscores
			foreach (var result in highscores)
			{
				// Look for the specific game type
				if(result["game"].ToString() == minigameType)
				{
					// Set the new high score
					result["score"] = newHighScore;
				}
			}
		}
		
		// Save file
		JSONParser.WriteJson("user", jArray);
	}

	// Create a new user with default values
	// Used this as guide: www.newtonsoft.com/json/help/html/CreateJsonManually.htm
	public static void CreateNewUser(string newUsername)
	{
		// Load data file using JSONParser
		JArray jArray = (JSONParser.ReadJson("user")).ToObject<JArray>();

		// Create new user
		JObject user = new JObject();
		user["name"] = newUsername;

		// Create new progress
		JArray progress = new JArray();
		foreach(string phylum in phyla)
		{
			JObject p = new JObject();
			p["name"] = phylum;
			JArray done = new JArray();
			p["done"] = done;
			progress.Add(p);
		}

		// Creatte new highscore
		JArray highscore = new JArray();
		foreach(string game in games)
		{
			JObject g = new JObject();
			g["game"] = game;
			g["score"] = 0;
			highscore.Add(g);
		}

		// Add progress and highscore
		user["progress"] = progress;
		user["highscore"] = highscore;

		// Add new user
		jArray.Add(user);

		// Save to file
		JSONParser.WriteJson("user", jArray);
	}
}
