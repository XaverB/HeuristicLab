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
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Analysis {
  /// <summary>
  /// Represents a collection of allele frequencies collections.
  /// </summary>
  [Item("AlleleFrequencyCollectionCollection", "Represents a collection of allele frequencies collections.")]
  [StorableClass]
  public class AlleleFrequencyCollectionCollection : ItemCollection<AlleleFrequencyCollection> {
    public AlleleFrequencyCollectionCollection() : base() { }
    public AlleleFrequencyCollectionCollection(IEnumerable<AlleleFrequencyCollection> collections) : base(new ItemCollection<AlleleFrequencyCollection>(collections)) { }
    [StorableConstructor]
    protected AlleleFrequencyCollectionCollection(bool deserializing) : base(deserializing) { }
  }
}
