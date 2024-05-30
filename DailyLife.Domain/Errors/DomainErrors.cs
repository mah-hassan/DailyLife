using DailyLife.Domain.Entities;
using DailyLife.Domain.Primitives;

namespace DailyLife.Domain.Errors
{
    public static class DomainErrors
    {
        public static class BusinessErrors
        {
            public static Error Forbidden =>
                new("User", "You are not allowed to access this resource.");
            public static Error OwnerExsists => new Error("Owner",
                        "Owner already have an existing business");

            public static Error NameIsTaken(string name) =>
                new Error(name,
                        $"'{name}' is in use please try anthor name");

            public static Error NotExsist(Guid id) =>
                new Error("Business.NotFound",
                    $"Business with Id: {id} was not found");
        }

        public static class CategoryErrors
        {
            public static Error NotExsist(Guid id) => new Error("Category.NotFound",
            $"Category with Id {id} was not found");
            public static Error NameExsists(string name) =>
                new Error(name,
                $"Category '{name}' already exsists");

        }
        internal static class Id
        {
            
            internal static Error NotEmptyOrNull = new("ID.NotEmptyOrNull", "Id mus not be empty or null");
        }       
        internal static class DateOfBirth
        {
            internal static Error InvalidDate =
                new (nameof(DateOfBirth)+".Invalid Date: "
                , $"value must be 18 Year in the past");

        }  
        internal static class FullName
        {
            internal static Error NotEmptyOrNull = new("FullName.NotEmptyOrNull", "FullName mus not be empty or null");
            internal static Error InvalidValue = new("FullName.InvalidValue", "Only Arabic, English and white space are allawed");
            internal static Error InvalidLength = new("FullName.InvalidLength", "FullNmae length must be less than 100");

        }
    }
}
