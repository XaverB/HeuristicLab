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

    public IRoute BuildProblemParameterRoute() {
      return new Route(actionProvider.GetProblemParameterAction, "Get", "/getProblemParameter");
    }

    public IRoute BuildGetParameterInfoInfoRoute() {
      return new Route(actionProvider.GetParameterInfoInfo, "Get", "/problem/parameter/info");
    }

    public IRoute BuildGetParameterInfoRoute() {
      return new Route(actionProvider.GetParameterInfo, "Get", "/getParameterInfo");
    }

    public IRoute BuildGetParameterInfosRoute() {
      return new Route(actionProvider.GetParameterInfos, "Get", "/getParameterInfos");
    }

    public IRoute BuildGetPossibleParameterValuesRoute() {
      return new Route(actionProvider.GetPossibleParameterValues, "Get", "/getPossibleParameterValues");
    }

    public IRoute BuildGetProblemParameterRoute() {
      // TODO this was just a quic fix so we can compile it
      // check if this is the right route
      return new Route(actionProvider.GetProblemParameter, "Get", "/getProblemParameter");
    }

    public IRoute BuildGetResultRoute() {
      return new Route(actionProvider.GetResult, "Get", "/result");
    } 

    public IRoute BuildPostProblemParameterRoute() {
      return new Route(actionProvider.PostProblemParameter, "Post", "/setProblemParameter");
    }

    public IRoute BuildSetProblemParameterRoute() {
      return new Route(actionProvider.SetProblemParameter, "Post", "/problem/parameter");
    }

    public IRoute BuildGetExecuteStateRoute() {
      return new Route(actionProvider.GetExecutionState, "Get", "/algorithm/executionState");
    }

    public IRoute BuildSetExecuteablePrepareRoute() {
      return new Route(actionProvider.SetExecuteablePrepare, "Post", "/algorithm/prepare");
    }

    public IRoute BuildSetExecuteableStartRoute() {
      return new Route(actionProvider.SetExecuteableStart, "Post", "/algorithm/start");
    }
    public IRoute BuildSetExecuteablePauseRoute() {
      return new Route(actionProvider.SetExecuteablePause, "Post", "/algorithm/pause");
    }
    public IRoute BuildSetExecuteableStopRoute() {
      return new Route(actionProvider.SetExecuteableStop, "Post", "/algorithm/stop");
    }
  }
}
