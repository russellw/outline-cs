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
	if (files.Count > 1)
		Console.WriteLine(file);
	foreach (var line in File.ReadLines(file)) {
		var s = line.Trim();
		if (s.StartsWith("//")) {
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine(line);
			continue;
		}
	}
	if (files.Count > 1)
		Console.WriteLine();
}
Console.ResetColor();
return 0;

static void Help() {
	Console.WriteLine("Usage: outline-cs file");
}
