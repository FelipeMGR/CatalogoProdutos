namespace APICatalogo.Logging
{
    public class CustomLogger : ILogger
    {
        readonly string LoggerName;
        readonly CustomLoggeProviderConfiguration LoggerConfig;

        public CustomLogger(string loggerName, CustomLoggeProviderConfiguration loggerConfig)
        {
            LoggerName = loggerName;
            LoggerConfig = loggerConfig;
        }

        public IDisposable? BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return LoggerConfig.LogLevel >= LogLevel.Warning;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

            EscreverNoArquivoTexto(mensagem);
        }

        private void EscreverNoArquivoTexto(string mensagem)
        {
            string caminho = @"C:\Users\felip\OneDrive\Documents\ASP.Net\teste.docx";

            using (StreamWriter sw = new(caminho, true))
            {
                try
                {
                    sw.WriteLine(mensagem);
                    sw.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
