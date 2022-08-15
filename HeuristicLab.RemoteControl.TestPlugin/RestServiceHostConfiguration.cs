using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeuristicLab.Optimization;

namespace HeuristicLab.RemoteControl.TestPlugin {
  public class HostConfiguration {
    public string Url { get; set; }
    public int Port { get; set; }
    public IAlgorithm Algorithm { get; set; }
    public string UrlWithPort => $"{Url}:{Port}/";
  }
}
