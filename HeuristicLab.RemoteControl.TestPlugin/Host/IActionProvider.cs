using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grapevine;

namespace HeuristicLab.RemoteControl.TestPlugin.Host {

  /// <summary>
  /// Interface to provide concrete actions to rest service routes.
  /// </summary>
  public interface IActionProvider {
    // Get meta data
    Task GetPropertyMetaData(IHttpContext ctx);
    Task GetPossibleTypes(IHttpContext ctx);

    // Set / Get parameter value
    Task PostParameterValue(IHttpContext ctx);
    Task GetParameterValue(IHttpContext ctx);

    // get execution state
    Task GetExecutionState(IHttpContext ctx);

    // Post execution state
    Task SetExecuteablePrepare(IHttpContext ctx);
    Task SetExecuteableStart(IHttpContext ctx);
    Task SetExecuteablePause(IHttpContext ctx);
    Task SetExecuteableStop(IHttpContext ctx);

    Task GetResult(IHttpContext ctx);
  }
}
