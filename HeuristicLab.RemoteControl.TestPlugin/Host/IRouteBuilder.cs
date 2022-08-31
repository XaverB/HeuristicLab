using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grapevine;

namespace HeuristicLab.RemoteControl.TestPlugin.Host {
  public interface IRouteBuilder {
    IRoute BuildGetParameterValueRoute();
    IRoute BuildProblemParameterRoute();
    IRoute BuildGetProblemParameterRoute();
    IRoute BuildGetPropertyPaths();
    IRoute BuildGetParameterInfosRoute();
    IRoute BuildGetPossibleParameterValuesRoute();
    IRoute BuildSetProblemParameterRoute();
    IRoute BuildGetParameterInfoInfoRoute();
    IRoute BuildPostProblemParameterRoute();
    IRoute BuildGetResultRoute();
    IRoute BuildGetExecuteStateRoute();
    IRoute BuildSetExecuteablePrepareRoute();
    IRoute BuildSetExecuteableStartRoute();
    IRoute BuildSetExecuteablePauseRoute();
    IRoute BuildSetExecuteableStopRoute();
  }
}
