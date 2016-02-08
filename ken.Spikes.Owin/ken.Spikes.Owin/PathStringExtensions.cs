using Microsoft.Owin;

namespace ken.Spikes.Owin
{
    public static class PathStringExtensions
    {
        public static string ToMimeType(this string path)
        {
            var mimeType = "application/unknown";
            var extension = System.IO.Path.GetExtension(path);
            if (null == extension) return "text/html";
            {
                string ext = extension.ToLower();
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
                if (regKey != null && regKey.GetValue("Content Type") != null)
                    mimeType = regKey.GetValue("Content Type").ToString();
            }
            return mimeType;
        }

        public static string ToMimeType(this PathString pathString)
        {
            return pathString.Value.ToMimeType();
        }
    }
}