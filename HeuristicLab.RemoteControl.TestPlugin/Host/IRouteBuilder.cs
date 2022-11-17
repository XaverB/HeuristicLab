using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grapevine;

namespace HeuristicLab.RemoteControl.TestPlugin.Host {

  /// <summary>
  /// Interface to configure a rest service.
  /// Determines routes and actions.
  /// </summary>
  public interface IRouteBuilder {
    // Get meta data
    IRoute BuildGetPropertyMetaData();
    IRoute BuildGetPossibleTypes();

    // Set / Get parameter value
    IRoute BuildGetParameterValueRoute();
    IRoute BuildPostParameterValueRoute();

    // Get execution state
    IRoute BuildGetExecuteStateRoute();

    // Post execution state
    IRoute BuildSetExecuteablePrepareRoute();
    IRoute BuildSetExecuteableStartRoute();
    IRoute BuildSetExecuteablePauseRoute();
    IRoute BuildSetExecuteableStopRoute();

    // Result

    IRoute BuildGetResultRoute();
  }
}
