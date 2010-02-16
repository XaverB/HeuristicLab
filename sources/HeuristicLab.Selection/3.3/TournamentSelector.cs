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

using System.Collections.Generic;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Selection {
  /// <summary>
  /// A tournament selection operator which considers a single double quality value for selection.
  /// </summary>
  [Item("TournamentSelector", "A tournament selection operator which considers a single double quality value for selection.")]
  [EmptyStorableClass]
  [Creatable("Test")]
  public sealed class TournamentSelector : StochasticSingleObjectiveSelector {
    public ValueLookupParameter<IntData> GroupSizeParameter {
      get { return (ValueLookupParameter<IntData>)Parameters["GroupSize"]; }
    }

    public TournamentSelector() : base() {
      Parameters.Add(new ValueLookupParameter<IntData>("GroupSize", "The size of the tournament group.", new IntData(2)));
    }

    protected override void Select(ScopeList source, ScopeList target) {
      int count = NumberOfSelectedSubScopesParameter.ActualValue.Value;
      bool copy = CopySelectedParameter.Value.Value;
      IRandom random = RandomParameter.ActualValue;
      bool maximization = MaximizationParameter.ActualValue.Value;
      List<DoubleData> qualities = new List<DoubleData>(QualityParameter.ActualValue);
      int groupSize = GroupSizeParameter.ActualValue.Value;

      for (int i = 0; i < count; i++) {
        int best = random.Next(source.Count);
        int index;
        for (int j = 1; j < groupSize; j++) {
          index = random.Next(source.Count);
          if (((maximization) && (qualities[index].Value > qualities[best].Value)) ||
              ((!maximization) && (qualities[index].Value < qualities[best].Value))) {
            best = index;
          }
        }

        if (copy)
          target.Add((IScope)source[best].Clone());
        else {
          target.Add(source[best]);
          source.RemoveAt(best);
          qualities.RemoveAt(best);
        }
      }
    }
  }
}
