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
	
	private HighScoreManager _scoreManager;
	
	
	
	
	
	
	
	
	
	
	
	public async void PrintCurrentScores()
	{
	_scoreManager = GetNode<HighScoreManager>("/root/HighScoreManager");
	Label latestTimeLabel = GetNode<Label>("MarginContainer/VBoxContainer/LatestTimeLabel");
	Label fastestTimesLabel = GetNode<Label>("MarginContainer/VBoxContainer/FastestTimesLabel");
	
	//_scoreManager._Ready();
	// This prints immediately.
	

	// Wait for 0.3 seconds without blocking the main thread.
	await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
	latestTimeLabel.Text = Global.allData1;
	fastestTimesLabel.Text = Global.allData2;
	// This prints only after 0.3 seconds have passed.
	GD.Print("bbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb");
}     












	public override void _Ready()
	{
		
		PrintCurrentScores();
		
		
		Button mainMenuButton = GetNode<Button>("MarginContainer/VBoxContainer/Button");
		mainMenuButton.Pressed += OnMainMenuPressed;
		
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
		}
	}
}
