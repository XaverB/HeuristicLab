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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HeuristicLab.MainForm;

namespace HeuristicLab.MainForm.WindowsForms {
  [Content(typeof(object))]
  public sealed partial class ViewHost : ContentView {
    private Dictionary<Type, IContentView> cachedViews;
    public ViewHost() {
      InitializeComponent();
      cachedViews = new Dictionary<Type, IContentView>();
      viewType = null;
      Content = null;
      startDragAndDrop = false;
      viewContextMenuStrip.IgnoredViewTypes = new List<Type>() { typeof(ViewHost) };
    }
    public ViewHost(object content)
      : this() {
      this.Content = content;
    }

    private Type viewType;
    public Type ViewType {
      get { return this.viewType; }
      set {
        if (viewType != value) {
          if (value != null && !ViewCanShowContent(value, Content))
            throw new ArgumentException(string.Format("View \"{0}\" cannot display content \"{1}\".",
                                                      value, Content.GetType()));
          viewType = value;
          UpdateView();
        }
      }
    }

    public new object Content {
      get { return base.Content; }
      set {
        if (value == null || this.Content == null || value.GetType() != this.Content.GetType())
          cachedViews.Clear();
        viewContextMenuStrip.Item = value;
        this.viewsLabel.Enabled = value != null;
        base.Content = value;
      }
    }

    public new bool Enabled {
      get { return base.Enabled; }
      set {
        this.SuspendRepaint();
        base.Enabled = value;
        this.viewsLabel.Enabled = value;
        this.ResumeRepaint(true);
      }
    }

    protected override void OnReadOnlyChanged() {
      base.OnReadOnlyChanged();
      foreach (IContentView view in cachedViews.Values)
        view.ReadOnly = this.ReadOnly;
    }

    protected override void OnContentChanged() {
      messageLabel.Visible = false;
      viewsLabel.Visible = false;
      viewPanel.Visible = false;
      if (viewPanel.Controls.Count > 0) viewPanel.Controls[0].Dispose();
      viewPanel.Controls.Clear();

      if (Content != null) {
        if (viewContextMenuStrip.Items.Count == 0)
          messageLabel.Visible = true;
        else
          viewsLabel.Visible = true;

        if (!ViewCanShowContent(viewType, Content)) {
          ViewType = MainFormManager.GetDefaultViewType(Content.GetType());
          if ((viewType == null) && (viewContextMenuStrip.Items.Count > 0))  // create first available view if default view is not available
            ViewType = (Type)viewContextMenuStrip.Items[0].Tag;
        }
        foreach (IContentView view in cachedViews.Values)
          view.Content = this.Content;
      } else {
        if (viewPanel.Controls.Count > 0) viewPanel.Controls[0].Dispose();
        viewPanel.Controls.Clear();
        cachedViews.Clear();
      }
    }


    private void UpdateView() {
      viewPanel.Controls.Clear();

      if (viewType == null || Content == null)
        return;

      if (!ViewCanShowContent(viewType, Content))
        throw new InvalidOperationException(string.Format("View \"{0}\" cannot display content \"{1}\".",
                                                          viewType, Content.GetType()));

      UpdateActiveMenuItem();
      IContentView view;
      if (cachedViews.ContainsKey(ViewType))
        view = cachedViews[ViewType];
      else {
        view = MainFormManager.CreateView(viewType, Content, ReadOnly);
        cachedViews.Add(viewType, view);
      }
      this.Caption = view.Caption;
      this.SaveEnabled = view.SaveEnabled;

      Control control = (Control)view;
      control.Dock = DockStyle.Fill;
      viewPanel.Controls.Add(control);
      viewPanel.Visible = true;
    }

    private void UpdateActiveMenuItem() {
      foreach (KeyValuePair<Type, ToolStripMenuItem> item in viewContextMenuStrip.MenuItems) {
        if (item.Key == viewType) {
          item.Value.Checked = true;
          item.Value.Enabled = false;
        } else {
          item.Value.Checked = false;
          item.Value.Enabled = true;
        }
      }
    }

    private bool ViewCanShowContent(Type viewType, object content) {
      if (content == null) // every view can display null
        return true;
      if (viewType == null)
        return false;
      return ContentAttribute.CanViewType(viewType, Content.GetType()) && viewContextMenuStrip.MenuItems.Any(item => item.Key == viewType);
    }

    private void viewsLabel_DoubleClick(object sender, EventArgs e) {
      MainFormManager.CreateView(viewType, Content, ReadOnly).Show();
    }

    private void viewContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e) {
      Type viewType = (Type)e.ClickedItem.Tag;
      ViewType = viewType;
    }

    private bool startDragAndDrop;
    private void viewsLabel_MouseDown(object sender, MouseEventArgs e) {
      startDragAndDrop = true;
      viewsLabel.Capture = false;
    }

    private void viewsLabel_MouseLeave(object sender, EventArgs e) {
      if ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left && startDragAndDrop) {
        DataObject data = new DataObject();
        data.SetData("Type", Content.GetType());
        data.SetData("Value", Content);
        DoDragDrop(data, DragDropEffects.Copy | DragDropEffects.Link);
      } else
        startDragAndDrop = false;
    }
  }
}
