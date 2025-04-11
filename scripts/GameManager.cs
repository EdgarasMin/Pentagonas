// GameManager.cs
using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    public int CurrentLevel { get; private set; } = 1;

    public override void _Ready()
    {
        if (Instance != null)
        {
            QueueFree();
            return;
        }

        Instance = this;
       // CurrentLevel = SaveSystem.LoadProgress();
    }

    public void CompleteCurrentLevel()
    {
        CurrentLevel = Math.Clamp(CurrentLevel + 1, 1, 3); // Assuming 3 levels
        SaveSystem.SaveProgress(CurrentLevel);
    }

    public void LoadCurrentLevel()
    {
        GetTree().ChangeSceneToFile($"res://Levels/Level{CurrentLevel}.tscn");
    }

    public void ResetProgress()
    {
        CurrentLevel = 1;
        SaveSystem.ResetProgress();
    }
}