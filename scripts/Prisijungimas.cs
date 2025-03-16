using Godot;
using System;

public partial class Prisijungimas : Control
{
<<<<<<< HEAD

    private LineEdit usernameLineEdit;
    private LineEdit passwordLineEdit;
    private Button loginButton;
=======
	private LineEdit usernameLineEdit;
	private LineEdit passwordLineEdit;
	private Button loginButton;
>>>>>>> origin/animacija

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get references to UI elements
		usernameLineEdit = GetNodeOrNull<LineEdit>("LineEdit2");
		passwordLineEdit = GetNodeOrNull<LineEdit>("LineEdit3");
		loginButton = GetNodeOrNull<Button>("Panel/Button");

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

	private bool ValidateLogin(string username, string password)
	{
		// Replace with your actual login validation logic
		// For example, check against database, API, or hardcoded values
		return username == "admin" && password == "admin";
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
