﻿#region License Information
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace HeuristicLab.MainForm.WindowsForms {
  public partial class DockingMainForm : MainForm {
    public DockingMainForm()
      : base() {
      InitializeComponent();
    }
    public DockingMainForm(Type userInterfaceItemType)
      : base(userInterfaceItemType) {
      InitializeComponent();
    }
    public DockingMainForm(Type userInterfaceItemType, bool showInViewHost)
      : this(userInterfaceItemType) {
      this.ShowInViewHost = showInViewHost;
    }

    protected override void Show(IView view, bool firstTimeShown) {
      if (InvokeRequired) Invoke((Action<IView, bool>)Show, view, firstTimeShown);
      else {
        base.Show(view, firstTimeShown);
        if (firstTimeShown)
          ((DockForm)GetForm(view)).Show(dockPanel);
        else
          ((DockForm)GetForm(view)).Activate();
      }
    }

    protected override void Hide(IView view) {
      if (InvokeRequired) Invoke((Action<IView>)HideView, view);
      else {
        Form form = base.GetForm(view);
        if (form != null) {
          ((DockForm)form).Hide();
        }
      }
    }

    protected override Form CreateForm(IView view) {
      DockForm form;
      IContentView contentView = view as IContentView;
      if (ShowInViewHost && contentView != null && contentView.GetType() != typeof(ViewHost)) {
        ViewHost viewHost = new ViewHost(contentView.Content);
        form = new DockForm(viewHost);
      } else
        form = new DockForm(view);
      return form;
    }

    private void dockPanel_ActiveContentChanged(object sender, EventArgs e) {
      DockForm content = this.dockPanel.ActiveContent as DockForm;
      if (content != null)
        this.ActiveView = content.View;
    }
  }
}
