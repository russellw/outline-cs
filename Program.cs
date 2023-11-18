var options = true;
var files = new List<string>();
foreach (var arg in args) {
	var s = arg;
	if (options) {
		if (s == "--") {
			options = false;
			continue;
		}
		if (s.StartsWith("-")) {
			if (s.StartsWith("--"))
				s = s[1..];
			switch (s) {
			case "-?":
			case "-h":
			case "-help":
				Help();
				return 0;
			case "-V":
			case "-v":
			case "-version":
				Console.WriteLine("outline-cs 1.0");
				return 0;
			default:
				Console.WriteLine("{0}: unknown option", arg);
				return 1;
			}
		}
	}
	files.Add(s);
}
if (files.Count == 0) {
	Help();
	return 0;
}

foreach (var file in files) {
	var line = 0;
	var prev = "";
	foreach (var s0 in File.ReadLines(file)) {
		line++;
		var s = s0.TrimEnd().Replace("\t", "    ");

		// All tests will ignore leading whitespace
		var t = s.TrimStart();

		// Line comment
		if (t.StartsWith("//")) {
			PrintLineNumber(line);
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(s);
			continue;
		}

		// Program should work correctly regardless of brace style
		if (t == "{" && !prev.EndsWith('{')) {
			s = prev + " {";
			prev = "";
			t = s.TrimStart();
		} else
			prev = s;

		// Skip implementation details
		if (!t.EndsWith('{'))
			continue;
		if (t.StartsWith("case "))
			continue;
		if (t.StartsWith("default:"))
			continue;
		if (t.StartsWith("for "))
			continue;
		if (t.StartsWith("foreach "))
			continue;
		if (t.StartsWith("if "))
			continue;
		if (t.StartsWith("switch "))
			continue;
		if (t.StartsWith("while "))
			continue;

		// Now the decision to print has been made
		PrintLineNumber(line);

		// Syntax color by line
		if (t.StartsWith("namespace "))
			Console.ForegroundColor = ConsoleColor.Red;
		else if (t.StartsWith("public "))
			Console.ForegroundColor = ConsoleColor.Yellow;
		else
			Console.ForegroundColor = ConsoleColor.White;

		// Print this line
		Console.WriteLine(s);
	}
}
Console.ResetColor();
return 0;

static void Help() {
	Console.WriteLine("Usage: outline-cs file");
}

static void PrintLineNumber(int line) {
	Console.ForegroundColor = ConsoleColor.Blue;
	Console.Write($"{line,6}  ");
}
