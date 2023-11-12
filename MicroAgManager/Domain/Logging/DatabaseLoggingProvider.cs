using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Domain.Logging
{
    public class DatabaseLoggingProvider : ILoggerProvider
    {
        private readonly ILoggingDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, SqlLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
        public DatabaseLoggingProvider(ILoggingDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        public ILogger CreateLogger(string categoryName)=>
              _loggers.GetOrAdd(categoryName, name => new SqlLogger(categoryName,_db,_configuration));

        public void Dispose() { _loggers.Clear(); }
    }

    internal class SqlLogger : ILogger
    {
        private readonly ILoggingDbContext _dbContext;
        private readonly string _categoryName;
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string,LogLevel> _enabledLevels = new(StringComparer.OrdinalIgnoreCase);
        public SqlLogger(string categoryName,ILoggingDbContext dbContext,IConfiguration configuration)
        {
            _dbContext = dbContext;
            _categoryName = categoryName;
            _configuration = configuration;
            var logLevels = _configuration.GetSection("Logging:LogLevel").GetChildren().Where(l=>l.Key==_categoryName);
            if(!logLevels.Any())
            {
                _enabledLevels.GetOrAdd(categoryName, Enum.Parse<LogLevel>(_configuration["Logging:LogLevel:Default"]));
            }
            foreach (var logLevel in logLevels)
            {
                _enabledLevels.GetOrAdd(categoryName,Enum.Parse<LogLevel>(logLevel.Value));
            }
            
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel)
        {
            if(_enabledLevels.TryGetValue(_categoryName,out var enabledLevel))
                return logLevel >= enabledLevel;

            return false;

        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if(IsEnabled(logLevel))
            {
                _dbContext.Logs.Add(new Entity.Log
                {
                    Level = logLevel.ToString(),
                    Message =$"{formatter(state, exception)} {exception?.StackTrace}",
                    CategoryName = _categoryName,
                    EventId = eventId.Id,
                    EventName = eventId.Name ?? string.Empty,
                    TimeStamp = DateTime.Now
                });
                Task.Run(async () =>await _dbContext.SaveChangesAsync(new()));
            }
        }
    }
}
