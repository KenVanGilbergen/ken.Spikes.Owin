namespace ken.Spikes.Owin.HtmlAppender
{
    public class HtmlAppenderMiddlewareOptions
    {
        public string Head { get; set; }
        public bool AppendToHead { get; set; }
        public string Body { get; set; }
        public bool AppendToBody { get; set; }

        public HtmlAppenderMiddlewareOptions()
        {
            Head = "<!-- HtmlAppender Head Insert -->";
            AppendToHead = true;
            Body = "<!-- HtmlAppender Body Insert -->";
            AppendToBody = true;
        }
    }
}

