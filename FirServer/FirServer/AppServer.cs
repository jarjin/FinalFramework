using FirServer.Common;
using FirServer.Define;
using log4net;
using log4net.Config;

namespace FirServer
{
    public class AppServer : BaseBehaviour
    {
        private static ILog? logger = null;

        private WebApplication? _app;

        public void Initialize()
        {
            AppConst.LogRepos = LogManager.CreateRepository(AppConst.RepositoryName);
            XmlConfigurator.Configure(AppConst.LogRepos, new FileInfo(AppConst.Log4jConfig));
            logger = LogManager.GetLogger(AppConst.LogRepos.Name, typeof(AppServer));

            ManagementCenter.Initialize();

            logger?.Warn("AppServer Initialized!!");
        }

        public void StartServer(string[] args) 
        {
            var builder = WebApplication.CreateBuilder(args);

            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

            // Add services to the container.
            builder.Services.AddGrpc();

            _app = builder.Build();

            // Configure the HTTP request pipeline.
            ManagementCenter.RegRpcService(_app);

            _app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            _app.Run();

            logger?.Warn("AppServer Started!!");
        }

        public void OnUpdate()
        {
            ManagementCenter.OnUpdate();
        }

        public void RestartServer() { }

        public void StopServer() 
        {
            _app?.WaitForShutdown();
            logger?.Warn("AppServer Stoped!!");
        }
    }
}
