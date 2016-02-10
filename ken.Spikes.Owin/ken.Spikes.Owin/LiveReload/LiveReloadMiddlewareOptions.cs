namespace ken.Spikes.Owin.LiveReload
{
    public class LiveReloadMiddlewareOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public LiveReloadMiddlewareOptions()
        {
            Host = "localhost";
            Port = 35729;
        }
    }
}

