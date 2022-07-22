#region License Information
/* HeuristicLab
 * Copyright (C) Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HeuristicLab.Optimizer;
using HeuristicLab.PluginInfrastructure;

namespace HeuristicLab.Shell.InternalShellMenuItem {
  internal class OpenMenuItem : HeuristicLab.MainForm.WindowsForms.MenuItem, IOptimizerUserInterfaceItemProvider {
    public override string Name {
      get { return "&Start Internal Shell..."; }
    }
    public override IEnumerable<string> Structure {
      get { return new string[] { "&Tools" }; }
    }
    public override int Position {
      get { return 8100; }
    }
    public override Image Image {
      get { return HeuristicLab.Common.Resources.VSImageLibrary.Script; }
    }
    public override Keys ShortCutKeys {
      get { return Keys.Control | Keys.Shift | Keys.S; }
    }

    public override void Execute() {
      ErrorHandling.ShowErrorDialog("It works, but is not implemented yet.", new System.Exception());
    }
  }
}
