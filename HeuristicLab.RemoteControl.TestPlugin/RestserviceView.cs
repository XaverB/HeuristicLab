using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HeuristicLab.Common;
using HeuristicLab.Core.Views;
using HeuristicLab.MainForm;
using HeuristicLab.MainForm.WindowsForms;
using HeuristicLab.Optimization;
using HeuristicLab.PluginInfrastructure;
using HeuristicLab.PluginInfrastructure.Manager;

namespace HeuristicLab.RemoteControl.TestPlugin {
  [View("RESTService View")]
  [Content(typeof(RestService), true)]
  public partial class RestserviceView : AsynchronousContentView {

    bool algorithmAssociated = false;

    public RestserviceView() {
      InitializeComponent();
      restserviceItemView.Content = new RestService();
    }

    protected virtual void tabControl_DragEnterOver(object sender, DragEventArgs e) {
      if (algorithmAssociated)
        return;
      

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
      if (e.Effect == DragDropEffects.None)
        return;
      //if (e.Effect != DragDropEffects.None) {
      //  IProblem problem = e.Data.GetData(HeuristicLab.Common.Constants.DragDropDataFormat) as IProblem;
      //  if (e.Effect.HasFlag(DragDropEffects.Copy)) problem = (IProblem)problem.Clone();
      //  Content.Problem = problem;

      var data = e.Data;
      var data2 = e.Data.GetData(HeuristicLab.Common.Constants.DragDropDataFormat);
      var type = data.GetType();
      var type2 = data2.GetType();

      var isCloneable = data2 is IDeepCloneable;
      var algoCopy = (data2 as IDeepCloneable).Clone();
      // should we link or copy?
      var defaultView = MainFormManager.CreateDefaultView(type2);
      defaultView.Content = (IContent)algoCopy;

      (restserviceItemView.Content as RestService).Algorithm = (IContent)algoCopy;

      var page = new System.Windows.Forms.TabPage();
      var control  = new HeuristicLab.MainForm.WindowsForms.DragOverTabControl();
      // 
      // tabPage1
      // 
      page.Controls.Add(defaultView as Control);
      page.Name = algoCopy.ToString();
      page.Text = algoCopy.ToString();
      page.UseVisualStyleBackColor = true;

      tabControl.TabPages.Add(page);
      algorithmAssociated = true;
      ;
      //}
    }
  }
}
