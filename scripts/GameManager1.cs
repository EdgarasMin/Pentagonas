using Godot;

public partial class GameManager1 : Node
{
    //public static GameManager Instance { get; private set; }
    public string CurrentScene { get; private set; }

    public override void _Ready()
    {
        //if (Instance != null)
        //{
        //    QueueFree();
        //    return;
        //}

        //Instance = this;
        CurrentScene = SaveSystem.LoadLastScene() ?? "res://scenes/Level1.tscn"; // Default scene
        GetTree().ChangeSceneToFile(CurrentScene);
    }

    public void SaveCurrentScene()
    {
        if (GetTree().CurrentScene != null)
        {
            CurrentScene = GetTree().CurrentScene.SceneFilePath;
            SaveSystem.SaveCurrentScene(CurrentScene);
            GD.Print("Saved scene: ", CurrentScene);
        }
    }

    public void LoadNextLevel()
    {
        string nextScene = CurrentScene switch
        {
            "res://scenes/Level1.tscn" => "res://scenes/Level2.tscn",
            "res://scenes/Level2.tscn" => "res://scenes/Level3.tscn",
            _ => "res://scenes/MainMenu.tscn" // Default fallback
        };

        CurrentScene = nextScene;
        SaveSystem.SaveCurrentScene(CurrentScene);
        GetTree().ChangeSceneToFile(CurrentScene);
    }

    public void ResetProgress()
    {
        CurrentScene = "res://scenes/Level1.tscn";
        SaveSystem.SaveCurrentScene(CurrentScene);
        GetTree().ChangeSceneToFile(CurrentScene);
    }
}