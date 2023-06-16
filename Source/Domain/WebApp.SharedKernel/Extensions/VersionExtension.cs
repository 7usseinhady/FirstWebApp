namespace WebApp.SharedKernel.Extensions
{
    public static class VersionExtension
    {
        public static Version IncrementMajor(this Version version)
        {
            return new Version(version.Major + 1, 0, 0, 0);
        }

        public static Version DecrementMajor(this Version version)
        {
            return new Version(version.Major - 1, 0, 0, 0);
        }


        public static Version IncrementMinor(this Version version)
        {
            return new Version(version.Major, version.Minor + 1, 0, 0);
        }

        public static Version DecrementMinor(this Version version)
        {
            return new Version(version.Major, version.Minor - 1, 0, 0);
        }


        public static Version Incrementbuild(this Version version)
        {
            return new Version(version.Major, version.Minor, version.Build + 1, 0);
        }

        public static Version Decrementbuild(this Version version)
        {
            return new Version(version.Major, version.Minor, version.Build - 1, 0);
        }


        public static Version IncrementRevision(this Version version)
        {
            return new Version(version.Major, version.Minor, version.Build, version.Revision + 1);
        }

        public static Version DecrementRevision(this Version version)
        {
            return new Version(version.Major, version.Minor, version.Build, version.Revision - 1);
        }
    }
}
