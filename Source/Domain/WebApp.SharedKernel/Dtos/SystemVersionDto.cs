
namespace WebApp.SharedKernel.Dtos
{
    public class SystemVersionDto
    {
        

        public SystemVersionDto(int major = 0, int minor = 0, int build = 0)
        {
            this.major = major;
            this.minor = minor;
            this.build = build;
        }

        public SystemVersionDto(Version version)
        {
            major = version.Major;
            minor = version.Minor;
            build = version.Build;
        }

        public int major { get; set; } = 0;
        public int minor { get; set; } = 0;
        public int build { get; set; } = 0;


        public override string ToString() => $"{major}.{minor}.{build}";
    }

    
}
