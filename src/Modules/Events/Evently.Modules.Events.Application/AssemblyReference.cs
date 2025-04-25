using System.Reflection;

namespace Evently.Modules.Events.Application;

public static class AssemblyReference
{
	public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;

	public static Assembly Assembly2 => typeof(AssemblyReference).Assembly;
}