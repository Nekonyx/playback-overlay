namespace PlaybackDataServer
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            using var app = new App.App();
            app.Start();

            Console.WriteLine("Write \"stop\" or close console to stop the server.");
            while (true)
            {
                var line = Console.ReadLine();
                if (line == "stop")
                {
                    break;
                }
            }
        }
    }
}