using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.RemoteControl.TestPlugin.Host;

namespace HeuristicLab.RemoteControl.TestPlugin {

  [Item("RESTService", "A RESTService.....")]
  [Creatable(CreatableAttribute.Categories.TestingAndAnalysis, Priority = int.MaxValue)]
  public class RestService : ParameterizedNamedItem {

    private Host.Host host;
    public IAlgorithm Algorithm { get; set; }
    public IProblem Problem => Algorithm?.Problem;

    #region Ctor/Cloner
    [StorableConstructor]
    private RestService(StorableConstructorFlag _) : base(_) { }
    private RestService(RestService original, Cloner cloner)
      : base(original, cloner) { }

    public RestService()
      : base() {

      var url = new StringValue("http://localhost");
      var port = new IntValue(1234);

      url.ValueChanged += Configuration_ValueChanged;
      port.ValueChanged += Configuration_ValueChanged;

      Parameters.Add(new ValueParameter<StringValue>("Url", "Base url of the RESTService", url));
      Parameters.Add(new ValueParameter<IntValue>("Port", "Port of the RESTService", port));
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new RestService(this, cloner);
    }
    #endregion

    private void Configuration_ValueChanged(object sender, EventArgs e) {
      // restart restservice with new configuration
      // TODO improve
      host = new Host.Host(new HostConfiguration() {
        Algorithm = Algorithm,
        Url = (Parameters["Url"] as StringValue).Value,
        Port = (Parameters["Port"] as IntValue).Value
      });
      host.Run();
    }

    public event EventHandler ValueChanged;
    protected virtual void OnValueChanged() {
      EventHandler handler = ValueChanged;
      if (handler != null) handler(this, EventArgs.Empty);
      OnItemImageChanged();
      OnToStringChanged();
    }
  }
}
