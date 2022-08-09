using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;

namespace HeuristicLab.RemoteControl.TestPlugin {

  [Item("RESTService", "A RESTService.....")]
  [Creatable(CreatableAttribute.Categories.TestingAndAnalysis, Priority = int.MaxValue)]
  public class Restservice : ParameterizedNamedItem {
    #region Ctor/Cloner
    [StorableConstructor]
    private Restservice(StorableConstructorFlag _) : base(_) { }
    private Restservice(Restservice original, Cloner cloner)
      : base(original, cloner) { }
    public Restservice()
      : base() {

      Parameters.Add(new ValueParameter<StringValue>("Url", "Base url of the RESTService"));
      Parameters.Add(new ValueParameter<IntValue>("Port", "Port of the RESTService"));
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new Restservice(this, cloner);
    }
    #endregion
  }
}
