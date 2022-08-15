using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
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
  public class RouteBuilder : IRouteBuilder {

    private readonly IActionProvider actionProvider;

    public RouteBuilder(IActionProvider actionProvider) {
      this.actionProvider = actionProvider;
    }

    public Route BuildProblemParameterRoute() {
      return new Route(actionProvider.GetProblemParameterAction, "Get", "/getProblemParameter");
    }

    public Route BuildGetParameterInfoInfoRoute() {
      return new Route(actionProvider.GetParameterInfoInfo, "Get", "/problem/parameter/info");
    }

    public Route BuildGetParameterInfoRoute() {
      return new Route(actionProvider.GetParameterInfo, "Get", "/getParameterInfo");
    }

    public Route BuildGetParameterInfosRoute() {
      return new Route(actionProvider.GetParameterInfos, "Get", "/getParameterInfos");
    }

    public Route BuildGetPossibleParameterValuesRoute() {
      return new Route(actionProvider.GetPossibleParameterValues, "Get", "/getPossibleParameterValues");
    }

    public Route BuildGetProblemParameterRoute() {
      throw new NotImplementedException();
    }

    public Route BuildGetResultRoute() {
      return new Route(actionProvider.GetResult, "Get", "/result");
    }

    public Route BuildPostProblemParameterRoute() {
      return new Route(actionProvider.PostProblemParameter, "Post", "/setProblemParameter");
    }

    public Route BuildSetProblemParameterRoute() {
      return new Route(actionProvider.SetProblemParameter, "Post", "/problem/parameter");
    }
  }
}
