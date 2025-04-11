// SaveSystem.cs
using Godot;
using System;

public static class SaveSystem
{
    private const string SAVE_PATH = "user://savegame.save";

    public static void SaveProgress(int currentLevel)
    {
        using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write);
        if (file == null)
        {
            GD.PrintErr("Failed to save: ", FileAccess.GetOpenError());
            return;
        }

        var data = new Godot.Collections.Dictionary
        {
            { "current_level", currentLevel }
        };

        file.StoreLine(Json.Stringify(data));
    }

    public static string LoadProgress()
    {
        if (!FileAccess.FileExists(SAVE_PATH))
            return "res://scenes/Ivadinis.tscn"; // Default to first level

        using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);
        if (file == null)
        {
            GD.PrintErr("Failed to load: ", FileAccess.GetOpenError());
            return "res://scenes/Ivadinis.tscn";
        }

        var data = Json.ParseString(file.GetLine()).AsGodotDictionary();
        return "res://scenes/Ivadinis.tscn";
    }

    public static void ResetProgress()
    {
        if (FileAccess.FileExists(SAVE_PATH))
            DirAccess.RemoveAbsolute(SAVE_PATH);
    }
}