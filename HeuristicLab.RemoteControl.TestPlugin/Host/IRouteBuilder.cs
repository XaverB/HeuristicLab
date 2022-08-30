using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grapevine;

namespace HeuristicLab.RemoteControl.TestPlugin.Host {
  public interface IRouteBuilder {
    IRoute BuildProblemParameterRoute();

    IRoute BuildGetProblemParameterRoute();
    IRoute BuildGetParameterInfoRoute();
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
