namespace Producer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var numbers = Enumerable.Range(0, 1_000_000).Select(x => x).ToArray();
            //var thread1 = new Thread(new ThreadStart(PrintContent));
            //var thread2 = new Thread(new ThreadStart(PrintContent));
            //thread1.Start();
            //thread2.Start();

            //using var client = new HttpClient();
            //Parallel.For(0, 10_000, _ =>
            //{
            //    Task.Delay(10);
            //    var response = client.GetAsync("https://localhost:7029/WeatherForecast").GetAwaiter().GetResult();
            //    response.EnsureSuccessStatusCode();
            //});

            Task.Run(PrintContent);
            Task.Run(PrintContent);



            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            //    await Task.Delay(100, stoppingToken);
            //}
        }

        private void PrintContent()
        {
            File.WriteAllText("test.txt", Guid.NewGuid().ToString());
            for(var i = 0; i < 1_000_000; i++)
            {
                var threadId = Thread.CurrentThread.ManagedThreadId;
                _logger.LogInformation("Worker running at: {time}, {threadId}", DateTimeOffset.Now, threadId);
            }
        }
    }
}