﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2008 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HeuristicLab.Core;

namespace HeuristicLab.SupportVectorMachines {
  public partial class SVMModelView : ViewBase {
    private SVMModel model;
    
    public SVMModelView(SVMModel model) : base() {
      InitializeComponent();
      this.model = model;
      model.Changed += (sender, args) => UpdateControls();

      UpdateControls();
    }

    protected override void UpdateControls() {
      base.UpdateControls();
      numberOfSupportVectors.Text = model.Model.SupportVectorCount.ToString();
      rho.Text = model.Model.Rho[0].ToString();
      svmType.Text = model.Model.Parameter.SvmType.ToString();
      kernelType.Text = model.Model.Parameter.KernelType.ToString();
      gamma.Text = model.Model.Parameter.Gamma.ToString();
      cost.Text = model.Model.Parameter.C.ToString();
      nu.Text = model.Model.Parameter.Nu.ToString();
    }
  }
}
