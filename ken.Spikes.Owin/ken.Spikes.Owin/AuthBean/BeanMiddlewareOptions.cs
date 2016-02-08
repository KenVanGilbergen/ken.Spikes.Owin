using System;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security.WsFederation;

namespace ken.Spikes.Owin.AuthBean
{
    public class BeanMiddlewareOptions : WsFederationAuthenticationOptions
    {
        public string BeanUrl { get; set; }
        public string SignoutUrl
        {
            get
            {
                return String.Format("{0}?wa=wsignout1.0&wreply={1}", BeanUrl, Wreply);
            }
        }
        public string CleanupUrl
        {
            get
            {
                return String.Format("{0}?wa=wsignoutcleanup1.0", BeanUrl);
            }
        }

        public BeanMiddlewareOptions()
        {
            //var force = "&amp;forceenv=tst";
            var force = String.Empty;

            BeanUrl = "https://beanservice-tst.euroconsumers.org";
            //BeanUrl = "https://login.test-aankoop.be";

            Wtrealm = "eur://euroconsumers.dev.cdn.it-it";
            
            Wreply = "http://localhost:22222/";

            MetadataAddress = String.Format("{0}/federationmetadata/2007-06/federationmetadata.xml?wtrealm={1}{2}", BeanUrl, Wtrealm, force);
            
            TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = Wtrealm + "/"
                };
        }
    }
}
