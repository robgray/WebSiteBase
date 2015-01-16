using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ApprovalTests.Reporters;
using Castle.Core.Internal;
using NUnit.Framework;

[assembly: UseReporter(typeof(DiffReporter))]

namespace Mvc.Tests.Conventions
{
	[TestFixture]
	public class ConventionTestsRunner
	{
		[TestFixtureSetUp]
		public static void GlobalSetUp()
		{
		}

		public TestCaseData[] Conventions
		{
			get
			{
				var types = GetConventionTypes();
				var conventionTests = Array.ConvertAll(types, BuildTestData);
				return conventionTests;
			}
		}

		private TestCaseData BuildTestData(Type t)
		{
			var convention = CreateConvention(t);
			return new TestCaseData(convention).SetName(convention.Name);
		}

		private static IConventionTest CreateConvention(Type t)
		{
			return (IConventionTest)Activator.CreateInstance(t);
		}

		private static Type[] GetConventionTypes()
		{
			var types =
				Assembly.GetExecutingAssembly().GetExportedTypes().Where(
					IsConventionTest).ToArray();
			return types;
		}

		private static bool IsConventionTest(Type type)
		{
			return type.IsClass && type.IsAbstract == false && typeof(IConventionTest).IsAssignableFrom(type) &&
				   type.HasAttribute<IgnoreAttribute>() == false;
		}

		[Test]
		[TestCaseSource("Conventions")]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Run(IConventionTest test)
		{
			test.Execute();
		}
	}
}
