using Godot;

public static class SaveSystem
{
    private const string SAVE_PATH = "user://savegame.save";

    public static void SaveCurrentScene(string scenePath)
    {
        using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Write);
        if (file != null)
        {
            var data = new Godot.Collections.Dictionary
            {
                { "last_scene", scenePath }
            };
            file.StoreLine(Json.Stringify(data));
        }
    }

    public static string LoadLastScene()
    {
        if (!FileAccess.FileExists(SAVE_PATH))
            return null;

        using var file = FileAccess.Open(SAVE_PATH, FileAccess.ModeFlags.Read);
        if (file != null)
        {
            var data = Json.ParseString(file.GetLine()).AsGodotDictionary();
            return (string)data["last_scene"];
        }
        return null;
    }
}