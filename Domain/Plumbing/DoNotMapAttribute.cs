using System;

namespace Domain.Plumbing
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class DoNotMapAttribute : Attribute
	{
	}
}
