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
using HeuristicLab.MainForm.WindowsForms;
using HeuristicLab.PluginInfrastructure;
using HeuristicLab.PluginInfrastructure.Manager;

namespace HeuristicLab.RemoteControl.TestPlugin {
  [View("RESTService View")]
  [Content(typeof(Restservice), true)]
  public partial class RestserviceView : AsynchronousContentView {
    public RestserviceView() {
      InitializeComponent();
      restserviceItemView.Content = new Restservice();
    }

    protected virtual void tabControl_DragEnterOver(object sender, DragEventArgs e) {

      

      e.Effect = DragDropEffects.None;
      if (!ReadOnly && (e.Data.GetData(HeuristicLab.Common.Constants.DragDropDataFormat) != null) /*&& Content.ProblemType.IsAssignableFrom(e.Data.GetData(HeuristicLab.Common.Constants.DragDropDataFormat).GetType()))*/) {
        if ((e.KeyState & 32) == 32) e.Effect = DragDropEffects.Link;  // ALT key
        else if ((e.KeyState & 4) == 4) e.Effect = DragDropEffects.Move;  // SHIFT key
        else if (e.AllowedEffect.HasFlag(DragDropEffects.Copy)) e.Effect = DragDropEffects.Copy;
        else if (e.AllowedEffect.HasFlag(DragDropEffects.Move)) e.Effect = DragDropEffects.Move;
        else if (e.AllowedEffect.HasFlag(DragDropEffects.Link)) e.Effect = DragDropEffects.Link;
      
      }
    }
    protected virtual void tabControl_DragDrop(object sender, DragEventArgs e) {
      //if (e.Effect != DragDropEffects.None) {
      //  IProblem problem = e.Data.GetData(HeuristicLab.Common.Constants.DragDropDataFormat) as IProblem;
      //  if (e.Effect.HasFlag(DragDropEffects.Copy)) problem = (IProblem)problem.Clone();
      //  Content.Problem = problem;
      var defaultView = MainFormManager.CreateDefaultView(e.Data.GetData(HeuristicLab.Common.Constants.DragDropDataFormat).GetType());

      var page = new System.Windows.Forms.TabPage();
      var control  = new HeuristicLab.MainForm.WindowsForms.DragOverTabControl();
      // 
      // tabPage1
      // 
      page.Controls.Add(defaultView as Control);
      page.Name = "new";
      page.Text = "new";
      page.UseVisualStyleBackColor = true;

      tabControl.TabPages.Add(page);
      ;
      //}
    }
  }
}
