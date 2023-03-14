namespace ProjectPlanner.DatabaseUpdater.Services
{
    public class ArgumentService
    {
        public static ArgumentService Instance { get; private set; }

        private readonly string[] _args;
        private readonly Dictionary<string, string> _arguments = new();

        public string Database { get; private set; }
        public string ConnectionString { get; private set; }
        public string ScriptPath { get; set; }

        public ArgumentService(string[] args)
        {
            _args = args;

            Console.WriteLine(string.Join('_', args));

            ResolveArguments();
            SetArgumentValues();
        }

        private void SetArgumentValues()
        {
            Database = GetArgument("Database", "D");
            ConnectionString = GetArgument("ConnectionString", "C");
            ScriptPath = GetArgument("ScriptPath", "S");
        }

        private void ResolveArguments()
        {
            if (_args.Length == 0)
            {
                return;
            }

            foreach (var arg in _args)
            {
                var argument = arg.StartsWith("-") ? arg.Replace("-", "") : arg;

                var i = argument.IndexOf('=');
                if (i != -1)
                {
                    var option = argument.Substring(0, i);
                    var value = argument.Substring(i + 1).Replace("\"", "");

                    _arguments.Add(option, value);
                }
                else
                {
                    _arguments.Add(argument, "true");
                }
            }
        }

        private string GetArgument(params string[] options)
        {
            foreach (var option in options)
            {
                if (_arguments.TryGetValue(option, out var value))
                {
                    return value;
                }
            }

            return string.Empty;
        }

        public static void Initialize(string[] args)
        {
            Instance = new ArgumentService(args);
        }
    }
}
