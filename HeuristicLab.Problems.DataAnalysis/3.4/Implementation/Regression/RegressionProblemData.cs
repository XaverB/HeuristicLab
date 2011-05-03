#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2011 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
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
using System.IO;
using System.Linq;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.DataAnalysis {
  [StorableClass]
  [Item("RegressionProblemData", "Represents an item containing all data defining a regression problem.")]
  public sealed class RegressionProblemData : DataAnalysisProblemData, IRegressionProblemData {
    private const string TargetVariableParameterName = "TargetVariable";

    #region default data
    private static double[,] kozaF1 = new double[,] {
          {2.017885919,	-1.449165046},
          {1.30060506,	-1.344523885},
          {1.147134798,	-1.317989331},
          {0.877182504,	-1.266142284},
          {0.852562452,	-1.261020794},
          {0.431095788,	-1.158793317},
          {0.112586002,	-1.050908405},
          {0.04594507,	-1.021989402},
          {0.042572879,	-1.020438113},
          {-0.074027291,	-0.959859562},
          {-0.109178553,	-0.938094706},
          {-0.259721109,	-0.803635355},
          {-0.272991057,	-0.387519561},
          {-0.161978191,	-0.193611001},
          {-0.102489983,	-0.114215349},
          {-0.01469968,	-0.014918985},
          {-0.008863365,	-0.008942626},
          {0.026751057,	0.026054094},
          {0.166922436,	0.14309643},
          {0.176953808,	0.1504144},
          {0.190233418,	0.159916534},
          {0.199800708,	0.166635331},
          {0.261502822,	0.207600348},
          {0.30182879,	0.232370249},
          {0.83763905,	0.468046718}
    };
    private static Dataset defaultDataset;
    private static IEnumerable<string> defaultAllowedInputVariables;
    private static string defaultTargetVariable;

    static RegressionProblemData() {
      defaultDataset = new Dataset(new string[] { "y", "x" }, kozaF1);
      defaultDataset.Name = "Fourth-order Polynomial Function Benchmark Dataset";
      defaultDataset.Description = "f(x) = x^4 + x^3 + x^2 + x^1";
      defaultAllowedInputVariables = new List<string>() { "x" };
      defaultTargetVariable = "y";
    }
    #endregion

    public IValueParameter<StringValue> TargetVariableParameter {
      get { return (IValueParameter<StringValue>)Parameters[TargetVariableParameterName]; }
    }
    public string TargetVariable {
      get { return TargetVariableParameter.Value.Value; }
    }

    [StorableConstructor]
    private RegressionProblemData(bool deserializing) : base(deserializing) { }
    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      RegisterParameterEvents();
    }


    private RegressionProblemData(RegressionProblemData original, Cloner cloner)
      : base(original, cloner) {
      RegisterParameterEvents();
    }
    public override IDeepCloneable Clone(Cloner cloner) { return new RegressionProblemData(this, cloner); }

    public RegressionProblemData()
      : this(defaultDataset, defaultAllowedInputVariables, defaultTargetVariable) {
    }

    public RegressionProblemData(Dataset dataset, IEnumerable<string> allowedInputVariables, string targetVariable)
      : base(dataset, allowedInputVariables) {
      var variables = InputVariables.Select(x => x.AsReadOnly()).ToList();
      Parameters.Add(new ConstrainedValueParameter<StringValue>(TargetVariableParameterName, new ItemSet<StringValue>(variables), variables.Where(x => x.Value == targetVariable).First()));
      RegisterParameterEvents();
    }

    private void RegisterParameterEvents() {
      TargetVariableParameter.ValueChanged += new EventHandler(TargetVariableParameter_ValueChanged);
    }
    private void TargetVariableParameter_ValueChanged(object sender, EventArgs e) {
      OnChanged();
    }

    #region Import from file
    public static RegressionProblemData ImportFromFile(string fileName) {
      TableFileParser csvFileParser = new TableFileParser();
      csvFileParser.Parse(fileName);

      Dataset dataset = new Dataset(csvFileParser.VariableNames, csvFileParser.Values);
      dataset.Name = Path.GetFileName(fileName);

      RegressionProblemData problemData = new RegressionProblemData(dataset, dataset.VariableNames.Skip(1), dataset.VariableNames.First());
      problemData.Name = "Data imported from " + Path.GetFileName(fileName);
      return problemData;
    }
    #endregion
  }
}
