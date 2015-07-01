using Nancy;

namespace EndersJson.Tests.Framework
{
    public class WebApiModule : NancyModule
    {
        public WebApiModule()
        {
            Get["/"] = _ => "hello nancy";
        }
    }
}