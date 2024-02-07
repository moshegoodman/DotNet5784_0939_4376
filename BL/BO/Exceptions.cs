

namespace BO;

public class InCorrectData : Exception
{
    public InCorrectData(string? message) : base(message) { }
}

public class DependentScheduleNotInitialized : Exception
{
    public DependentScheduleNotInitialized(string? message) : base(message) { }
}
public class BlEngineerIsAlreadyOccupied : Exception
{
    public BlEngineerIsAlreadyOccupied(string? message) : base(message) { }
}

public class BlAlreadyExistsException : Exception
{
    public BlAlreadyExistsException(string? message) : base(message) { }
    public BlAlreadyExistsException(string message, Exception innerException)
                : base(message, innerException) { }

}

public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }

}

public class BlDeletionImpossible : Exception
{
    public BlDeletionImpossible(string? message) : base(message) { }
}

public class BlUpdateImpossible : Exception
{
    public BlUpdateImpossible(string? message) : base(message) { }
}

public class BlScheduled : Exception
{
    public BlScheduled(string? message) : base(message) { }
}

public class BlUnScheduled : Exception
{
    public BlUnScheduled(string? message) : base(message) { }
}

