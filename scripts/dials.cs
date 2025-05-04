using Godot;
using System;

public partial class dials : Area2D
{
	
	public CharacterBody2D specificCharacter;
	public bool inArea = false;
	TextureRect textureRect;
	Texture2D newTexture1;
	Texture2D newTexture2;
	Texture2D newTexture3;
	Texture2D newTexture4;
	Label text;
	public string[] texts;
	
	
	public int x = 1;
	
	private void OnAreaBodyEntered(Node body)
	{
		// Check if the body is of type CharacterBody2D
		if (body == specificCharacter)
		{
			inArea = true;
			RunWhileInArea();
			//GD.Print("Character entered the area!");
		}
	}
	
	private void OnAreaBodyExited(Node body)
	{
		// Check if the body is of type CharacterBody2D
		if (body == specificCharacter)
		{
			inArea = false;
			//GD.Print("Character left the area!");
		}
	}
	
	private async void RunWhileInArea()
	{
		while (inArea)
		{
			await ToSignal(GetTree(), "process_frame"); // Waits one frame to avoid freezing
		}

		GD.Print("Character left the area.");
	}
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		specificCharacter = GetNode<CharacterBody2D>($"../CharacterBody2D");
		textureRect = GetNode<TextureRect>("TextureRect");
		
		newTexture1 = GD.Load<Texture2D>("res://assets/Level4/dial1.png");
		newTexture2 = GD.Load<Texture2D>("res://assets/Level4/dial2.png");
		newTexture3 = GD.Load<Texture2D>("res://assets/Level4/dial3.png");
		newTexture4 = GD.Load<Texture2D>("res://assets/Level4/dial4.png");
		
		text = GetNode<Label>("Label");
		//text.Text = "Test";
		// Connect the 'body_entered' signal to the method
		this.BodyEntered += OnAreaBodyEntered;
		this.BodyExited += OnAreaBodyExited;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	//2 3 1 4 4 2
	public override void _Process(double delta)
	{
		if (!inArea)
			return;
			
		switch (Name)
		{
			case "line1":
				texts = new string[] { "int score = rand() % 100;", "#include <iostream>", "cout << fixed << setprecision(2) << 3.14159;", "map<char, int> letterCount;" };
				break;
			case "line2":
				texts = new string[] { "bool isEven = (number % 2 == 0);", "char grade = (marks > 90) ? 'A' : 'B';", "using namespace std;", "auto now = chrono::system_clock::now();" };
				break;
			case "line3":
				texts = new string[] { "int main() {", "unique_ptr<int> ptr = make_unique<int>(42);", "int* p = new int(25);", "delete p;" };
				break;
			case "line4":
				texts = new string[] { "array<int, 5> nums = {1, 2, 3, 4, 5};", "enum Color { RED, GREEN, BLUE };", "Color favorite = GREEN;", "    int x = 5, y = 10;" };
				break;
			case "line5":
				texts = new string[] { "mutex mtx;", "lock_guard<mutex> lock(mtx);", "int result = static_cast<int>(3.7);", "    int temp = x; x = y; y = temp;" };
				break;
			case "line6":
				texts = new string[] { "GD.Print(x)", "    cout << \"x=\" << x << \", y=\" << y;}", "x++;", "break;" };
				break;
			default:
				GD.Print("Invalid option.");
				break;
		}
		
		if (Input.IsActionJustPressed("B_pressed"))
		{
			GD.Print(Name);
			x++;
			if (x==5)
				x=1;
				
			text.Text = texts[x-1];
			switch (x)
			{
				case 1:
					textureRect.Texture = newTexture1;
					break;
				case 2:
					textureRect.Texture = newTexture2;
					break;
				case 3:
					textureRect.Texture = newTexture3;
					break;
				case 4:
					textureRect.Texture = newTexture4;
					break;
				default:
					Console.WriteLine("Invalid option.");
					break;
			}
			
		}
			//GD.Print("B pressed");
	}
}
