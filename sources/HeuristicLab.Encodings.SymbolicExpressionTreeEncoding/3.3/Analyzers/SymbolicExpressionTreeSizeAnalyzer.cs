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

using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using System.Collections.Generic;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding.Interfaces;
using HeuristicLab.Analysis;

namespace HeuristicLab.Encodings.SymbolicExpressionTreeEncoding.Analyzers {
  /// <summary>
  /// An operator that tracks the tree size of a symbolic expression tree.
  /// </summary>
  [Item("SymbolicExpressionTreeSizeAnalyzer", "An operator that tracks the tree size of symbolic expression trees.")]
  [StorableClass]
  public sealed class SymbolicExpressionTreeSizeAnalyzer : AlgorithmOperator, ISymbolicExpressionTreeAnalyzer {
    private const string SymbolicExpressionTreeParameterName = "SymbolicExpressionTree";
    private const string SymbolicExpressionTreeSizeParameterName = "SymbolicExpressionTreeSize";
    private const string SymbolicExpressionTreeSizesParameterName = "SymbolicExpressionTreeSizes";
    private const string ResultsParameterName = "Results";


    #region parameter properties
    public ILookupParameter<SymbolicExpressionTree> SymbolicExpressionTreeParameter {
      get { return (ILookupParameter<SymbolicExpressionTree>)Parameters[SymbolicExpressionTreeParameterName]; }
    }
    public ILookupParameter<DoubleValue> SymbolicExpressionTreeSizeParameter {
      get { return (ILookupParameter<DoubleValue>)Parameters[SymbolicExpressionTreeSizeParameterName]; }
    }
    public ValueLookupParameter<DataTable> SymbolicExpressionTreeSizesParameter {
      get { return (ValueLookupParameter<DataTable>)Parameters[SymbolicExpressionTreeSizesParameterName]; }
    }
    public ValueLookupParameter<VariableCollection> ResultsParameter {
      get { return (ValueLookupParameter<VariableCollection>)Parameters[ResultsParameterName]; }
    }

    #endregion

    public SymbolicExpressionTreeSizeAnalyzer()
      : base() {
      Parameters.Add(new LookupParameter<SymbolicExpressionTree>(SymbolicExpressionTreeParameterName, "The symbolic expression tree whose size should be calculated."));
      Parameters.Add(new LookupParameter<DoubleValue>(SymbolicExpressionTreeSizeParameterName, "The tree size of the symbolic expression tree."));
      Parameters.Add(new ValueLookupParameter<DataTable>(SymbolicExpressionTreeSizesParameterName, "The data table to store the tree sizes."));
      Parameters.Add(new ValueLookupParameter<VariableCollection>(ResultsParameterName, "The results collection where the analysis values should be stored."));


      SymbolicExpressionTreeSizeCalculator sizeCalculator = new SymbolicExpressionTreeSizeCalculator();
      SolutionValueAnalyzer valueAnalyzer = new SolutionValueAnalyzer();
      sizeCalculator.SymbolicExpressionTreeParameter.ActualName = SymbolicExpressionTreeParameter.Name;
      sizeCalculator.SymbolicExpressionTreeSizeParameter.ActualName = SymbolicExpressionTreeSizeParameter.Name;
      valueAnalyzer.ValueParameter.ActualName = sizeCalculator.SymbolicExpressionTreeSizeParameter.Name;
      valueAnalyzer.ValuesParameter.ActualName = SymbolicExpressionTreeSizesParameter.Name;
      valueAnalyzer.ResultsParameter.ActualName = ResultsParameter.Name;

      OperatorGraph.InitialOperator = sizeCalculator;
      sizeCalculator.Successor = valueAnalyzer;
      valueAnalyzer.Successor = null;
    }
  }
}
