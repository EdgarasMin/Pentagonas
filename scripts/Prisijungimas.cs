using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class Prisijungimas : Control
{
	private LineEdit usernameLineEdit;
	private LineEdit passwordLineEdit;
	private Button loginButton;
	private Button registerButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get references to UI elements
		AudioServer.SetBusMute(0,true);
		usernameLineEdit = GetNodeOrNull<LineEdit>("LineEdit2");
		passwordLineEdit = GetNodeOrNull<LineEdit>("LineEdit3");
		loginButton = GetNodeOrNull<Button>("Panel/Button");
		registerButton = GetNodeOrNull<Button>("Panel/Button2");
		registerButton.Pressed += registerButtonPressed;
		// Debug output to check if nodes were found
		GD.Print($"Username LineEdit found: {usernameLineEdit != null}");
		GD.Print($"Password LineEdit found: {passwordLineEdit != null}");
		GD.Print($"Login Button found: {loginButton != null}");

		// Check if all UI elements were found
		if (loginButton != null)
		{
			// Connect button press event
			loginButton.Pressed += LoginButtonPressed;
			GD.Print("Login button connected");
		}
		else
		{
			GD.PrintErr("Login button not found");
		}

		GD.Print("Login phase");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void LoginButtonPressed()
	{
		GD.Print("Login Button Pressed");

		// Check if LineEdit controls exist before accessing them
		if (usernameLineEdit == null || passwordLineEdit == null)
		{
			GD.PrintErr("Username or password input fields not found");
			ShowErrorPopup("Error", "Login fields not found");
			return;
		}

		// Get entered username and password
		string username = usernameLineEdit.Text;
		string password = passwordLineEdit.Text;

		GD.Print($"Login attempt - Username: {username}, Password: {password}");

		// Check login credentials
		if (ValidateLogin(username, password))
		{
			Global.user=username;
			GD.Print("Login successful");
			GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
		}
		else
		{
			GD.Print("Login failed");
			// Show error message popup
			ShowErrorPopup("Login Failed", "Invalid username or password");
		}
	}
	
	public void registerButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/Registracija.tscn");
	}

	private bool ValidateLogin(string username, string password)
	{
		Dictionary<string, string> userDictionary = ReadDataFileToDictionary("dataTest");
		
		// Replace with your actual login validation logic
		// For example, check against database, API, or hardcoded values
		//return username == "admin" && password == "admin";
		if (userDictionary.ContainsKey(username))
			return password == userDictionary[username];
		return false;
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
}
