namespace ken.Spikes.Owin.Cornify
{
    public class CornifyMiddlewareOptions
    {
        public string CornifyAssetsPath { get; set; }
        public int AddDelayInMilliseconds { get; set; }

        public CornifyMiddlewareOptions()
        {
            //CornifyAssetsPath = "http://www.cornify.com/js/";
            CornifyAssetsPath = "/cornify";
            AddDelayInMilliseconds = 300;
        }
    }
}

