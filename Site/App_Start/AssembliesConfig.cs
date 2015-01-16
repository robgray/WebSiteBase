using System;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;

namespace WebBase.Mvc.App_Start
{
	/// <summary>
	/// The idea with this class is to load all the WebBase assemblies.  
	/// Castle windsor can then find all the installers. Rather than having to mention them from each 
	/// assembly explicitly.
	/// </summary>
	public static class AssembliesConfig
	{
		public static Boolean LookInBinSubfolder = true;

		private static Assembly[] EnsureServerAssembliesLoaded()
		{
			DirectoryInfo directory;
			if (LookInBinSubfolder) {
				directory = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin"));
			}
			else {
				directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
			}

			// use a pattern that identify all of the .dll files that contain your services
			var files = directory.GetFiles("WebBase.*.dll", SearchOption.TopDirectoryOnly);

			return (from file in files
					select AssemblyName.GetAssemblyName(file.FullName)
						into assemblyName
						let existing = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == assemblyName.FullName)
						select existing ?? AppDomain.CurrentDomain.Load(assemblyName)).ToArray();
		}

		public static Assembly[] Configure(ILog logger)
		{
			try {
				return EnsureServerAssembliesLoaded();
			}
			catch (Exception e) {
				logger.Fatal("Could not load service assemblies.", e);
				throw;
			}
		}
	}
}