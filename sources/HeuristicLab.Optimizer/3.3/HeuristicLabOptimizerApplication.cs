#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2009 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using HeuristicLab.PluginInfrastructure;
using HeuristicLab.Common.Resources;
using HeuristicLab.MainForm.WindowsForms;

namespace HeuristicLab.Optimizer {
  [ClassInfo(Name = "Optimizer 3.3", Description="Next generation heuristic optimization environment.")]
  class HeuristicLabOptimizerApplication : ApplicationBase {
    public override void Run() {
      DockingMainForm mainForm = new DockingMainForm(typeof(IOptimizerUserInterfaceItemProvider));
      mainForm.Title = "HeuristicLab Optimizer " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
      mainForm.Icon = Resources.HeuristicLabIcon;
      Application.Run(mainForm);
    }
  }
}
