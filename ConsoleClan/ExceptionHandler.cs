using Microsoft.Extensions.Logging;

namespace ConsoleClan
{
    public static class ExceptionHandler
    {
        public static Exception Create(ILogger logger, Exception exception)
        {
            return Create(logger, default(string), exception);
        }

        private static Exception Create(ILogger logger, string? message, Exception? exception = null)
        {
            logger.LogError(message, exception);
            if (exception == null)
            {
                return new Exception(message);
            }
            return exception;
        }

        public static Exception Create(ILogger logger, Exception exception, params (string ParameterName, object? ParameterValue)[] parametersInformation)
        {
            string? message = null;
            if (parametersInformation != null)
            {
                var parameters = parametersInformation.Select(parameterInformation => $"{parameterInformation.ParameterName} = {parameterInformation.ParameterValue}").ToList();
                message = $"Parameters: {string.Join(", ", parameters)}";
            }
            return Create(logger, message, exception);
        }
    }
}
