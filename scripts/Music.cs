using Godot;
using System;

public partial class Music : Control
{
	public AudioStreamPlayer song1;
	public AudioStreamPlayer song2;
	public AudioStreamPlayer song3;
	public AudioStreamPlayer song4;
	public AudioStreamPlayer song5;
	public AudioStreamPlayer current;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		song1 = GetNode<AudioStreamPlayer>("Song1");
		song2 = GetNode<AudioStreamPlayer>("Song2");
		song3 = GetNode<AudioStreamPlayer>("Song3");
		song4 = GetNode<AudioStreamPlayer>("Song4");
		song5 = GetNode<AudioStreamPlayer>("Song5");
		current = song3;
		current.Play();
		
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void switchSong(string song)
	{

			switch(song)
			{
				case "Song1":
					current.Stop();
					current = song1;
					current.Play();
					break;
				case "Song2":
					current.Stop();
					current = song2;
					current.Play();
					break;
				case "Song3":
					current.Stop();
					current = song3;
					current.Play();
					break;
				case "Song4":
					current.Stop();
					current = song4;
					current.Play();
					break;
				case "Song5":
					current.Stop();
					current = song5;
					current.Play();
					break;
				default:
					Console.WriteLine("Song not found.");
					break;
			}
		


	}
	
	
	
}
