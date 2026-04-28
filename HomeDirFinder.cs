using System.Runtime.InteropServices;

namespace SchoolProject;

public static class HomeDirFinder
{
    public static string Find()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return Environment.GetEnvironmentVariable("USERPROFILE")
                ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        return Environment.GetEnvironmentVariable("HOME")
            ?? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    }
}
