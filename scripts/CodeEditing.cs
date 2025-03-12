using Godot;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Godot.CodeEdit;

public partial class CodeEditing : CanvasLayer
{
    bool editorShown = false;

    private CodeEdit codeBox;
    private TextEdit console; // Single node for both input and output
    private TextEdit consoleLine;
    private bool isConsoleLineFocused = false;
    Button submitConsoleButton;

    public override void _Ready()
    {
        // Try to get the Submit button node and ensure it's found
        Button submitButton = GetNodeOrNull<Button>("Submit");

       
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

        // Connect the TextChanged signal (optional, if you want real-time updates)
        codeBox.TextChanged += OnTextChanged;

        // Connect focus events
        consoleLine.FocusEntered += _on_FocusEntered;
        consoleLine.FocusExited += _on_FocusExited;

        // Connect the GUI input signal to handle Enter key press
        consoleLine.GuiInput += OnConsoleLineGuiInput;

        // Set up syntax highlighting, code completion, and editor features
        SetupSyntaxHighlighting();
        SetupCodeCompletion();
        SetupEditorFeatures();

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
        // Add string delimiters for C++
        codeBox.AddStringDelimiter("\"", "\"", false); // Double quotes for strings
        codeBox.AddStringDelimiter("'", "'", false);   // Single quotes for characters

        // Add comment delimiters for C++
        codeBox.AddCommentDelimiter("//", "\n", true); // Single-line comments
        codeBox.AddCommentDelimiter("/*", "*/", false); // Multi-line comments

        // Enable auto brace completion
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

    private void SetupCodeCompletion()
    {
        // Add C++ keywords
        codeBox.AddCodeCompletionOption(CodeCompletionKind.PlainText, "int", "int ");
        codeBox.AddCodeCompletionOption(CodeCompletionKind.PlainText, "float", "float ");
        codeBox.AddCodeCompletionOption(CodeCompletionKind.PlainText, "double", "double ");
        codeBox.AddCodeCompletionOption(CodeCompletionKind.PlainText, "char", "char ");
        codeBox.AddCodeCompletionOption(CodeCompletionKind.PlainText, "void", "void ");
        codeBox.AddCodeCompletionOption(CodeCompletionKind.PlainText, "return", "return ");

        // Add common functions
        codeBox.AddCodeCompletionOption(CodeCompletionKind.Function, "cout", "cout << ");
        codeBox.AddCodeCompletionOption(CodeCompletionKind.Function, "cin", "cin >> ");

        // Enable code completion
        codeBox.CodeCompletionEnabled = true;
    }

    private void SetupEditorFeatures()
    {
        // Enable line numbers
        codeBox.GuttersDrawLineNumbers = true;

        // Enable line folding
        codeBox.LineFolding = true;
    }

    private void OnTextChanged()
    {
        // Optional: Use this for real-time updates (e.g., syntax validation)
        GD.Print("Player modified the code! Current text:\n" + codeBox.Text);
    }

    private void AppendToConsole(string text)
    {
        // Append text to the console and scroll to the bottom
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

        // Handle cout statements
        string coutPattern = @"cout\s*((?:<<\s*""[^""]*""\s*)+)(?:<<\s*endl\s*)?;";
        var coutMatches = Regex.Matches(codeText, coutPattern);
        GD.Print("Found " + coutMatches.Count + " cout statements.");

        foreach (Match match in coutMatches)
        {
            if (match.Groups.Count > 1)
            {
                string fullChain = match.Groups[1].Value;

                // Extract all the quoted strings inside the chain
                var stringMatches = Regex.Matches(fullChain, "\"([^\"]*)\"");
                string finalOutput = "";

                foreach (Match sm in stringMatches)
                {
                    finalOutput += sm.Groups[1].Value;
                }

                AppendToConsole("[Output] " + finalOutput);
            }
        }

        // Handle cin statements
        string cinPattern = @"cin\s*>>\s*([^;]+);";
        var cinMatches = Regex.Matches(codeText, cinPattern);
        GD.Print("Found " + cinMatches.Count + " cin statements.");
        foreach (Match match in cinMatches)
        {
            if (match.Groups.Count > 1)
            {
                string variableName = match.Groups[1].Value.ToString().Trim();
                AppendToConsole("[Input] Please enter a value for " + variableName + ":");

                // Enable the consoleLine for input
                consoleLine.Editable = true;
                consoleLine.Clear(); // Clear previous input
                consoleLine.GrabFocus(); // Focus the consoleLine

                // Wait for input
                string input = await WaitForInput();
                AppendToConsole("[Input] You entered: " + input);

                // Process the input
                OnCinInputSubmitted(input, variableName);
            }
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


    private void _on_FocusEntered()
    {
        GD.Print("ConsoleLine has gained focus!");
        isConsoleLineFocused = true;
    }

    private void _on_FocusExited()
    {
        GD.Print("ConsoleLine has exited focus!");
        isConsoleLineFocused = false;
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