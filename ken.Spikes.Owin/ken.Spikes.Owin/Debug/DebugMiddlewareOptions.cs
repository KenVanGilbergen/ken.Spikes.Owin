using System;
using Microsoft.Owin;

// ReSharper disable once CheckNamespace
namespace Owin
{
    public class DebugMiddlewareOptions
    {
        public Action<IOwinContext> OnIncomingRequest { get; set; }
        public Action<IOwinContext> OnOutgoingRequest { get; set; }
    }
}