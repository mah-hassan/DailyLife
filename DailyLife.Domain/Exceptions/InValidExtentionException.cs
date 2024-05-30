namespace DailyLife.Domain.Exceptions;

public class InValidExtentionException : Exception
{

    public InValidExtentionException(string extention)

       : base($"Unsupported Media Type {extention}")
    {

    }
}
