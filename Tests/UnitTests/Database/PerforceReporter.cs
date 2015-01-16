using System;
using System.Linq;
using ApprovalTests.Reporters;
using ApprovalUtilities.Utilities;

namespace UnitTests.Database
{
    [Obsolete]
    public class PerforceReporter : GenericDiffReporter
    {
        private static string GetPath()
        {
            var basePath = DotNet4Utilities
                .GetProgramFilesPaths()
                .FirstOrDefault(x => !x.Contains("x86"));
            return basePath + @"\Perforce\p4merge.exe";
        }

        private static readonly string path = GetPath();
        public static readonly PerforceReporter Instance = new PerforceReporter();

        static PerforceReporter() { }

        public PerforceReporter()
            : base(path, "\"{0}\" \"{1}\"", "Could not find P4Merge at {0}, please install it".FormatWith(new object[1]
            {
                path
            }), TEXT_FILE_TYPES.Concat(TortoiseImageDiffReporter.IMAGE_FILE_TYPES).ToArray()) { }
    }
}