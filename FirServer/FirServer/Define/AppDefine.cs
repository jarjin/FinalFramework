using FirServer.Interface;
using Grpc.Core;
using GrpcGateway;
using System.Reflection;
using System.Runtime.Loader;

namespace FirServer.Define
{
    public class GlobalConfig
    {
        public string? _name { get; set; }
        public string? _percent { get; set; }
        public string? _takecashPoundage { get; set; }
        public float _failAmount { get; set; }
        public Dictionary<uint, GameConfigInfo> _gameConfigList = new Dictionary<uint, GameConfigInfo>();
    }

    public class GameInfoPercent
    {
        public float _baseValue;
        public float _maxValue;
    }

    public class GameConfigInfo
    {
        public uint _id { get; set; }
        public string? _name { get; set; }
        public string? _libName { get; set; }
        public string? _mainClass { get; set; }
        public string? _version { get; set; }
    }

    public class GameAssemblyLoadContext : AssemblyLoadContext
    {
        public GameAssemblyLoadContext() : base(isCollectible: true) {}
        protected override Assembly? Load(AssemblyName assemblyName) => null;
    }

    public class AssemblyInfo
    {
        public GameAssemblyLoadContext _context;
        public Assembly _assembly;

        public AssemblyInfo(GameAssemblyLoadContext context, Assembly assembly)
        {
            _context = context;
            _assembly = assembly;
        }
    }

    public class MsgChannel
    {
        public string? Name;
        public string? Token;
        public IObject? paramObject;
        public IServerStreamWriter<HelloReply>? Stream;
    }
}
