using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grapevine;

namespace HeuristicLab.RemoteControl.TestPlugin.Host {
  public interface IActionProvider {
    Task PostParameterValue(IHttpContext ctx);

    Task GetParameterValue(IHttpContext ctx);
    Task GetProblemParameterAction(IHttpContext ctx);
    Task GetParameterInfoInfo(IHttpContext ctx);
    Task GetPropertyPaths(IHttpContext ctx);
    Task GetParameterInfos(IHttpContext ctx);
    Task GetPossibleParameterValues(IHttpContext ctx);
    Task GetProblemParameter(IHttpContext ctx);
    Task GetResult(IHttpContext ctx);
    Task PostProblemParameter(IHttpContext ctx);
    Task SetProblemParameter(IHttpContext ctx);
    Task SetExecuteablePrepare(IHttpContext ctx);
    Task GetExecutionState(IHttpContext ctx);
    Task SetExecuteableStart(IHttpContext ctx);
    Task SetExecuteablePause(IHttpContext ctx);
    Task SetExecuteableStop(IHttpContext ctx);
  }
}
