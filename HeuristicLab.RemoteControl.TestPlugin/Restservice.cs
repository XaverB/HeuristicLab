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

  [Item("RESTService", "RESTService to remote control HeuristicLab algorithms")]
  [Creatable(CreatableAttribute.Categories.TestingAndAnalysis, Priority = int.MaxValue)]
  public class RestServiceItem : ParameterizedNamedItem {
    /// <summary>
    /// The host of the REST - services. This object is used to start and control the rest - service.
    /// </summary>
    private RESTServiceHost host;
    /// <summary>
    /// Reference to a algorithm, which should be controlled by the REST - service
    /// </summary>
    public IAlgorithm Algorithm { get; set; }
    /// <summary>
    /// The problem associated with the referenced algorithm, which should be controlled by the REST - service
    /// </summary>
    public IProblem Problem => Algorithm?.Problem;

    #region Ctor/Cloner
    [StorableConstructor]
    private RestServiceItem(StorableConstructorFlag _) : base(_) { }
    private RestServiceItem(RestServiceItem original, Cloner cloner)
      : base(original, cloner) { }

    public RestServiceItem()
      : base() {

      var url = new StringValue("http://localhost");
      var port = new IntValue(1234);

      url.ValueChanged += Configuration_ValueChanged;
      port.ValueChanged += Configuration_ValueChanged;

      Parameters.Add(new ValueParameter<StringValue>("Url", "Base url of the RESTService",
        new StringValue("http://localhost")));
      Parameters.Add(new ValueParameter<IntValue>("Port", "Port of the RESTService",
        new IntValue(1234)));
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new RestServiceItem(this, cloner);
    }
    #endregion

    private void Configuration_ValueChanged(object sender, EventArgs e) {
      // restart restservice with new configuration
      // TODO improve

      try {
        ConfigureAndRunHost();
      } catch (Exception ex) {
        Console.Error.WriteLine("Unhandled exception in ValueChanged", ex);
      }
    }

    public void ConfigureAndRunHost() {
      if (host != null) {
        host.Stop();
      }

      var urlParameter = Parameters["Url"];
      var portparameter = Parameters["Port"];

      var urlValue = urlParameter.ActualValue;
      var portValue = portparameter.ActualValue;

      // maybe the restservice item should also work with HL items
      // so we don't have to do those casts here
      var url = ((HeuristicLab.Data.StringValue)urlValue).Value;
      var port = ((HeuristicLab.Data.ValueTypeValue<int>)portValue).Value;

      host = new Host.RESTServiceHost(new HostConfiguration() {
        Algorithm = Algorithm,
        Url = url,
        Port = port
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
