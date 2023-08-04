namespace webapi.ProjectSearch.Services
{
    public class LoggerProvider
    {
        private static ILoggerFactory loggerFactory;

        public static ILoggerFactory GetLoggerFactory()
        {
            if (loggerFactory == null)
            {
                loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Information);
                    builder.AddDebug();
                });
            }
            return loggerFactory;
        }


    }
}
