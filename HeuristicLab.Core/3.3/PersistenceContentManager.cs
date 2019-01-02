﻿#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2019 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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

using System.Threading;
using HeuristicLab.Common;
using HeuristicLab.Persistence.Default.Xml;
using HEAL.Fossil;

namespace HeuristicLab.Core {
  public class PersistenceContentManager : ContentManager {
    public PersistenceContentManager() : base() { }

    protected override IStorableContent LoadContent(string filename) {
      // first try to load using the new persistence format
      try {
        var ser = new ProtoBufSerializer();
        return (IStorableContent)ser.Deserialize(filename);
      } catch (PersistenceException e) {
        // try old format if new format fails
        return XmlParser.Deserialize<IStorableContent>(filename);
      }
    }

    protected override void SaveContent(IStorableContent content, string filename, bool compressed, CancellationToken cancellationToken) {
      // XmlGenerator.Serialize(content, filename, compressed ? CompressionLevel.Optimal : CompressionLevel.NoCompression, cancellationToken);
      // store files with the new persistence format
      var ser = new ProtoBufSerializer();
      ser.Serialize(content, filename); // TODO: support cancellation
    }
  }
}
