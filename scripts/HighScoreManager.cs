using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public partial class HighScoreManager : Node
{
	private const string SAVE_FILE_PATH = "res://high_scores.json";
	private const int MAX_FASTEST_TIMES = 10;

	private List<(float Time, string User)> _fastestTimes = new List<(float Time, string User)>();
	private (float Time, string User) _latestTime = (0L, "");

	private class ScoreData
	{
		public List<ScoreEntry> FastestTimes { get; set; }
	}

	private class ScoreEntry
	{
		public float Time { get; set; }
		public string User { get; set; }
	}

	public override void _Ready()
	{
		_latestTime.Time = Global.elapsedTime;
		GD.Print("HighScoreManager: _Ready vykdomas");
		LoadScores();
		GD.Print("HighScoreManager paruoštas (laikai ms su vartotojais, _latestTime nesaugomas).");
	}

	public void AddNewTime(float newTimeMs, string user)
	{
		GD.Print($"HighScoreManager: AddNewTime({newTimeMs}ms, {user})");
		_latestTime = (newTimeMs, user ?? "Unknown");
		GD.Print($"Naujas laikas pridėtas: {FormatTimeMilliseconds(newTimeMs)} ({newTimeMs}ms) vartotojas: {user}");

		_fastestTimes.Add((newTimeMs, user));
		_fastestTimes.Sort((a, b) => a.Time.CompareTo(b.Time));

		if (_fastestTimes.Count > MAX_FASTEST_TIMES)
		{
			_fastestTimes = _fastestTimes.Take(MAX_FASTEST_TIMES).ToList();
		}

		SaveScores();
		PrintCurrentScores();
	}

	public List<(float Time, string User)> GetFastestTimes()
	{
		GD.Print($"HighScoreManager: GetFastestTimes, įrašų skaičius: {_fastestTimes.Count}");
		return new List<(float Time, string User)>(_fastestTimes);
	}

	public (float Time, string User) GetLatestTime()
	{
		Global.Latest = $"{_latestTime.Time}ms, {_latestTime.User}";
		return _latestTime;
	}

	public void SaveScores()
	{
		GD.Print("HighScoreManager: SaveScores vykdomas");
		var scoreData = new ScoreData
		{
			FastestTimes = _fastestTimes.Select(entry => new ScoreEntry
			{
				Time = entry.Time,
				User = entry.User
			}).ToList()
		};

		try
		{
			string jsonString = JsonSerializer.Serialize(scoreData, new JsonSerializerOptions { WriteIndented = true });
			using var file = FileAccess.Open(SAVE_FILE_PATH, FileAccess.ModeFlags.Write);
			if (file == null)
			{
				GD.PrintErr($"Klaida atidarant failą rašymui ({SAVE_FILE_PATH}): {FileAccess.GetOpenError()}");
				return;
			}
			file.StoreString(jsonString);
			GD.Print($"Rezultatai išsaugoti į {SAVE_FILE_PATH}");
		}
		catch (Exception e)
		{
			GD.PrintErr($"Klaida saugant rezultatus: {e.Message}");
		}
	}

	public void  LoadScores()
	{
		GD.Print("HighScoreManager: LoadScores vykdomas");
		_latestTime = (0L, "");

		if (!FileAccess.FileExists(SAVE_FILE_PATH))
		{
			GD.Print($"{SAVE_FILE_PATH} nerastas. Kuriamas naujas failas su pavyzdiniais duomenimis.");
			InitializeDefaultScores();
			SaveScores();
			return;
		}

		try
		{
			using var file = FileAccess.Open(SAVE_FILE_PATH, FileAccess.ModeFlags.Read);
			if (file == null)
			{
				GD.PrintErr($"Klaida atidarant failą skaitymui ({SAVE_FILE_PATH}): {FileAccess.GetOpenError()}");
				InitializeDefaultScores();
				SaveScores();
				return;
			}

			string jsonString = file.GetAsText();
			GD.Print($"Įkeltas JSON turinys iš {SAVE_FILE_PATH}:\n{jsonString}");
			var loadedData = JsonSerializer.Deserialize<ScoreData>(jsonString);

			if (loadedData?.FastestTimes != null && loadedData.FastestTimes.Count > 0)
			{
				_fastestTimes = loadedData.FastestTimes
					.Select(entry => (entry.Time, entry.User ?? "Unknown"))
					.ToList();
				_fastestTimes.Sort((a, b) => a.Time.CompareTo(b.Time));
				if (_fastestTimes.Count > MAX_FASTEST_TIMES)
				{
					_fastestTimes = _fastestTimes.Take(MAX_FASTEST_TIMES).ToList();
				}
				GD.Print($"Sėkmingai įkelti {loadedData.FastestTimes.Count} rezultatai iš {SAVE_FILE_PATH}.");
			}
			else
			{
				GD.PrintErr($"Nepavyko deserializuoti rezultatų: FastestTimes yra null arba tuščias.");
				InitializeDefaultScores();
				SaveScores();
			}
		}
		catch (Exception e)
		{
			GD.PrintErr($"Klaida įkeliant rezultatus iš {SAVE_FILE_PATH}: {e.Message}");
			InitializeDefaultScores();
			SaveScores();
		}
	}

	private void InitializeDefaultScores()
	{
		GD.Print("HighScoreManager: InitializeDefaultScores vykdomas");
		_fastestTimes = new List<(float Time, string User)>
		{
			(1500, "Vartotojas1"),
			(2000, "Vartotojas2"),
			(2500, "Vartotojas3")
		};
		_latestTime = (0L, "");
		GD.Print("Inicializuoti pavyzdiniai rezultatai:");
		PrintCurrentScores();
	}

	public static string FormatTimeMilliseconds(float totalMilliseconds)
	{
		int minutes = (int)(totalMilliseconds/ 60);
		int seconds = (int)(totalMilliseconds % 60);
		int milliseconds = (int)((totalMilliseconds * 1000) % 1000);
		string Text = $"{minutes:00}:{seconds:00}:{milliseconds:000}";
		return Text;
	}

	public void PrintCurrentScores()
{
	string allData = "";

	allData += $"{FormatTimeMilliseconds(_latestTime.Time)}   {_latestTime.User} \n";
	allData += "\n";
	Global.allData1=allData;
	allData="";
	allData += "\n";
	// Append fastest times with a newline at the end of each record.
	for (int i = 0; i < _fastestTimes.Count; i++)
	{

		allData += $"{i + 1,-2}.   {FormatTimeMilliseconds(_fastestTimes[i].Time)}   {_fastestTimes[i].User} \n";
	}
	allData += "\n";
	allData += "\n";
	Global.allData2=allData;
	// If you want to view the complete string:
	GD.Print("Constructed output:\n" + allData);
	
	// Then print the header and list separately:
	GD.Print("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
	
}
}
