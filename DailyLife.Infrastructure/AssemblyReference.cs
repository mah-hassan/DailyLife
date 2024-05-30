using System.Reflection;

namespace DailyLife.Infrastructure;

internal static class AssemblyReference
{
    internal static Assembly Assembly = typeof(AssemblyReference).Assembly;
}
