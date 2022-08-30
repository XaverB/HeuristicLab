namespace HeuristicLab.RemoteControl.TestPlugin {
  partial class RestserviceView {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.restserviceItemView = new HeuristicLab.Core.Views.ParameterizedNamedItemView();
            this.tabControl = new HeuristicLab.MainForm.WindowsForms.DragOverTabControl();
            this.tabPage1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.restserviceItemView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 424);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "RESTService";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // restserviceItemView
            // 
            this.restserviceItemView.Caption = "ParameterizedNamedItem View";
            this.restserviceItemView.Content = null;
            this.restserviceItemView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.restserviceItemView.Location = new System.Drawing.Point(3, 3);
            this.restserviceItemView.Name = "restserviceItemView";
            this.restserviceItemView.ReadOnly = false;
            this.restserviceItemView.Size = new System.Drawing.Size(786, 418);
            this.restserviceItemView.TabIndex = 0;
            this.restserviceItemView.Load += new System.EventHandler(this.restserviceItemView_Load);
            // 
            // tabControl
            // 
            this.tabControl.AllowDrop = true;
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 450);
            this.tabControl.TabIndex = 0;
            this.tabControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl_DragDrop);
            this.tabControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabControl_DragEnterOver);
            this.tabControl.DragOver += new System.Windows.Forms.DragEventHandler(this.tabControl_DragEnterOver);
            // 
            // RestserviceView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "RestserviceView";
            this.Size = new System.Drawing.Size(800, 450);
            this.tabPage1.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabPage tabPage1;
    private MainForm.WindowsForms.DragOverTabControl tabControl;
    private Core.Views.ParameterizedNamedItemView restserviceItemView;
  }
}