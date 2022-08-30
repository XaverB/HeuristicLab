using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Grapevine;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.PluginInfrastructure;
using HeuristicLab.RemoteControl.TestPlugin.Util;

namespace HeuristicLab.RemoteControl.TestPlugin.Host {
  public class Host {

    private readonly HostConfiguration hostConfiguration;
    private IRestServer server;
    private IProblem Problem => hostConfiguration.Algorithm?.Problem;
    private IAlgorithm Algorithm => hostConfiguration.Algorithm;
    private string Url => hostConfiguration.UrlWithPort;
    private readonly IRouteBuilder handler;

    public Host(HostConfiguration configuration) {
      hostConfiguration = configuration;

      handler = new RouteBuilder(
        new ActionProvider(
          Problem, 
          Algorithm
          )
        );

      Initialize();
    }

    private void Initialize() {
      server = RestServerBuilder.UseDefaults().Build();

      server.Prefixes.Add(Url);

      server.AfterStarting += (s) => {
        RegisterRoutes(s);
      };

      server.AfterStopping += (e) => { Console.WriteLine("=== === Server stopped === ==="); };
      Console.WriteLine($"* Server listening on {string.Join(", ", server.Prefixes)}{Environment.NewLine}");
    }

    private void RegisterRoutes(IRestServer s) {
      IEnumerable<MethodInfo> methods = GetRouteMethods();
      foreach (var method in methods) {
        IRoute route = (IRoute)method.Invoke(handler, null);
        s.Router.Register(route);
      }
    }

    private IEnumerable<MethodInfo> GetRouteMethods() {
      // add all Routes to the Service
      // we determine the Route Methods by their ReturnType, which must be IRoute
      return handler.GetType().GetMethods().Where(m => m.IsPublic && m.ReturnType == typeof(IRoute));
    }

    public void Run() {
      server.Start();
    }

    public void Stop() {
      server.Stop();
      server.Dispose();
    }

  }
}
