namespace ken.Spikes.Owin.Cornify
{
    public class CornifyMiddlewareOptions
    {
        public string AssetsPath { get; set; }
        public int AddDelayInMilliseconds { get; set; }
        public bool Autostart { get; set; }

        public CornifyMiddlewareOptions()
        {
            //AssetsPath = "http://www.cornify.com/js/";
            AssetsPath = "/cornify";
            AddDelayInMilliseconds = 300;
            Autostart = true;
        }
    }
}

