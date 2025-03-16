using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json; // Import the JsonSerializer

// Example class
public class PlayerData
{
	public string PlayerName { get; set; }
	public int Score { get; set; }
	public float Health { get; set; }
	public int Level { get; set; }
	public List<string> Inventory { get; set; } = new List<string>();

	public PlayerData() { } // Default constructor

	public PlayerData(string name, int score, float health, int level)
	{
		PlayerName = name;
		Score = score;
		Health = health;
		Level = level;
		Inventory = new List<string>();
	}
}

public class SaveLoadManager
{
	// --- Simple Text File ---
	public static void SaveToTextFile(string filePath, string data)
	{
		try
		{
			File.WriteAllText(filePath, data, Encoding.UTF8);
			GD.Print($"Data saved to: {filePath}");
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Error saving to text file: {ex.Message}");
		}
	}

	public static string LoadFromTextFile(string filePath)
	{
		try
		{
			return File.Exists(filePath) ? File.ReadAllText(filePath, Encoding.UTF8) : null;
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Error loading from text file: {ex.Message}");
			return null;
		}
	}
}


public partial class MyGameNode : Node
{
	public override void _Ready()
	{
		// --- Text File Example ---
		SaveLoadManager.SaveToTextFile("user://save.txt", "This is my saved data from a Node!");
		string loadedData = SaveLoadManager.LoadFromTextFile("user://save.txt");
		GD.Print($"Loaded data (from Node): {loadedData}");

		// --- Attempt to load a non-existent file ---
		string nonExistentData = SaveLoadManager.LoadFromTextFile("user://nonexistent.txt");
		GD.Print($"Non-existent data: {nonExistentData}");

		// --- JSON Example ---
		PlayerData myPlayer = new PlayerData("Alice", 100, 95.5f, 5);
		myPlayer.Inventory.Add("Sword");
		myPlayer.Inventory.Add("Potion");

		SavePlayerData(myPlayer, "user://player_data.json");
		PlayerData loadedPlayer = LoadPlayerData("user://player_data.json");

		if (loadedPlayer != null)
		{
			GD.Print($"Loaded Player Name: {loadedPlayer.PlayerName}");
			GD.Print($"Loaded Player Score: {loadedPlayer.Score}");
		}

		//load non existent json file
		PlayerData nonExistentJsonPlayer = LoadPlayerData("user://nonexistentfile.json");
		if (nonExistentJsonPlayer != null)
		{
			GD.Print($"Loaded non existent Player Name: {nonExistentJsonPlayer.PlayerName}");
		}
	}

	// --- JSON Save/Load ---
	public void SavePlayerData(PlayerData player, string filePath)
	{
		try
		{
			string jsonString = JsonSerializer.Serialize(player, new JsonSerializerOptions { WriteIndented = true });
			File.WriteAllText(filePath, jsonString, Encoding.UTF8);
			GD.Print($"PlayerData saved to: {filePath}");
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Error saving PlayerData: {ex.Message}");
		}
	}

	public PlayerData LoadPlayerData(string filePath)
	{
		try
		{
			if (File.Exists(filePath))
			{
				string jsonString = File.ReadAllText(filePath, Encoding.UTF8);
				PlayerData data = JsonSerializer.Deserialize<PlayerData>(jsonString);
				if (data == null)
				{
					Console.WriteLine($"Failed to deserialize JSON from '{filePath}'.  Returning default value.");
					return new PlayerData(); // Return a new instance of T (using the default constructor)
				}
				return data;
			}
			else
			{
				GD.Print($"File not found: {filePath}");
				return new PlayerData();
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Error loading PlayerData: {ex.Message}");
			GD.Print($"Exception details: {ex}"); // More detailed error
			return new PlayerData();
		}
	}
}
