namespace OrdersExercise.Models;

public enum LogEvents
{
    OrderCreationStarted = 2001,
    OrderValidationFailed = 2002,
    OrderCreationCompleted = 2003,
    DatabaseOperationStarted = 2004,
    DatabaseOperationCompleted = 2005,
    CacheOperationPerformed = 2006,
    ISBNValidationPerformed = 2007,
    StockValidationPerformed = 2008
}