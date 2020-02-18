﻿namespace HeuristicLab.JsonInterface.OptimizerIntegration {
  partial class NumericRangeControl {
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      this.textBoxFrom = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.checkBoxTo = new System.Windows.Forms.CheckBox();
      this.textBoxTo = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.checkBoxFrom = new System.Windows.Forms.CheckBox();
      this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // textBoxFrom
      // 
      this.textBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.errorProvider.SetIconAlignment(this.textBoxFrom, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
      this.textBoxFrom.Location = new System.Drawing.Point(67, 17);
      this.textBoxFrom.Name = "textBoxFrom";
      this.textBoxFrom.ReadOnly = true;
      this.textBoxFrom.Size = new System.Drawing.Size(253, 20);
      this.textBoxFrom.TabIndex = 2;
      this.textBoxFrom.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxFrom_Validating);
      // 
      // label6
      // 
      this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(31, 21);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(30, 13);
      this.label6.TabIndex = 0;
      this.label6.Text = "From";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.checkBoxTo);
      this.groupBox2.Controls.Add(this.textBoxTo);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Controls.Add(this.checkBoxFrom);
      this.groupBox2.Controls.Add(this.textBoxFrom);
      this.groupBox2.Controls.Add(this.label6);
      this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox2.Location = new System.Drawing.Point(0, 0);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(326, 75);
      this.groupBox2.TabIndex = 19;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Range";
      // 
      // checkBoxTo
      // 
      this.checkBoxTo.AutoSize = true;
      this.checkBoxTo.Location = new System.Drawing.Point(10, 46);
      this.checkBoxTo.Name = "checkBoxTo";
      this.checkBoxTo.Size = new System.Drawing.Size(15, 14);
      this.checkBoxTo.TabIndex = 7;
      this.checkBoxTo.UseVisualStyleBackColor = true;
      // 
      // textBoxTo
      // 
      this.textBoxTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.errorProvider.SetIconAlignment(this.textBoxTo, System.Windows.Forms.ErrorIconAlignment.TopLeft);
      this.textBoxTo.Location = new System.Drawing.Point(67, 43);
      this.textBoxTo.Name = "textBoxTo";
      this.textBoxTo.ReadOnly = true;
      this.textBoxTo.Size = new System.Drawing.Size(253, 20);
      this.textBoxTo.TabIndex = 6;
      this.textBoxTo.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxTo_Validating);
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(31, 46);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(20, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "To";
      // 
      // checkBoxFrom
      // 
      this.checkBoxFrom.AutoSize = true;
      this.checkBoxFrom.Location = new System.Drawing.Point(10, 20);
      this.checkBoxFrom.Name = "checkBoxFrom";
      this.checkBoxFrom.Size = new System.Drawing.Size(15, 14);
      this.checkBoxFrom.TabIndex = 4;
      this.checkBoxFrom.UseVisualStyleBackColor = true;
      // 
      // errorProvider
      // 
      this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
      this.errorProvider.ContainerControl = this;
      // 
      // NumericRangeControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox2);
      this.Name = "NumericRangeControl";
      this.Size = new System.Drawing.Size(326, 75);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.TextBox textBoxFrom;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.CheckBox checkBoxTo;
    private System.Windows.Forms.TextBox textBoxTo;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox checkBoxFrom;
    private System.Windows.Forms.ErrorProvider errorProvider;
  }
}
