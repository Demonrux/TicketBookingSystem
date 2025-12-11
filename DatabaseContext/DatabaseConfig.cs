namespace DatabaseContext
{
    public class DatabaseConfig
    {
        public string ConnectionString { get; private set; }

        public DatabaseConfig(string configFilePath = "C:\\Users\\Пользователь\\source\\repos\\TicketBookingSystem\\Presentation\\files\\config.txt")
        {
            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException($"Файл конфигурации не найден: {Path.GetFullPath(configFilePath)}");
            }

            LoadFromFile(configFilePath);
        }

        private void LoadFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            string host = null;
            string port = null;
            string database = null;
            string username = null;
            string password = null;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#"))
                    continue;

                var parts = line.Split('=', 2);
                if (parts.Length != 2) continue;

                var key = parts[0].Trim().ToLower();
                var value = parts[1].Trim();

                switch (key)
                {
                    case "host": host = value; break;
                    case "port": port = value; break;
                    case "database": database = value; break;
                    case "username": username = value; break;
                    case "password": password = value; break;
                }
            }

            var missingParams = new System.Collections.Generic.List<string>();
            if (string.IsNullOrEmpty(host)) missingParams.Add("host");
            if (string.IsNullOrEmpty(port)) missingParams.Add("port");
            if (string.IsNullOrEmpty(database)) missingParams.Add("database");
            if (string.IsNullOrEmpty(username)) missingParams.Add("username");
            if (string.IsNullOrEmpty(password)) missingParams.Add("password");

            if (missingParams.Count > 0)
            {
                throw new ArgumentException(
                    $"В файле конфигурации отсутствуют параметры: {string.Join(", ", missingParams)}\n");
            }

            ConnectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";
        }
    }
}