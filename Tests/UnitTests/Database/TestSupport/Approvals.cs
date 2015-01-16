using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using ApprovalTests.Reporters;
using ApprovalUtilities.Utilities;

namespace UnitTests.Database.TestSupport
{
    public class DiffReporterWithCustomOrder : FirstWorkingReporter
    {
        public static readonly DiffReporterWithCustomOrder INSTANCE = new DiffReporterWithCustomOrder();

        public DiffReporterWithCustomOrder()
            : base(
				BeyondCompareReporter.INSTANCE,
                VisualStudioReporter.INSTANCE,
                CodeCompareReporter.INSTANCE,
                P4MergeCompareReporter.INSTANCE,                
                TortoiseDiffReporter.INSTANCE,
                WinMergeReporter.INSTANCE,
                KDiffReporter.INSTANCE,
                FrameworkAssertReporter.INSTANCE,
                QuietReporter.INSTANCE) { }

        public static void Launch(LaunchArgs launchArgs)
        {
            try
            {
                Process.Start(launchArgs.Program, launchArgs.Arguments);
            }
            catch (Win32Exception ex)
            {
                throw new Exception("Unable to launch: {0} with arguments {1}\nError Message: {2}".FormatWith(launchArgs.Program, launchArgs.Arguments, ex.Message), ex);
            }
        }
    }

    public class P4MergeCompareReporter : GenericDiffReporter
    {
        private static readonly string PATH = @"c:\Program Files\Perforce\p4merge.exe";
        public static readonly P4MergeCompareReporter INSTANCE = new P4MergeCompareReporter();

        public P4MergeCompareReporter() :
            base(
                 PATH, "\"{0}\" \"{1}\"",
                 "Could not find P4Merge at {0}, please install it".FormatWith(PATH),
                 TEXT_FILE_TYPES.Concat(TortoiseImageDiffReporter.IMAGE_FILE_TYPES).ToArray()) { }
    }
}
