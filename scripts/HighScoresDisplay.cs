using Godot;
using System.Collections.Generic;

public partial class HighScoresDisplay : Control
{
	// Eksportuojame UI elementus pagal naujus kelius:
	// LatestTimeLabel – rodo esamo bandymo rezultatą
	[Export] private Label _latestTimeLabel;
	
	// FastestTimesLabel – rodo 10 geriausių rezultatų sąrašą
	[Export] private Label _fastestTimesLabel;
	// Papildomas Label pranešimui, jei nėra įrašų (nebūtina)
	[Export] private Label _noScoresLabel;
	
	// Add reference to the winning sound player
	[Export] private AudioStreamPlayer _winningSound;
	
	private HighScoreManager _scoreManager;
	
	public async void PrintCurrentScores()
	{
		_scoreManager = GetNode<HighScoreManager>("/root/HighScoreManager");
		Label latestTimeLabel = GetNode<Label>("MarginContainer/VBoxContainer/LatestTimeLabel");
		Label fastestTimesLabel = GetNode<Label>("MarginContainer/VBoxContainer/FastestTimesLabel");
		
		// Wait for 0.3 seconds without blocking the main thread.
		await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
		latestTimeLabel.Text = Global.allData1;
		fastestTimesLabel.Text = Global.allData2;
		
		// Play winning sound
		PlayWinningSound();
		
		GD.Print("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb");
	}     

	public override void _Ready()
	{
		PrintCurrentScores();
		
		Button mainMenuButton = GetNode<Button>("MarginContainer/VBoxContainer/Button");
		mainMenuButton.Pressed += OnMainMenuPressed;
		
		// Initialize winning sound reference if not set via Export
		if (_winningSound == null)
		{
			_winningSound = GetNode<AudioStreamPlayer>("Winning");
		}
		
		// Gauname HighScoreManager – įsitikinkite, kad jis yra užregistruotas kaip Autoload.
		if (_scoreManager == null)
		{
			GD.PrintErr("HighScoresDisplay: HighScoreManager nerastas! Įsitikinkite, kad jis nustatytas kaip Autoload.");
			if (_latestTimeLabel != null)
				_latestTimeLabel.Text = "Klaida: Rezultatų sistema neprieinama.";
			if (_noScoresLabel != null)
				_noScoresLabel.Text = "Klaida.";
			if (_fastestTimesLabel != null)
				_fastestTimesLabel.Visible = false;
			return;
		}

		GD.Print($"Global.elapsedTime: {Global.elapsedTime}, Global.user: {Global.user ?? "null"}");

		// Atlikti duomenų atnaujinimą
		UpdateLatestTime();
		PopulateScores();
	}

	private void PlayWinningSound()
	{
		if (_winningSound != null && !_winningSound.Playing)
		{
			GD.Print("Playing winning sound!");
			_winningSound.Play();
		}
		else if (_winningSound == null)
		{
			GD.PrintErr("HighScoresDisplay: _winningSound is not assigned!");
		}
	}

	private void UpdateLatestTime()
	{
		if (_scoreManager == null)
			return;

		float latestTimeMs = (float)Global.elapsedTime;
		string user = Global.user ?? "Unknown";
		_scoreManager.AddNewTime(latestTimeMs, user);
		GD.Print($"Pridėtas laikas: {latestTimeMs}ms, vartotojas: {user}");
	}

	public void PopulateScores()
	{
		if (_scoreManager == null)
			return;

		// Atnaujiname esamo (naujausio) bandymo rezultatą.
		if (_latestTimeLabel != null)
		{
			var (timeMs, user) = _scoreManager.GetLatestTime();
			_latestTimeLabel.Text = timeMs >= 0L
				? $"Naujausias bandymas: {HighScoreManager.FormatTimeMilliseconds(timeMs)} ({user})"
				: "Naujausias bandymas: (Nėra duomenų)";
			_latestTimeLabel.Visible = true;
			
			// Debug: spausdiname, kokia vertė priskirta LatestTimeLabel
			GD.Print("LatestTimeLabel teksto reikšmė: " + _latestTimeLabel.Text);
		}
		else
		{
			GD.PrintErr("HighScoresDisplay: _latestTimeLabel nepriskirtas.");
		}

		// Gauname 10 geriausių rezultatų sąrašą.
		List<(float Time, string User)> fastestTimes = _scoreManager.GetFastestTimes();
		GD.Print("Greičiausių rezultatų skaičius: " + fastestTimes.Count);

		if (_noScoresLabel != null)
		{
			_noScoresLabel.Visible = (fastestTimes.Count == 0);
		}

		if (_fastestTimesLabel != null)
		{
			string resultsText = "";
			for (int i = 0; i < fastestTimes.Count; i++)
			{
				resultsText += $"{i + 1}. {fastestTimes[i].User} {HighScoreManager.FormatTimeMilliseconds(fastestTimes[i].Time)}\n";
			}
			_fastestTimesLabel.Text = resultsText;
			_fastestTimesLabel.Visible = true;
			
			// Debug: spausdiname, kokia vertė priskirta FastestTimesLabel
			GD.Print("FastestTimesLabel teksto reikšmė:\n" + _fastestTimesLabel.Text);
		}
		else
		{
			GD.PrintErr("HighScoresDisplay: _fastestTimesLabel nepriskirtas.");
		}
	}

	private void OnMainMenuPressed()
	{
		GD.Print("Atgal mygtukas paspaustas.");
		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}

	public override void _Notification(int what)
	{
		if (what == NotificationVisibilityChanged && Visible)
		{
			UpdateLatestTime();
			PopulateScores();
			
			// Play winning sound when the scene becomes visible
			PlayWinningSound();
		}
	}
}
