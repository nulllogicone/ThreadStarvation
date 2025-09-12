using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Threading.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/hello")] // Good old sync code like in admin and apis
        public string Hello()
        {
            Thread.Sleep(2000); // DB query, REST call,
            return "sync-over-sync";
        }

        [HttpGet("/hello-async-over-sync")] // New project using old services
        public async Task<string> HelloAsyncOverSync()
        {
            await Task.Run(() => Thread.Sleep(2000));
            return "async-over-sync";
        }

        [HttpGet("/hello-sync-over-async")] // Old code using new APIs
        public string HelloSyncOverAsync()
        {
            Task.Delay(2000).Wait();
            return "sync-over-async (☠)";
        }

        [HttpGet("/hello-async")] // All new
        public async Task<string> HelloAsync()
        {
            await Task.Delay(2000);
            return "async-over-async";
        }
    }
}
