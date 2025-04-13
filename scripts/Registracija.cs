using Godot;
using System;
using System.IO;
using System.Collections.Generic;

public partial class Registracija : Control
{
	private LineEdit username;
	private LineEdit password;
	private Button registerButton;
	private Button backButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		username = GetNodeOrNull<LineEdit>("Panel/LineEdit");
		password = GetNodeOrNull<LineEdit>("Panel/LineEdit2");
		registerButton = GetNodeOrNull<Button>("Panel/Button2");
		backButton = GetNodeOrNull<Button>("Panel/Button");
		
		registerButton.Pressed += registerButtonPressed;
		backButton.Pressed += backButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void registerButtonPressed()
	{
		String regUsername = username.Text.Trim();
		String regPassword = password.Text.Trim();
		
		Dictionary<string, string> userDictionary = ReadDataFileToDictionary("dataTest");
		
		if (String.IsNullOrEmpty(regUsername) || String.IsNullOrEmpty(regPassword) || regPassword.Length < 7)
		{
			ShowErrorPopup("Klaida", "Neteisingas vardas arba slaptazodis");
			GD.Print("Netinkmas slaptazodis");
			return;
		}
		
		if (userDictionary.ContainsKey(regUsername))
		{
			ShowErrorPopup("Klaida", "Toks vartotjas jau egzistuoja");
			GD.Print("Toks vartotojas jau egzistuoja");
			return;
		}
		
		SaveUserData(regUsername, regPassword);
	}
	
	public void SaveUserData(string user, string pass)
	{
		using (StreamWriter writer = new StreamWriter("dataTest", append:true))
		{
			writer.WriteLine($"{user}:{pass}");
		}
	}
	
	public void backButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Prisijungimas.tscn");
	}
	
	static Dictionary<string, string> ReadDataFileToDictionary(string filePath)
	{
		// Initialize a dictionary to store the key-value pairs
		Dictionary<string, string> dataDict = new Dictionary<string, string>();

		// Read the file line by line
		try
		{
			foreach (var line in File.ReadLines(filePath))
			{
				// Split each line by the colon ':'
				var parts = line.Split(':');

				if (parts.Length == 2)
				{
					// Add key-value pair to the dictionary
					dataDict[parts[0].Trim()] = parts[1].Trim();
				}
				else
				{
					GD.Print($"Skipping invalid line: {line}");
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error reading file: {ex.Message}");
		}

		return dataDict;
	}
	
	private void ShowErrorPopup(string title, string message)
	{
		// Create a new AcceptDialog (popup message)
		AcceptDialog popup = new AcceptDialog();
		AddChild(popup);

		// Set popup properties
		popup.Title = title;
		popup.DialogText = message;
		popup.Size = new Vector2I(300, 150);

		// Show the popup centered
		popup.PopupCentered();

		// Connect signals to remove the popup when closed
		popup.Confirmed += () => popup.QueueFree();
		popup.Canceled += () => popup.QueueFree();

		// No need to manually position the dialog since PopupCentered() does this
	}
}
