#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2010 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

namespace HeuristicLab.Optimization.Views {
  partial class EngineAlgorithmView {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing) {
        if (engineTypeSelectorDialog != null) engineTypeSelectorDialog.Dispose();
        if (components != null) components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.engineLabel = new System.Windows.Forms.Label();
      this.engineTextBox = new System.Windows.Forms.TextBox();
      this.setEngineButton = new System.Windows.Forms.Button();
      this.createUserDefinedAlgorithmButton = new System.Windows.Forms.Button();
      this.tabControl.SuspendLayout();
      this.parametersTabPage.SuspendLayout();
      this.problemTabPage.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
      this.SuspendLayout();
      // 
      // tabControl
      // 
      this.tabControl.Location = new System.Drawing.Point(0, 149);
      this.tabControl.Size = new System.Drawing.Size(656, 278);
      this.tabControl.TabIndex = 7;
      // 
      // parametersTabPage
      // 
      this.parametersTabPage.Size = new System.Drawing.Size(648, 252);
      // 
      // problemTabPage
      // 
      this.problemTabPage.Size = new System.Drawing.Size(648, 252);
      // 
      // parameterCollectionView
      // 
      this.parameterCollectionView.Size = new System.Drawing.Size(636, 240);
      // 
      // problemViewHost
      // 
      this.problemViewHost.Size = new System.Drawing.Size(470, 106);
      // 
      // startButton
      // 
      this.startButton.Location = new System.Drawing.Point(0, 433);
      this.startButton.TabIndex = 8;
      // 
      // stopButton
      // 
      this.stopButton.Location = new System.Drawing.Point(30, 433);
      this.stopButton.TabIndex = 9;
      // 
      // resetButton
      // 
      this.resetButton.Location = new System.Drawing.Point(60, 433);
      this.resetButton.TabIndex = 10;
      // 
      // executionTimeLabel
      // 
      this.executionTimeLabel.Location = new System.Drawing.Point(430, 440);
      this.executionTimeLabel.TabIndex = 12;
      // 
      // executionTimeTextBox
      // 
      this.executionTimeTextBox.Location = new System.Drawing.Point(519, 437);
      this.executionTimeTextBox.TabIndex = 13;
      // 
      // nameTextBox
      // 
      this.errorProvider.SetIconAlignment(this.nameTextBox, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
      this.errorProvider.SetIconPadding(this.nameTextBox, 2);
      this.nameTextBox.Size = new System.Drawing.Size(584, 20);
      // 
      // descriptionTextBox
      // 
      this.descriptionTextBox.Size = new System.Drawing.Size(584, 87);
      // 
      // engineLabel
      // 
      this.engineLabel.AutoSize = true;
      this.engineLabel.Location = new System.Drawing.Point(3, 124);
      this.engineLabel.Name = "engineLabel";
      this.engineLabel.Size = new System.Drawing.Size(43, 13);
      this.engineLabel.TabIndex = 4;
      this.engineLabel.Text = "&Engine:";
      // 
      // engineTextBox
      // 
      this.engineTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.engineTextBox.Location = new System.Drawing.Point(72, 121);
      this.engineTextBox.Name = "engineTextBox";
      this.engineTextBox.ReadOnly = true;
      this.engineTextBox.Size = new System.Drawing.Size(554, 20);
      this.engineTextBox.TabIndex = 6;
      this.engineTextBox.DoubleClick += new System.EventHandler(this.engineTextBox_DoubleClick);
      // 
      // setEngineButton
      // 
      this.setEngineButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.setEngineButton.Image = HeuristicLab.Common.Resources.VS2008ImageLibrary.Add;
      this.setEngineButton.Location = new System.Drawing.Point(632, 119);
      this.setEngineButton.Name = "setEngineButton";
      this.setEngineButton.Size = new System.Drawing.Size(24, 24);
      this.setEngineButton.TabIndex = 5;
      this.toolTip.SetToolTip(this.setEngineButton, "Set Engine");
      this.setEngineButton.UseVisualStyleBackColor = true;
      this.setEngineButton.Click += new System.EventHandler(this.setEngineButton_Click);
      // 
      // createUserDefinedAlgorithmButton
      // 
      this.createUserDefinedAlgorithmButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.createUserDefinedAlgorithmButton.Location = new System.Drawing.Point(90, 433);
      this.createUserDefinedAlgorithmButton.Name = "createUserDefinedAlgorithmButton";
      this.createUserDefinedAlgorithmButton.Size = new System.Drawing.Size(254, 24);
      this.createUserDefinedAlgorithmButton.TabIndex = 11;
      this.createUserDefinedAlgorithmButton.Text = "&Create User Defined Algorithm";
      this.toolTip.SetToolTip(this.createUserDefinedAlgorithmButton, "Create User Defined Algorithm from this Algorithm");
      this.createUserDefinedAlgorithmButton.UseVisualStyleBackColor = true;
      this.createUserDefinedAlgorithmButton.Click += new System.EventHandler(this.createUserDefinedAlgorithmButton_Click);
      // 
      // EngineAlgorithmView
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.setEngineButton);
      this.Controls.Add(this.engineTextBox);
      this.Controls.Add(this.engineLabel);
      this.Controls.Add(this.createUserDefinedAlgorithmButton);
      this.Name = "EngineAlgorithmView";
      this.Size = new System.Drawing.Size(656, 457);
      this.Controls.SetChildIndex(this.createUserDefinedAlgorithmButton, 0);
      this.Controls.SetChildIndex(this.engineLabel, 0);
      this.Controls.SetChildIndex(this.engineTextBox, 0);
      this.Controls.SetChildIndex(this.setEngineButton, 0);
      this.Controls.SetChildIndex(this.resetButton, 0);
      this.Controls.SetChildIndex(this.executionTimeLabel, 0);
      this.Controls.SetChildIndex(this.executionTimeTextBox, 0);
      this.Controls.SetChildIndex(this.stopButton, 0);
      this.Controls.SetChildIndex(this.startButton, 0);
      this.Controls.SetChildIndex(this.tabControl, 0);
      this.Controls.SetChildIndex(this.nameLabel, 0);
      this.Controls.SetChildIndex(this.descriptionLabel, 0);
      this.Controls.SetChildIndex(this.nameTextBox, 0);
      this.Controls.SetChildIndex(this.descriptionTextBox, 0);
      this.tabControl.ResumeLayout(false);
      this.parametersTabPage.ResumeLayout(false);
      this.problemTabPage.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    protected System.Windows.Forms.Label engineLabel;
    protected System.Windows.Forms.TextBox engineTextBox;
    protected System.Windows.Forms.Button setEngineButton;
    protected System.Windows.Forms.Button createUserDefinedAlgorithmButton;


  }
}
