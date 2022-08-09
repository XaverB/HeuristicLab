using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HeuristicLab.Core.Views;
using HeuristicLab.MainForm;

namespace HeuristicLab.RemoteControl.TestPlugin {
  [View("DebugEngine View")]
  [Content(typeof(Restservice), true)]
  public partial class RestserviceView : ItemView {
    public RestserviceView() {
      InitializeComponent();
    }
  }
}
