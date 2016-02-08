namespace ken.Spikes.Owin.KonamiCode
{
    public class KonamiCodeMiddlewareOptions
    {
        public string AssetsPath { get; set; }
        public string Action { get; set; }

        public KonamiCodeMiddlewareOptions()
        {
            AssetsPath = "/konami";
            Action = "alert('Hello konami code');";
        }
    }
}

