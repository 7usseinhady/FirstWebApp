
namespace WebApp.DataTransferObjects.DTOs
{
    public class SystemVersionDTO
    {
        public SystemVersionDTO()
        {
        }

        public SystemVersionDTO(int major = 0, int minor = 0, int build = 0)
        {
            this.major = major;
            this.minor = minor;
            this.build = build;
        }

        public SystemVersionDTO(System.Version version)
        {
            major = version.Major;
            minor = version.Minor;
            build = version.Build;
        }

        public int major { get; set; } = 0;
        public int minor { get; set; } = 0;
        public int build { get; set; } = 0;


        public string ToString()
        {
            return $"{major}.{minor}.{build}";
        }
    }

    
}
