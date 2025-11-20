using Microsoft.Extensions.Logging;
using OrdersExercise.Models;
using System;

namespace OrdersExercise.Logging;

public static partial class LoggingOrderCreationMetrics
{
    [LoggerMessage(
        EventId = 101,
        Level = LogLevel.Information,
        Message = "Order creation metrics: Title='{OrderTitle}', ISBN='{ISBN}', Category='{Category}'. " +
                  "Timings(ms): Validation={ValidationDurationMs}, DB Save={DatabaseSaveDurationMs}, Total={TotalDurationMs}. " +
                  "Success={Success}. Error='{ErrorReason}'")]
    public static partial void LogOrderCreationMetricsRaw(
        this ILogger logger,
        string orderTitle,
        string isbn,
        OrderCategory category,
        double validationDurationMs,
        double databaseSaveDurationMs,
        double totalDurationMs,
        bool success,
        string? errorReason);

    public static void LogOrderCreationMetrics(this ILogger logger, OrderCreationMetrics metrics)
    {
        LogOrderCreationMetricsRaw(
            logger,
            metrics.OrderTitle,
            metrics.ISBN,
            metrics.Category,
            metrics.ValidationDuration.TotalMilliseconds,
            metrics.DatabaseSaveDuration.TotalMilliseconds,
            metrics.TotalDuration.TotalMilliseconds,
            metrics.Success,
            metrics.ErrorReason);
    }
}