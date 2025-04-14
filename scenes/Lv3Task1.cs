using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static CodeEditing2;
using static Godot.CodeEdit;
using static System.Net.Mime.MediaTypeNames;

public partial class Lv3Task1 : CanvasLayer
{

	public bool editorShown = false;
	private Label stopwatchLabel;
	private CodeEdit codeBox;
	private TextEdit console; // Single node for both input and output
	private TextEdit consoleLine;
	private bool isConsoleLineFocused = false;
	private RichTextLabel taskDescription;
	Button submitConsoleButton;
	private List<Task> tasks = new List<Task>();
	private Button refreshButton;
	private int currentTask = 0;

	private AnimatedSprite2D door;
	private CollisionShape2D doorCollision;

	public HeroTemp heroTemp;

	public static Lv3Task1 Instance { get; private set; }

	private const string COMPILER_API = "https://godbolt.org/api/compiler/g102/compile";
	private HttpRequest request;
	// Užduoties klasė
	public class Task
	{
		public string Description { get; set; }
		public string ExpectedOutput { get; set; }
		public TaskType Type { get; set; }

		public string CodeBoxText { get; set; }

		public Task(string CodeBoxText, string description, string expectedOutput, TaskType type)
		{
			this.CodeBoxText = CodeBoxText;
			Description = description;
			ExpectedOutput = expectedOutput;
			Type = type;
		}
	}

	// Užduočių tipai
	public enum TaskType
	{
		Text,  // Užduotis, kur reikia įvesti tekstą
		Number // Užduotis, kur reikia įvesti skaičių
	}

	public override void _Ready()
	{
		PrintSceneTree(GetTree().Root);
		
		
		
		
		
		
		
		// Try to get the Submit button node and ensure it's found
		// Initialize HTTPRequest (must have the node in your scene)
		request = GetNode<HttpRequest>("HTTPRequest");
		request.RequestCompleted += OnRequestCompleted;

		door = GetNodeOrNull<AnimatedSprite2D>("../StaticBody2D3/DoorAnimatedSprite2d");
		doorCollision = GetNodeOrNull<CollisionShape2D>("../StaticBody2D3/nu");
		if (doorCollision == null)
		{
			GD.PrintErr("Door collision shape not found!");
		}
		Instance = this;
		Button submitButton = GetNodeOrNull<Button>("Submit");

		if (heroTemp == null)
		{
			heroTemp = GetNodeOrNull<HeroTemp>("res://scripts/HeroTemp.cs");
		}
		if (submitButton != null)
		{
			submitButton.Pressed += OnSubmitButtonPressed; // Connect the signal
		}
		else
		{
			GD.PrintErr("Submit button not found! Please check the node path.");
		}
		submitConsoleButton = GetNodeOrNull<Button>("PanelConsole/Submit");
		//submitConsoleButton.Pressed += OnSubmitConsoleButtonPressed;

		// Get the console node (TextEdit for both input and output)
		console = GetNodeOrNull<TextEdit>("PanelConsole/Console");
		if (console == null)
		{
			GD.PrintErr("Console node not found! Please check the node path.");
			return; // Exit early to avoid further errors
		}

		// Disable editing for the console (read-only mode)
		console.Editable = false;

		// Get the CodeEdit node
		codeBox = GetNodeOrNull<CodeEdit>("CodeEdit1");
		if (codeBox == null)
		{
			GD.PrintErr("CodeEdit node not found! Please check the node path.");
			return; // Exit early to avoid further errors
		}

		// Get the consoleLine node
		consoleLine = GetNode<TextEdit>("PanelConsole/ConsoleLine");


		taskDescription = GetNode<RichTextLabel>("TaskDescription");

		// Connect the TextChanged signal (optional, if you want real-time updates)
		//codeBox.TextChanged += OnTextChanged;

		refreshButton = GetNode<Button>("Refresh");

		refreshButton.Pressed += OnRefreshButtonPressed;

	  
		SetupSyntaxHighlighting();
		

		// Set up syntax highlighter
		CodeHighlighter cppHighlighter = new CodeHighlighter();
		// Define C++ keywords and their colors
		cppHighlighter.AddKeywordColor("int", new Color(0.8f, 0.2f, 0.2f)); // Reddish
		cppHighlighter.AddKeywordColor("float", new Color(0.8f, 0.2f, 0.2f)); // Reddish
		cppHighlighter.AddKeywordColor("double", new Color(0.8f, 0.2f, 0.2f)); // Reddish
		cppHighlighter.AddKeywordColor("char", new Color(0.8f, 0.2f, 0.2f)); // Reddish
		cppHighlighter.AddKeywordColor("bool", new Color(0.8f, 0.2f, 0.2f)); // Reddish
		cppHighlighter.AddKeywordColor("void", new Color(0.8f, 0.2f, 0.2f)); // Reddish
		cppHighlighter.AddKeywordColor("class", new Color(0.2f, 0.6f, 0.8f)); // Blueish
		cppHighlighter.AddKeywordColor("struct", new Color(0.2f, 0.6f, 0.8f)); // Blueish
		cppHighlighter.AddKeywordColor("return", new Color(0.2f, 0.8f, 0.2f)); // Greenish
		cppHighlighter.AddKeywordColor("if", new Color(0.2f, 0.8f, 0.2f)); // Greenish
		cppHighlighter.AddKeywordColor("else", new Color(0.2f, 0.8f, 0.2f)); // Greenish
		cppHighlighter.AddKeywordColor("for", new Color(0.2f, 0.8f, 0.2f)); // Greenish
		cppHighlighter.AddKeywordColor("while", new Color(0.2f, 0.8f, 0.2f)); // Greenish
		cppHighlighter.AddKeywordColor("switch", new Color(0.2f, 0.8f, 0.2f)); // Greenish
		cppHighlighter.AddKeywordColor("case", new Color(0.2f, 0.8f, 0.2f)); // Greenish
		cppHighlighter.AddKeywordColor("break", new Color(0.2f, 0.8f, 0.2f)); // Greenish
		cppHighlighter.AddKeywordColor("continue", new Color(0.2f, 0.8f, 0.2f)); // Greenish
		cppHighlighter.AddKeywordColor("public", new Color(0.8f, 0.5f, 0.2f)); // Orangeish
		cppHighlighter.AddKeywordColor("private", new Color(0.8f, 0.5f, 0.2f)); // Orangeish
		cppHighlighter.AddKeywordColor("protected", new Color(0.8f, 0.5f, 0.2f)); // Orangeish

		cppHighlighter.AddKeywordColor("cout", new Color(0.2f, 0.8f, 0.2f)); // Green for cout
		cppHighlighter.AddKeywordColor("cin", new Color(0.2f, 0.8f, 0.2f)); // Green for cin
		cppHighlighter.AddKeywordColor("endl", new Color(0.2f, 0.8f, 0.2f)); // Green for cin

		// Set colors for numbers, symbols, and functions
		cppHighlighter.NumberColor = new Color(0.0f, 0.5f, 1.0f); // Blue for numbers
		cppHighlighter.SymbolColor = new Color(0.8f, 0.8f, 0.8f); // Light gray for symbols
		cppHighlighter.FunctionColor = new Color(0.9f, 0.6f, 0.1f); // Orange for functions
		cppHighlighter.MemberVariableColor = new Color(0.6f, 0.4f, 0.8f); // Purple for member variables

		// Define color regions for comments and strings
		cppHighlighter.AddColorRegion("//", "", new Color(0.5f, 0.5f, 0.5f), true); // Light green for single-line comments
		cppHighlighter.AddColorRegion("/*", "*/", new Color(0.5f, 0.5f, 0.5f)); // Gray for multi-line comments
		cppHighlighter.AddColorRegion("\"", "\"", new Color(0.8f, 0.2f, 0.8f)); // Pink for strings
		cppHighlighter.AddColorRegion("'", "'", new Color(0.8f, 0.2f, 0.2f)); // Pink for single-character literals

		// Assign the highlighter to the CodeEdit node
		codeBox.SyntaxHighlighter = cppHighlighter;

		tasks.Add(new Task("Nėra task atrakink užduotį su brangakmeniais", "","Nan", TaskType.Text));
		tasks.Add(new Task("#include <iostream>\n\nusing namespace std;\n\nint main() {\n   int x;\ncout << \"Įveskite x reikšmę\" << endl;\ncin >> x;\ncout << x << endl;\n    return 0;\n}", "[color=orange][b]Turi sugebėti priimti vartotojo įvestį. C++ programavimo kalboje tai atliekama naudojant komandą cin, kuri leidžia nuskaityti vartotojo įvestą tekstą ar skaičių[/b] [/color]\r\n\r\n[color=#FF6347][b]*[/b][/color]*Tavo užduotis yra priimti konsole įvestį, kuri yra skaičius 7, ir ją išvesti ekrane.\r\n\r\n[color=lightblue]Paaiškinimas:\r\n\r\ncin reiškia „console input“ – tai būdas gauti duomenis iš vartotojo.\r\n\r\n\">>\" yra operatorius, kuris nurodo, kad duomenys turi būti saugomi nurodytoje kintamojoje.\r\n\r\nPvz., jei nori priimti skaičių, gali naudoti:\r\ncin >> kintamasis;\r\n\r\nKiekvieną kartą, kai naudotojas įveda reikšmę ir paspaudžia Enter mygtuką, ši reikšmė bus priskirta kintamajam.\r\nAtmink, kad turi įvesti teisingą duomenų tipą. Jei kintamasis yra tipo int, įvesk sveiką skaičių!", "7", TaskType.Number));


//*********************************************************

Task newTask = new Task(
	"#include <iostream>\n\nusing namespace std;\n\nint main() {\n    int a = 5;\n    int b = 10;\n    std::cout << a + b << std::endl;\n    return 0;\n}",
	"[color=orange][b]Sudėties užduotis[/b][/color]\n\nParašyk programą, kuri sudeda du skaičius ir išveda rezultatą.\n\n[color=lightblue]Patarimas: naudok std::cout, kad parodytum rezultatą.[/color]",
	"15",
	TaskType.Number
);

tasks.Add(newTask);

currentTask = 0;

// Atnaujinam UI
UpdateTaskDescription();
UpdateCodeBoxText();
//*********************************************************
	}

	public override void _Process(double delta)
	{
		// Check if the "Code_Editor" action is pressed (bind this in Input Map)
		if (Input.IsActionJustPressed("CodeEditor"))
		{
			// Toggle the editor visibility and the flag
			editorShown = !editorShown;
			this.Visible = editorShown;
		}
		
		var h1 = GetNode<CanvasGroup>("../Hidden1");
		var h2 = GetNode<CanvasGroup>("../Hidden2");
		var h3 = GetNode<CanvasGroup>("../Hidden3");
		var h4 = GetNode<CanvasGroup>("../Hidden4");
		var h5 = GetNode<CanvasGroup>("../Hidden5");
		var h6 = GetNode<CanvasGroup>("../Hidden6");
		
		if (!h1.Visible && !h2.Visible && !h3.Visible && !h4.Visible && !h5.Visible && !h6.Visible)
		{
			currentTask = 1;
			UpdateCodeBoxText();
			UpdateTaskDescription();
			var code = GetNode<CanvasLayer>("../CanvasLayer"); 
			code.Visible = true;
			h1.Visible=true;
		}
	}

	private void SetupSyntaxHighlighting()
	{
		// Pridėkite string delimitatorius C++ kalbai
		codeBox.AddStringDelimiter("\"", "\"", false); // Dvigubos kabutės eilutėms
		codeBox.AddStringDelimiter("'", "'", false);   // Viengubos kabutės simboliams

		// Pridėkite komentarų delimitatorius C++ kalbai

		//codeBox.AddCommentDelimiter("//", "\n", true);  // Vienos eilutės komentarai
		codeBox.AddCommentDelimiter("/*", "*/", false); // Daugiau eilutės komentarai

		// Įjungti automatinį skliaustų užbaigimą
		codeBox.AutoBraceCompletionEnabled = true;
		codeBox.SetAutoBraceCompletionPairs(new Godot.Collections.Dictionary
	{
		{ "{", "}" },
		{ "[", "]" },
		{ "(", ")" },
		{ "\"", "\"" },
		{ "'", "'" }
	});
	}
	private void OnSubmitButtonPressed()
	{
		GD.Print("Submit button pressed!");
		ExecuteCodeViaCompilerExplorer(codeBox.Text);
	}

	private void ExecuteCodeViaCompilerExplorer(string code)
	{
		var requestData = new Godot.Collections.Dictionary
	{
		{ "source", code },
		{
			"options", new Godot.Collections.Dictionary
			{
				{ "userArguments", "-O2" },
				{ "compilerOptions", new Godot.Collections.Dictionary { { "executorRequest", true } } }
			}
		}
	};

		string[] headers = { "Content-Type: application/json" };
		// aktyvuoja OnRequestCompleted
		Error err = request.Request(COMPILER_API, headers, HttpClient.Method.Post, Json.Stringify(requestData));

		if (err != Error.Ok)
		{
			AppendToConsole("[Error] Failed to connect to compiler API");
		}
	}
	private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		string responseText = body.GetStringFromUtf8();
		//HTTP status code (200 = OK, 404 = Not Found, etc.) 
		if (responseCode != 200)
		{
			AppendToConsole($"[Error] API request failed (HTTP {responseCode})");
			return;
		}

		// Extract just the output between "Standard out:" and the code
		string output = ExtractCompilerOutput(responseText);

		if (!string.IsNullOrEmpty(output))
		{
			output = output.Trim();
			AppendToConsole("[Output]\n" + output);

			if (currentTask < tasks.Count && output.Contains(tasks[currentTask].ExpectedOutput))
			{
				AppendToConsole("Task completed successfully!");
				currentTask++;
				UpdateTaskDescription();
				UpdateCodeBoxText();
			}
		}
		else
		{
			AppendToConsole("[Error] No output found in response");
			AppendToConsole($"Full response: {responseText}");
		}
	}

	private string ExtractCompilerOutput(string fullResponse)
	{
		// Look for "Standard out:" marker
		int outputStart = fullResponse.IndexOf("Standard out:");
		if (outputStart < 0) return "";

		// Find where the actual program starts
		int outputEnd = fullResponse.IndexOf("#include");
		if (outputEnd < 0) outputEnd = fullResponse.Length;

		// Extract the text between markers
		string output = fullResponse.Substring(outputStart + "Standard out:".Length,
											 outputEnd - (outputStart + "Standard out:".Length));

		// Clean up extra whitespace and empty lines
		return string.Join("\n",
			output.Split('\n')
				.Select(line => line.Trim())
				.Where(line => !string.IsNullOrEmpty(line)));
	}

	private void OnRefreshButtonPressed()
	{
		codeBox.Text = tasks[currentTask].CodeBoxText;
	}
	private void UpdateTaskDescription()
	{
		if (currentTask < tasks.Count)
		{
			taskDescription.Text = "[color=green][b]Užduotis:[/b][/color] " + tasks[currentTask].Description;
		}
		else
		{

			taskDescription.Text = "[color=green][b]Visos užduotys baigtos![/b][/color] Sveikiname!";
		var code = GetNode<CanvasLayer>("../CanvasLayer");
		code.Visible = false;
		
		
		
		var node = GetNode("../CharacterBody2D/stopwatch_label");
		var stopwatchLabel = node as StopwatchLabel;
		stopwatchLabel.Stop();
		
		}
	}
	
	
	void PrintSceneTree(Node node, string indent = "")
{
	GD.Print(indent + node.Name + " (" + node.GetType() + ")");
	foreach (Node child in node.GetChildren())
	{
		PrintSceneTree(child, indent + "  ");
	}
}

// Call this in _Ready()




	private void UpdateCodeBoxText()
	{
		if (currentTask < tasks.Count)
		{
			codeBox.Text = tasks[currentTask].CodeBoxText;
		}
	}
	private void OnTextChanged()
	{
		GD.Print("Player modified the code! Current text:\n" + codeBox.Text);
	}

	private void AppendToConsole(string text)
	{
		
		if (text == tasks[currentTask].ExpectedOutput)
		{
			UpdateCodeBoxText();
			UpdateTaskDescription();
		}
		console.Text += text + "\n";
		console.ScrollVertical = (int)console.GetLineCount() - 1;
	}

   
}
