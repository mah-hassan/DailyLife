using DailyLife.Domain.Shared;

namespace DailyLife.Domain.Errors
{
    
    public class Error : IEquatable<Error>
    {
        public string Message { get; init; }

        public string Code { get; init; }
        
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

        public static implicit operator string(Error error) => error.Code;   
        public Error(string code, string message)
        {
            Message = message;
            Code = code;
        }
        public override string ToString()
        {
            return $"{Code}: {Message}.";
        }

        public bool Equals(Error? other)
        {
            if (!(other is Error error))
            {
                return false;
            }
            return other.Code == Code && other.Message == Message;
        }

        public override bool Equals(object? obj)
            => obj is Error error && Equals(error);

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + (Code != null ? Code.GetHashCode() : 0);
            hash = hash * 23 + (Message != null ? Message.GetHashCode() : 0);
            return hash;
        }
    }
}
