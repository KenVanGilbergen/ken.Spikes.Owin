using System;
using System.IO;

namespace ken.Spikes.Owin.ServeDirectory
{
    public class ServeDirectoryMiddlewareOptions
    {
        public bool UseDefaultIndex { get; set; }

        private string _rootDirectory;
        public string RootDirectory
        {
            get { return _rootDirectory; }
            set
            {
                if (Directory.Exists(value)) _rootDirectory = value;
                else
                {
                    var fap = DirectoryFullPathFromAppDomain(value);
                    if (Directory.Exists(fap)) _rootDirectory = fap;
                    else throw new ArgumentException("RootDirectory");
                }
            }
        }

        public ServeDirectoryMiddlewareOptions()
        {
            UseDefaultIndex = true;
            RootDirectory = "_site";
        }

        private string DirectoryFullPathFromAppDomain(string root)
        {
            var applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var fullRoot = Path.GetFullPath(Path.Combine(applicationBase, root));
            if (!fullRoot.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
            {
                fullRoot += Path.DirectorySeparatorChar;
            }
            return fullRoot;
        }
    }
}

