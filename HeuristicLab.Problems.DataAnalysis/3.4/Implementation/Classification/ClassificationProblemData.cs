﻿#region License Information
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
  [Item("ClassificationProblemData", "Represents an item containing all data defining a classification problem.")]
  public class ClassificationProblemData : DataAnalysisProblemData, IClassificationProblemData {
    private const string TargetVariableParameterName = "TargetVariable";
    private const string ClassNamesParameterName = "ClassNames";
    private const string ClassificationPenaltiesParameterName = "ClassificationPenalties";

    #region default data
    private static string[] defaultVariableNames = new string[] { "sample", "clump thickness", "cell size", "cell shape", "marginal adhesion", "epithelial cell size", "bare nuclei", "chromatin", "nucleoli", "mitoses", "class" };
    private static double[,] defaultData = new double[,]{
     {1000025,5,1,1,1,2,1,3,1,1,2      },
     {1002945,5,4,4,5,7,10,3,2,1,2     },
     {1015425,3,1,1,1,2,2,3,1,1,2      },
     {1016277,6,8,8,1,3,4,3,7,1,2      },
     {1017023,4,1,1,3,2,1,3,1,1,2      },
     {1017122,8,10,10,8,7,10,9,7,1,4   },
     {1018099,1,1,1,1,2,10,3,1,1,2     },
     {1018561,2,1,2,1,2,1,3,1,1,2      },
     {1033078,2,1,1,1,2,1,1,1,5,2      },
     {1033078,4,2,1,1,2,1,2,1,1,2      },
     {1035283,1,1,1,1,1,1,3,1,1,2      },
     {1036172,2,1,1,1,2,1,2,1,1,2      },
     {1041801,5,3,3,3,2,3,4,4,1,4      },
     {1043999,1,1,1,1,2,3,3,1,1,2      },
     {1044572,8,7,5,10,7,9,5,5,4,4     },
     {1047630,7,4,6,4,6,1,4,3,1,4      },
     {1048672,4,1,1,1,2,1,2,1,1,2      },
     {1049815,4,1,1,1,2,1,3,1,1,2      },
     {1050670,10,7,7,6,4,10,4,1,2,4    },
     {1050718,6,1,1,1,2,1,3,1,1,2      },
     {1054590,7,3,2,10,5,10,5,4,4,4    },
     {1054593,10,5,5,3,6,7,7,10,1,4    },
     {1056784,3,1,1,1,2,1,2,1,1,2      },
     {1057013,8,4,5,1,2,2,7,3,1,4      },
     {1059552,1,1,1,1,2,1,3,1,1,2      },
     {1065726,5,2,3,4,2,7,3,6,1,4      },
     {1066373,3,2,1,1,1,1,2,1,1,2      },
     {1066979,5,1,1,1,2,1,2,1,1,2      },
     {1067444,2,1,1,1,2,1,2,1,1,2      },
     {1070935,1,1,3,1,2,1,1,1,1,2      },
     {1070935,3,1,1,1,1,1,2,1,1,2      },
     {1071760,2,1,1,1,2,1,3,1,1,2      },
     {1072179,10,7,7,3,8,5,7,4,3,4     },
     {1074610,2,1,1,2,2,1,3,1,1,2      },
     {1075123,3,1,2,1,2,1,2,1,1,2      },
     {1079304,2,1,1,1,2,1,2,1,1,2      },
     {1080185,10,10,10,8,6,1,8,9,1,4   },
     {1081791,6,2,1,1,1,1,7,1,1,2      },
     {1084584,5,4,4,9,2,10,5,6,1,4     },
     {1091262,2,5,3,3,6,7,7,5,1,4      },
     {1096800,6,6,6,9,6,4,7,8,1,2      },
     {1099510,10,4,3,1,3,3,6,5,2,4     },
     {1100524,6,10,10,2,8,10,7,3,3,4   },
     {1102573,5,6,5,6,10,1,3,1,1,4     },
     {1103608,10,10,10,4,8,1,8,10,1,4  },
     {1103722,1,1,1,1,2,1,2,1,2,2      },
     {1105257,3,7,7,4,4,9,4,8,1,4      },
     {1105524,1,1,1,1,2,1,2,1,1,2      },
     {1106095,4,1,1,3,2,1,3,1,1,2      },
     {1106829,7,8,7,2,4,8,3,8,2,4      },
     {1108370,9,5,8,1,2,3,2,1,5,4      },
     {1108449,5,3,3,4,2,4,3,4,1,4      },
     {1110102,10,3,6,2,3,5,4,10,2,4    },
     {1110503,5,5,5,8,10,8,7,3,7,4     },
     {1110524,10,5,5,6,8,8,7,1,1,4     },
     {1111249,10,6,6,3,4,5,3,6,1,4     },
     {1112209,8,10,10,1,3,6,3,9,1,4    },
     {1113038,8,2,4,1,5,1,5,4,4,4      },
     {1113483,5,2,3,1,6,10,5,1,1,4     },
     {1113906,9,5,5,2,2,2,5,1,1,4      },
     {1115282,5,3,5,5,3,3,4,10,1,4     },
     {1115293,1,1,1,1,2,2,2,1,1,2      },
     {1116116,9,10,10,1,10,8,3,3,1,4   },
     {1116132,6,3,4,1,5,2,3,9,1,4      },
     {1116192,1,1,1,1,2,1,2,1,1,2      },
     {1116998,10,4,2,1,3,2,4,3,10,4    },
     {1117152,4,1,1,1,2,1,3,1,1,2      },
     {1118039,5,3,4,1,8,10,4,9,1,4     },
     {1120559,8,3,8,3,4,9,8,9,8,4      },
     {1121732,1,1,1,1,2,1,3,2,1,2      },
     {1121919,5,1,3,1,2,1,2,1,1,2      },
     {1123061,6,10,2,8,10,2,7,8,10,4   },
     {1124651,1,3,3,2,2,1,7,2,1,2      },
     {1125035,9,4,5,10,6,10,4,8,1,4    },
     {1126417,10,6,4,1,3,4,3,2,3,4     },
     {1131294,1,1,2,1,2,2,4,2,1,2      },
     {1132347,1,1,4,1,2,1,2,1,1,2      },
     {1133041,5,3,1,2,2,1,2,1,1,2      },
     {1133136,3,1,1,1,2,3,3,1,1,2      },
     {1136142,2,1,1,1,3,1,2,1,1,2      },
     {1137156,2,2,2,1,1,1,7,1,1,2      },
     {1143978,4,1,1,2,2,1,2,1,1,2      },
     {1143978,5,2,1,1,2,1,3,1,1,2      },
     {1147044,3,1,1,1,2,2,7,1,1,2      },
     {1147699,3,5,7,8,8,9,7,10,7,4     },
     {1147748,5,10,6,1,10,4,4,10,10,4  },
     {1148278,3,3,6,4,5,8,4,4,1,4      },
     {1148873,3,6,6,6,5,10,6,8,3,4     },
     {1152331,4,1,1,1,2,1,3,1,1,2      },
     {1155546,2,1,1,2,3,1,2,1,1,2      },
     {1156272,1,1,1,1,2,1,3,1,1,2      },
     {1156948,3,1,1,2,2,1,1,1,1,2      },
     {1157734,4,1,1,1,2,1,3,1,1,2      },
     {1158247,1,1,1,1,2,1,2,1,1,2      },
     {1160476,2,1,1,1,2,1,3,1,1,2      },
     {1164066,1,1,1,1,2,1,3,1,1,2      },
     {1165297,2,1,1,2,2,1,1,1,1,2      },
     {1165790,5,1,1,1,2,1,3,1,1,2      },
     {1165926,9,6,9,2,10,6,2,9,10,4    },
     {1166630,7,5,6,10,5,10,7,9,4,4    },
     {1166654,10,3,5,1,10,5,3,10,2,4   },
     {1167439,2,3,4,4,2,5,2,5,1,4      },
     {1167471,4,1,2,1,2,1,3,1,1,2      },
     {1168359,8,2,3,1,6,3,7,1,1,4      },
     {1168736,10,10,10,10,10,1,8,8,8,4 },
     {1169049,7,3,4,4,3,3,3,2,7,4      },
     {1170419,10,10,10,8,2,10,4,1,1,4  },
     {1170420,1,6,8,10,8,10,5,7,1,4    },
     {1171710,1,1,1,1,2,1,2,3,1,2      },
     {1171710,6,5,4,4,3,9,7,8,3,4      },
     {1171795,1,3,1,2,2,2,5,3,2,2      },
     {1171845,8,6,4,3,5,9,3,1,1,4      },
     {1172152,10,3,3,10,2,10,7,3,3,4   },
     {1173216,10,10,10,3,10,8,8,1,1,4  },
     {1173235,3,3,2,1,2,3,3,1,1,2      },
     {1173347,1,1,1,1,2,5,1,1,1,2      },
     {1173347,8,3,3,1,2,2,3,2,1,2      },
     {1173509,4,5,5,10,4,10,7,5,8,4    },
     {1173514,1,1,1,1,4,3,1,1,1,2      },
     {1173681,3,2,1,1,2,2,3,1,1,2      },
     {1174057,1,1,2,2,2,1,3,1,1,2      },
     {1174057,4,2,1,1,2,2,3,1,1,2      },
     {1174131,10,10,10,2,10,10,5,3,3,4 },
     {1174428,5,3,5,1,8,10,5,3,1,4     },
     {1175937,5,4,6,7,9,7,8,10,1,4     },
     {1176406,1,1,1,1,2,1,2,1,1,2      },
     {1176881,7,5,3,7,4,10,7,5,5,4        }
};
    private static Dataset defaultDataset;
    private static IEnumerable<string> defaultAllowedInputVariables;
    private static string defaultTargetVariable;
    static ClassificationProblemData() {
      defaultDataset = new Dataset(defaultVariableNames, defaultData);
      defaultDataset.Name = "Wisconsin classification problem";
      defaultDataset.Description = "subset from to ..";

      defaultAllowedInputVariables = defaultVariableNames.Except(new List<string>() { "sample", "class" });
      defaultTargetVariable = "class";
    }
    #endregion

    #region parameter properties
    public IValueParameter<StringValue> TargetVariableParameter {
      get { return (IValueParameter<StringValue>)Parameters[TargetVariableParameterName]; }
    }
    public IFixedValueParameter<StringMatrix> ClassNamesParameter {
      get { return (IFixedValueParameter<StringMatrix>)Parameters[ClassNamesParameterName]; }
    }
    public IFixedValueParameter<DoubleMatrix> ClassificationPenaltiesParameter {
      get { return (IFixedValueParameter<DoubleMatrix>)Parameters[ClassificationPenaltiesParameterName]; }
    }
    #endregion

    #region properties
    public string TargetVariable {
      get { return TargetVariableParameter.Value.Value; }
    }

    private List<double> classValues;
    public List<double> ClassValues {
      get {
        if (classValues == null) {
          classValues = Dataset.GetEnumeratedVariableValues(TargetVariableParameter.Value.Value).Distinct().ToList();
          classValues.Sort();
        }
        return classValues;
      }
    }
    IEnumerable<double> IClassificationProblemData.ClassValues {
      get { return ClassValues; }
    }

    public int Classes {
      get { return ClassValues.Count; }
    }

    private List<string> classNames;
    public List<string> ClassNames {
      get {
        if (classNames == null) {
          classNames = new List<string>();
          for (int i = 0; i < ClassNamesParameter.Value.Rows; i++)
            classNames.Add(ClassNamesParameter.Value[i, 0]);
        }
        return classNames;
      }
    }
    IEnumerable<string> IClassificationProblemData.ClassNames {
      get { return ClassNames; }
    }

    private Dictionary<Tuple<double, double>, double> classificationPenaltiesCache = new Dictionary<Tuple<double, double>, double>();
    #endregion


    [StorableConstructor]
    protected ClassificationProblemData(bool deserializing) : base(deserializing) { }
    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      RegisterParameterEvents();
    }

    protected ClassificationProblemData(ClassificationProblemData original, Cloner cloner)
      : base(original, cloner) {
      RegisterParameterEvents();
    }
    public override IDeepCloneable Clone(Cloner cloner) { return new ClassificationProblemData(this, cloner); }

    public ClassificationProblemData() : this(defaultDataset, defaultAllowedInputVariables, defaultTargetVariable) { }
    public ClassificationProblemData(Dataset dataset, IEnumerable<string> allowedInputVariables, string targetVariable)
      : base(dataset, allowedInputVariables) {
      var variables = InputVariables.Select(x => x.AsReadOnly()).ToList();
      Parameters.Add(new ConstrainedValueParameter<StringValue>(TargetVariableParameterName, new ItemSet<StringValue>(variables), variables.Where(x => x.Value == targetVariable).First()));
      Parameters.Add(new FixedValueParameter<StringMatrix>(ClassNamesParameterName, ""));
      Parameters.Add(new FixedValueParameter<DoubleMatrix>(ClassificationPenaltiesParameterName, ""));

      ResetTargetVariableDependentMembers();
      RegisterParameterEvents();
    }

    private void ResetTargetVariableDependentMembers() {
      DergisterParameterEvents();

      classNames = null;
      ((IStringConvertibleMatrix)ClassNamesParameter.Value).Columns = 1;
      ((IStringConvertibleMatrix)ClassNamesParameter.Value).Rows = ClassValues.Count;
      for (int i = 0; i < Classes; i++)
        ClassNamesParameter.Value[i, 0] = "Class " + ClassValues[i];
      ClassNamesParameter.Value.ColumnNames = new List<string>() { "ClassNames" };
      ClassNamesParameter.Value.RowNames = ClassValues.Select(s => "ClassValue: " + s);

      classificationPenaltiesCache.Clear();
      ((ValueParameter<DoubleMatrix>)ClassificationPenaltiesParameter).ReactOnValueToStringChangedAndValueItemImageChanged = false;
      ((IStringConvertibleMatrix)ClassificationPenaltiesParameter.Value).Rows = Classes;
      ((IStringConvertibleMatrix)ClassificationPenaltiesParameter.Value).Columns = Classes;
      ClassificationPenaltiesParameter.Value.RowNames = ClassNames.Select(name => "Actual " + name);
      ClassificationPenaltiesParameter.Value.ColumnNames = ClassNames.Select(name => "Estimated " + name);
      for (int i = 0; i < Classes; i++) {
        for (int j = 0; j < Classes; j++) {
          if (i != j) ClassificationPenaltiesParameter.Value[i, j] = 1;
          else ClassificationPenaltiesParameter.Value[i, j] = 0;
        }
      }
      ((ValueParameter<DoubleMatrix>)ClassificationPenaltiesParameter).ReactOnValueToStringChangedAndValueItemImageChanged = true;
      RegisterParameterEvents();
    }

    public string GetClassName(double classValue) {
      if (!ClassValues.Contains(classValue)) throw new ArgumentException();
      int index = ClassValues.IndexOf(classValue);
      return ClassNames[index];
    }
    public double GetClassValue(string className) {
      if (!ClassNames.Contains(className)) throw new ArgumentException();
      int index = ClassNames.IndexOf(className);
      return ClassValues[index];
    }
    public void SetClassName(double classValue, string className) {
      if (!classValues.Contains(classValue)) throw new ArgumentException();
      int index = ClassValues.IndexOf(classValue);
      ClassNames[index] = className;
      ClassNamesParameter.Value[index, 0] = className;
    }

    public double GetClassificationPenalty(string correctClassName, string estimatedClassName) {
      return GetClassificationPenalty(GetClassValue(correctClassName), GetClassValue(estimatedClassName));
    }
    public double GetClassificationPenalty(double correctClassValue, double estimatedClassValue) {
      var key = Tuple.Create(correctClassValue, estimatedClassValue);
      if (!classificationPenaltiesCache.ContainsKey(key)) {
        int correctClassIndex = ClassValues.IndexOf(correctClassValue);
        int estimatedClassIndex = ClassValues.IndexOf(estimatedClassValue);
        classificationPenaltiesCache[key] = ClassificationPenaltiesParameter.Value[correctClassIndex, estimatedClassIndex];
      }
      return classificationPenaltiesCache[key];
    }
    public void SetClassificationPenalty(string correctClassName, string estimatedClassName, double penalty) {
      SetClassificationPenalty(GetClassValue(correctClassName), GetClassValue(estimatedClassName), penalty);
    }
    public void SetClassificationPenalty(double correctClassValue, double estimatedClassValue, double penalty) {
      var key = Tuple.Create(correctClassValue, estimatedClassValue);
      int correctClassIndex = ClassValues.IndexOf(correctClassValue);
      int estimatedClassIndex = ClassValues.IndexOf(estimatedClassValue);

      ClassificationPenaltiesParameter.Value[correctClassIndex, estimatedClassIndex] = penalty;
    }

    #region events
    private void RegisterParameterEvents() {
      TargetVariableParameter.ValueChanged += new EventHandler(TargetVariableParameter_ValueChanged);
      ClassNamesParameter.Value.Reset += new EventHandler(Parameter_ValueChanged);
      ClassNamesParameter.Value.ItemChanged += new EventHandler<EventArgs<int, int>>(MatrixParameter_ItemChanged);
      ClassificationPenaltiesParameter.Value.Reset += new EventHandler(Parameter_ValueChanged);
      ClassificationPenaltiesParameter.Value.ItemChanged += new EventHandler<EventArgs<int, int>>(MatrixParameter_ItemChanged);
    }
    private void DergisterParameterEvents() {
      TargetVariableParameter.ValueChanged -= new EventHandler(TargetVariableParameter_ValueChanged);
      ClassNamesParameter.Value.Reset -= new EventHandler(Parameter_ValueChanged);
      ClassNamesParameter.Value.ItemChanged -= new EventHandler<EventArgs<int, int>>(MatrixParameter_ItemChanged);
      ClassificationPenaltiesParameter.Value.Reset -= new EventHandler(Parameter_ValueChanged);
      ClassificationPenaltiesParameter.Value.ItemChanged -= new EventHandler<EventArgs<int, int>>(MatrixParameter_ItemChanged);
    }

    private void TargetVariableParameter_ValueChanged(object sender, EventArgs e) {
      classValues = null;
      ResetTargetVariableDependentMembers();
      OnChanged();
    }
    private void Parameter_ValueChanged(object sender, EventArgs e) {
      OnChanged();
    }
    private void MatrixParameter_ItemChanged(object sender, EventArgs<int, int> e) {
      OnChanged();
    }
    #endregion

    #region Import from file
    public static ClassificationProblemData ImportFromFile(string fileName) {
      TableFileParser csvFileParser = new TableFileParser();
      csvFileParser.Parse(fileName);

      Dataset dataset = new Dataset(csvFileParser.VariableNames, csvFileParser.Values);
      dataset.Name = Path.GetFileName(fileName);

      ClassificationProblemData problemData = new ClassificationProblemData(dataset, dataset.VariableNames.Skip(1), dataset.VariableNames.First());
      problemData.Name = "Data imported from " + Path.GetFileName(fileName);
      return problemData;
    }
    #endregion
  }
}
