using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grapevine;

namespace HeuristicLab.RemoteControl.TestPlugin.Host {
  public interface IRouteBuilder {
    Route BuildProblemParameterRoute();

    Route BuildGetProblemParameterRoute();
    Route BuildGetParameterInfoRoute();
    Route BuildGetParameterInfosRoute();
    Route BuildGetPossibleParameterValuesRoute();
    Route BuildSetProblemParameterRoute();
    Route BuildGetParameterInfoInfoRoute();
    Route BuildPostProblemParameterRoute();
    Route BuildGetResultRoute();

  }
}
