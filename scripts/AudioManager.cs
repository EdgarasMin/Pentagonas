using Godot;

public partial class AudioManager : Node
{
	private AudioStreamPlayer audioPlayer;

	public override void _Ready()
	{
		Instance = this;
		audioPlayer = new AudioStreamPlayer();
		AddChild(audioPlayer);
		audioPlayer.Bus = "SFX";
	}

	public static AudioManager Instance { get; private set; }
	public static void PlaySound(AudioStream sound)
	{
		if (Instance.audioPlayer.Playing)
			Instance.audioPlayer.Stop();

		Instance.audioPlayer.Stream = sound;
		Instance.audioPlayer.Play();
	}
}
