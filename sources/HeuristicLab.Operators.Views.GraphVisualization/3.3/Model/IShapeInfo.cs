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
using System.Linq;
using System.Text;
using HeuristicLab.Core;
using System.Drawing;
using Netron.Diagramming.Core;
using HeuristicLab.Collections;

namespace HeuristicLab.Operators.Views.GraphVisualization {
  public interface IShapeInfo : IItem{
    Type ShapeType { get; }
    Point Location { get; set; }
    Size Size { get; set; }

    void AddConnector(string connectorName);
    void RemoveConnector(string connectorName);

    IEnumerable<KeyValuePair<string,IShapeInfo>> Connections {get;}
    INotifyObservableDictionaryItemsChanged<string, IShapeInfo> ObservableConnections { get; }

    void AddConnection(string fromConnectorName, IShapeInfo toShapeInfo);
    void RemoveConnection(string fromConnectorName);
    void ChangeConnection(string fromConnector, IShapeInfo toShapeInfo);

    IShape CreateShape();
    void UpdateShape(IShape shape);
  }
}
