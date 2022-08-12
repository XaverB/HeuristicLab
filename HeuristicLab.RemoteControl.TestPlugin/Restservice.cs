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

      var url = new StringValue("http://localhost");
      var port = new IntValue(1234);

      url.ValueChanged += Configuration_ValueChanged;
      port.ValueChanged += Configuration_ValueChanged;

      Parameters.Add(new ValueParameter<StringValue>("Url", "Base url of the RESTService", url));
      Parameters.Add(new ValueParameter<IntValue>("Port", "Port of the RESTService", port));
    }

    private void Configuration_ValueChanged(object sender, EventArgs e) {
      // restart restservice with new configuration
    }

    public event EventHandler ValueChanged;
    protected virtual void OnValueChanged() {
      EventHandler handler = ValueChanged;
      if (handler != null) handler(this, EventArgs.Empty);
      OnItemImageChanged();
      OnToStringChanged();
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new Restservice(this, cloner);
    }
    #endregion
  }
}
