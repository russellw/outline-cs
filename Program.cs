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
	var prev = "";
	foreach (var line0 in File.ReadLines(file)) {
		var line = line0.TrimEnd().Replace("\t", "    ");

		// All tests will ignore leading whitespace
		var s = line.TrimStart();

		// line comment
		if (s.StartsWith("//")) {
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(line);
			continue;
		}

		// Program should work correctly regardless of brace style
		if (s == "{" && !prev.EndsWith('{')) {
			line = prev + " {";
			prev = "";
			s = line.TrimStart();
		} else
			prev = line;

		// skip implementation details
		if (!s.EndsWith('{'))
			continue;
		if (s.StartsWith("case "))
			continue;
		if (s.StartsWith("default:"))
			continue;
		if (s.StartsWith("for "))
			continue;
		if (s.StartsWith("foreach "))
			continue;
		if (s.StartsWith("if "))
			continue;
		if (s.StartsWith("switch "))
			continue;
		if (s.StartsWith("while "))
			continue;

		// syntax color by line
		if (s.StartsWith("namespace "))
			Console.ForegroundColor = ConsoleColor.Red;
		else if (s.StartsWith("public "))
			Console.ForegroundColor = ConsoleColor.Yellow;
		else
			Console.ForegroundColor = ConsoleColor.White;

		// print this line
		Console.WriteLine(line);
	}
}
Console.ResetColor();
return 0;

static void Help() {
	Console.WriteLine("Usage: outline-cs file");
}
