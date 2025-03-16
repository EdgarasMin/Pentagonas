using Godot;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static CodeEditing;
using static Godot.CodeEdit;
using static System.Net.Mime.MediaTypeNames;

public partial class CodeEditing : CanvasLayer
{
    public bool editorShown = false;
   
    private CodeEdit codeBox;
    private TextEdit console; // Single node for both input and output
    private TextEdit consoleLine;
    private bool isConsoleLineFocused = false;
    private RichTextLabel taskDescription;
    Button submitConsoleButton;
    private List<Task> tasks = new List<Task>();
    private Button refreshButton;
    private int currentTask = 0;

    public HeroTemp heroTemp;

    public static CodeEditing Instance { get; private set; }
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
        // Try to get the Submit button node and ensure it's found
        Instance = this;
        Button submitButton = GetNodeOrNull<Button>("Submit");

        if (heroTemp == null)
        {
            heroTemp = GetNodeOrNull<HeroTemp>("res://scripts/HeroTemp.cs");
        }
        if (submitButton != null)
        {
            submitButton.Pressed += OnSubmitButtonPressedAsync; // Connect the signal
        }
        else
        {
            GD.PrintErr("Submit button not found! Please check the node path.");
        }
        submitConsoleButton = GetNodeOrNull<Button>("PanelConsole/Submit");
        submitConsoleButton.Pressed += OnSubmitConsoleButtonPressed;

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

        // Connect the GUI input signal to handle Enter key press
        consoleLine.GuiInput += OnConsoleLineGuiInput;

        // Set up syntax highlighting, code completion, and editor features
        SetupSyntaxHighlighting();
        //SetupCodeCompletion();
        //SetupEditorFeatures();

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

        tasks.Add(new Task("cout << \"Hello, World!\" << endl;", "[color=orange][b]Norėdamas pradėti savo kelionę požemiuose, tu turi sugebėti parašyti savo pirmąją kodo eilutę[/b][/color]\r\n\r\nC++ programavimo kalboje tai atliekama naudojant komandą cout, kuri leidžia parodyti tekstą ekrane.\r\n\r\n[color=#FF6347][b]*[/b][/color]Tavo užduotis yra pakeisti teksto išvedimą į \"Hello, Dungeon!\"\r\n\r\n[color=lightblue]Paaiškinimas:\r\n\r\ncout reiškia „console output“ – tai būdas išvesti tekstą į ekraną.\r\n\r\n\"<<\" yra kaip rodyklė, nukreipianti, ką rodyti.\r\n\r\n\"Hello, dungeon!\" – tai tavo sveikinimo žinutė.\r\n\r\n\"endl\" reiškia eilutės pabaigą – tai tiesiog perkelia tekstą į naują eilutę.[/color]", "Hello, Dungeon!", TaskType.Text));
        tasks.Add(new Task("int x;\ncout << \"Įveskite x reikšmę\" << endl;\ncin >> x;\ncout << x << endl;", "[color=orange][b]Turi sugebėti priimti vartotojo įvestį. C++ programavimo kalboje tai atliekama naudojant komandą cin, kuri leidžia nuskaityti vartotojo įvestą tekstą ar skaičių[/b] [/color]\r\n\r\n[color=#FF6347][b]*[/b][/color]*Tavo užduotis yra priimti konsole įvestį, kuri yra skaičius 7, ir ją išvesti ekrane.\r\n\r\n[color=lightblue]Paaiškinimas:\r\n\r\ncin reiškia „console input“ – tai būdas gauti duomenis iš vartotojo.\r\n\r\n\">>\" yra operatorius, kuris nurodo, kad duomenys turi būti saugomi nurodytoje kintamojoje.\r\n\r\nPvz., jei nori priimti skaičių, gali naudoti:\r\ncin >> kintamasis;\r\n\r\nKiekvieną kartą, kai naudotojas įveda reikšmę ir paspaudžia Enter mygtuką, ši reikšmė bus priskirta kintamajam.\r\nAtmink, kad turi įvesti teisingą duomenų tipą. Jei kintamasis yra tipo int, įvesk sveiką skaičių!", "7", TaskType.Number));

        UpdateTaskDescription();

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


    //private void SetupCodeCompletion()
    //{
    //    // Add C++ keywords
    //    codeBox.AddCodeCompletionOption(CodeCompletionKind.PlainText, "int", "int ");
    //    codeBox.AddCodeCompletionOption(CodeCompletionKind.PlainText, "float", "float ");
    //    codeBox.AddCodeCompletionOption(CodeCompletionKind.PlainText, "double", "double ");
    //    codeBox.AddCodeCompletionOption(CodeCompletionKind.PlainText, "char", "char ");
    //    codeBox.AddCodeCompletionOption(CodeCompletionKind.PlainText, "void", "void ");
    //    codeBox.AddCodeCompletionOption(CodeCompletionKind.PlainText, "return", "return ");

    //    // Add common functions
    //    codeBox.AddCodeCompletionOption(CodeCompletionKind.Function, "cout", "cout << ");
    //    codeBox.AddCodeCompletionOption(CodeCompletionKind.Function, "cin", "cin >> ");

    //    // Enable code completion
    //    codeBox.CodeCompletionEnabled = true;
    //}

    //private void SetupEditorFeatures()
    //{
    //    // Enable line numbers
    //    codeBox.GuttersDrawLineNumbers = true;

    //    // Enable line folding
    //    codeBox.LineFolding = true;
    //}
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
        }
    }
    private void UpdateCodeBoxText()
    {
        if (currentTask < tasks.Count)
        {
            codeBox.Text = tasks[currentTask].CodeBoxText;
        }
    }
    private void OnTextChanged()
    {
        // Optional: Use this for real-time updates (e.g., syntax validation)
        GD.Print("Player modified the code! Current text:\n" + codeBox.Text);
    }

    private void AppendToConsole(string text)
    {
        // Append text to the console and scroll to the bottom
        if(text == tasks[currentTask].ExpectedOutput)
        {
            UpdateCodeBoxText();
            UpdateTaskDescription();
        }
        console.Text += text + "\n";
        console.ScrollVertical = (int)console.GetLineCount() - 1;
    }

    private void OnCinInputSubmitted(string input, string variableName)
    {
        // Disable the console for input after submission
        console.Editable = false;

        // Process the input based on the variable type
        string typePattern = @"(int|double|float|char)\s+" + variableName;
        var typeMatch = Regex.Match(codeBox.Text, typePattern);

        if (typeMatch.Success)
        {
            string type = typeMatch.Groups[1].Value.ToString();
            AppendToConsole("[Input] You entered: " + input);

            // Update the code with the new value
            string updatedCode = codeBox.Text;
            string assignmentPattern = variableName + @"\s*=\s*[^;]+;";
            string newAssignment = variableName + " = " + input + ";";

            if (Regex.IsMatch(updatedCode, assignmentPattern))
            {
                // Replace existing assignment
                updatedCode = Regex.Replace(updatedCode, assignmentPattern, newAssignment);
            }
            else
            {
                // Add new assignment
                updatedCode += "\n" + newAssignment;
            }

            codeBox.Text = updatedCode;
        }
        else
        {
            AppendToConsole("[Error] Variable type not found for " + variableName);
        }
    }

    private async void OnSubmitButtonPressedAsync()
    {
        GD.Print("Submit button pressed!");

        string codeText = codeBox.Text;

        // Split the code into lines
        string[] lines = codeText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Dictionary to store variable values
        Dictionary<string, string> variables = new Dictionary<string, string>();

        // Process each line
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();

            // Handle cout statements
            if (line.StartsWith("cout"))
            {
                string coutPattern = @"cout\s*((?:<<\s*(?:""[^""]*""|[^;]+)\s*)+)(?:<<\s*endl\s*)?;";
                var coutMatch = Regex.Match(line, coutPattern);

                if (coutMatch.Success)
                {
                    string fullChain = coutMatch.Groups[1].Value;

                    // Extract all the quoted strings and variables inside the chain
                    var stringMatches = Regex.Matches(fullChain, "\"([^\"]*)\"");
                    var variableMatches = Regex.Matches(fullChain, @"<<\s*([a-zA-Z_][a-zA-Z0-9_]*)");

                    string finalOutput = "";

                    // Process literal strings
                    foreach (Match sm in stringMatches)
                    {
                        finalOutput += sm.Groups[1].Value;
                    }

                    // Process variables
                    foreach (Match vm in variableMatches)
                    {
                        string variableName = vm.Groups[1].Value;

                        if (variableName == "endl")
                        {
                            // Handle endl as a newline character
                            finalOutput += "\n";
                        }
                        else if (variables.ContainsKey(variableName))
                        {
                            // Use the variable's value
                            finalOutput += variables[variableName];
                        }
                        else
                        {
                            AppendToConsole($"[Error] Variable '{variableName}' has no assigned value.");
                        }
                    }

                    // Trim any extra whitespace (including newlines) from finalOutput
                    finalOutput = finalOutput.Trim();

                    // Output the result
                    AppendToConsole("[Output] " + finalOutput);

                    // Check if the output matches the expected output
                    if (finalOutput == tasks[currentTask].ExpectedOutput)
                    {
                        AppendToConsole("Užduotis ivykdyta");
                        currentTask++;
                        UpdateTaskDescription();
                        UpdateCodeBoxText();
                    }
                }
            }
            // Handle cin statements
            else if (line.StartsWith("cin"))
            {
                string cinPattern = @"cin\s*>>\s*([^;]+);";
                var cinMatch = Regex.Match(line, cinPattern);

                if (cinMatch.Success)
                {
                    string variableName = cinMatch.Groups[1].Value.ToString().Trim();

                    // Determine the variable type
                    string typePattern = $@"\b(int|float|double|char|bool|string)\s+{variableName}\b";
                    var typeMatch = Regex.Match(codeBox.Text, typePattern);

                    if (typeMatch.Success)
                    {
                        string type = typeMatch.Groups[1].Value;
                        GD.Print($"Variable '{variableName}' is of type: {type}");

                        // Prompt the player for input
                        AppendToConsole($"[Input] Please enter a value for {variableName} (type: {type}):");

                        consoleLine.Editable = true;
                        consoleLine.Clear();
                        consoleLine.GrabFocus();

                        string input = await WaitForInput();
                        AppendToConsole("[Input] You entered: " + input);

                        // Validate the input based on the variable type
                        if (ValidateInput(input, type))
                        {
                            // Store the variable's value
                            variables[variableName] = input;
                        }
                        else
                        {
                            AppendToConsole($"[Error] Invalid input for variable '{variableName}' of type '{type}'.");
                        }
                    }
                    else
                    {
                        GD.PrintErr($"[Warning] Could not determine type for variable '{variableName}'.");
                    }
                }
            }
            // Handle variable assignments
            else if (line.Contains("="))
            {
                string assignmentPattern = @"([a-zA-Z_][a-zA-Z0-9_]*)\s*=\s*([^;]+);";
                var assignmentMatch = Regex.Match(line, assignmentPattern);

                if (assignmentMatch.Success)
                {
                    string variableName = assignmentMatch.Groups[1].Value;
                    string value = assignmentMatch.Groups[2].Value;

                    // Store the variable's value
                    variables[variableName] = value;
                }
            }
        }
    }


    private bool ValidateInput(string input, string type)
    {
        switch (type)
        {
            case "int":
                return int.TryParse(input, out _);
            case "float":
                return float.TryParse(input, out _);
            case "double":
                return double.TryParse(input, out _);
            case "char":
                return input.Length == 1;
            case "bool":
                return bool.TryParse(input, out _);
            case "string":
                return true; // Any input is valid for string
            default:
                return false;
        }
    }
    private async void OnSubmitConsoleButtonPressed()
    {
        GD.Print("Submit Console button pressed!");

        // Wait for input
        string inputText = await WaitForInput();

        // Process the input text
        ProcessInputText(inputText);
    }
    private void ProcessInputText(string inputText)
    {
        // Example: Append the input text to the main console
        AppendToConsole("[Input] You entered: " + inputText);

        // Example: Handle specific commands or logic
        if (inputText == "clear")
        {
            console.Text = ""; // Clear the main console
        }
        else
        {
            // Add your custom logic here
        }
    }
    private async Task<string> WaitForInput()
    {
        // Flag to track if the button has been pressed
        bool isButtonPressed = false;

        // Connect the button's Pressed signal to set the flag
        submitConsoleButton.Pressed += () =>
        {
            isButtonPressed = true;
        };

        // Wait for the button to be pressed
        while (!isButtonPressed)
        {
            // Wait for the next frame
            await ToSignal(GetTree(), "process_frame");
        }

        // Capture the input text
        string inputText = consoleLine.Text;
        GD.Print("Buvo nuskaitytas tekstas: " + inputText);

        // Clear the consoleLine for the next input
        consoleLine.Clear();

        return inputText;
    }


    

    private void OnConsoleLineGuiInput(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            if (keyEvent.Keycode == Key.Enter)
            {
                // Prevent the default behavior of inserting a newline
                GetViewport().SetInputAsHandled();

                // Trigger input submission
                GetTree().CallGroup("input_submission", "OnInputSubmitted");
            }
        }
    }
}