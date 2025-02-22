﻿#region License Information
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

using System;
using System.Collections.Generic;
using System.Linq;
using HEAL.Attic;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Encodings.SymbolicExpressionTreeEncoding;
using HeuristicLab.Problems.DataAnalysis;
using HeuristicLab.Problems.DataAnalysis.Symbolic;

namespace HeuristicLab.Algorithms.DataAnalysis {
  [StorableType("55412E08-DAD4-4C2E-9181-C142E7EA9474")]
  [Item("RandomForestModelFull", "Represents a random forest for regression and classification.")]
  public sealed class RandomForestModelFull : ClassificationModel, IRandomForestModel {

    public override IEnumerable<string> VariablesUsedForPrediction {
      get { return inputVariables; }
    }

    [Storable]
    private double[] classValues;

    public int NumClasses => classValues == null ? 0 : classValues.Length;

    [Storable]
    private string[] inputVariables;

    [Storable]
    public int NumberOfTrees {
      get; private set;
    }

    // not persisted
    private alglib.decisionforest randomForest;

    [Storable]
    private string RandomForestSerialized {
      get { alglib.dfserialize(randomForest, out var serialized); return serialized; }
      set { if (value != null) alglib.dfunserialize(value, out randomForest); }
    }

    [StorableConstructor]
    private RandomForestModelFull(StorableConstructorFlag _) : base(_) { }

    private RandomForestModelFull(RandomForestModelFull original, Cloner cloner) : base(original, cloner) {
      if (original.randomForest != null)
        randomForest = (alglib.decisionforest)original.randomForest.make_copy();
      NumberOfTrees = original.NumberOfTrees;

      // following fields are immutable so we don't need to clone them
      inputVariables = original.inputVariables;
      classValues = original.classValues;
    }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new RandomForestModelFull(this, cloner);
    }

    public RandomForestModelFull(alglib.decisionforest decisionForest, int nTrees, string targetVariable, IEnumerable<string> inputVariables, IEnumerable<double> classValues = null) : base(targetVariable) {
      this.name = ItemName;
      this.description = ItemDescription;

      randomForest = (alglib.decisionforest)decisionForest.make_copy();
      NumberOfTrees = nTrees;

      this.inputVariables = inputVariables.ToArray();

      //classValues are only use for classification models
      if (classValues == null) this.classValues = new double[0];
      else this.classValues = classValues.ToArray();
    }


    public IRegressionSolution CreateRegressionSolution(IRegressionProblemData problemData) {
      return new RandomForestRegressionSolution(this, new RegressionProblemData(problemData));
    }
    public override IClassificationSolution CreateClassificationSolution(IClassificationProblemData problemData) {
      return new RandomForestClassificationSolution(this, new ClassificationProblemData(problemData));
    }

    public bool IsProblemDataCompatible(IRegressionProblemData problemData, out string errorMessage) {
      return RegressionModel.IsProblemDataCompatible(this, problemData, out errorMessage);
    }

    public override bool IsProblemDataCompatible(IDataAnalysisProblemData problemData, out string errorMessage) {
      if (problemData == null) throw new ArgumentNullException("problemData", "The provided problemData is null.");

      var regressionProblemData = problemData as IRegressionProblemData;
      if (regressionProblemData != null)
        return IsProblemDataCompatible(regressionProblemData, out errorMessage);

      var classificationProblemData = problemData as IClassificationProblemData;
      if (classificationProblemData != null)
        return IsProblemDataCompatible(classificationProblemData, out errorMessage);

      throw new ArgumentException("The problem data is not compatible with this random forest. Instead a " + problemData.GetType().GetPrettyName() + " was provided.", "problemData");
    }

    public IEnumerable<double> GetEstimatedValues(IDataset dataset, IEnumerable<int> rows) {
      double[,] inputData = dataset.ToArray(inputVariables, rows);
      RandomForestUtil.AssertInputMatrix(inputData);

      int n = inputData.GetLength(0);
      int columns = inputData.GetLength(1);
      double[] x = new double[columns];
      double[] y = new double[1];

      alglib.dfcreatebuffer(randomForest, out var buf);
      for (int row = 0; row < n; row++) {
        for (int column = 0; column < columns; column++) {
          x[column] = inputData[row, column];
        }
        alglib.dftsprocess(randomForest, buf, x, ref y); // thread-safe process (as long as separate buffers are used)
        yield return y[0];
      }
    }
    public IEnumerable<double> GetEstimatedVariances(IDataset dataset, IEnumerable<int> rows) {
      double[,] inputData = dataset.ToArray(inputVariables, rows);
      RandomForestUtil.AssertInputMatrix(inputData);

      int n = inputData.GetLength(0);
      int columns = inputData.GetLength(1);
      double[] x = new double[columns];
      double[] ys = new double[this.randomForest.innerobj.ntrees];

      for (int row = 0; row < n; row++) {
        for (int column = 0; column < columns; column++) {
          x[column] = inputData[row, column];
        }
        lock (randomForest)
          alglib.dforest.dfprocessraw(randomForest.innerobj, x, ref ys, null);
        yield return ys.VariancePop();
      }
    }

    public override IEnumerable<double> GetEstimatedClassValues(IDataset dataset, IEnumerable<int> rows) {
      double[,] inputData = dataset.ToArray(inputVariables, rows);
      RandomForestUtil.AssertInputMatrix(inputData);

      int n = inputData.GetLength(0);
      int columns = inputData.GetLength(1);
      double[] x = new double[columns];
      double[] y = new double[NumClasses];

      alglib.dfcreatebuffer(randomForest, out var buf);
      for (int row = 0; row < n; row++) {
        for (int column = 0; column < columns; column++) {
          x[column] = inputData[row, column];
        }
        alglib.dftsprocess(randomForest, buf, x, ref y);
        // find class for with the largest probability value
        int maxProbClassIndex = 0;
        double maxProb = y[0];
        for (int i = 1; i < y.Length; i++) {
          if (maxProb < y[i]) {
            maxProb = y[i];
            maxProbClassIndex = i;
          }
        }
        yield return classValues[maxProbClassIndex];
      }
    }

    public ISymbolicExpressionTree ExtractTree(int treeIdx) {
      var rf = randomForest;
      // hoping that the internal representation of alglib is stable

      // TREE FORMAT
      // W[Offs]      -   size of sub-array (for the tree)
      //     node info:
      // W[K+0]       -   variable number        (-1 for leaf mode)
      // W[K+1]       -   threshold              (class/value for leaf node)
      // W[K+2]       -   ">=" branch index      (absent for leaf node)

      // skip irrelevant trees
      int offset = 0;
      for (int i = 0; i < treeIdx - 1; i++) {
        offset = offset + (int)Math.Round(rf.innerobj.trees[offset]);
      }

      var numSy = new Number();
      var varCondSy = new VariableCondition() { IgnoreSlope = true };

      var node = CreateRegressionTreeRec(rf.innerobj.trees, offset, offset + 1, numSy, varCondSy);

      var startNode = new StartSymbol().CreateTreeNode();
      startNode.AddSubtree(node);
      var root = new ProgramRootSymbol().CreateTreeNode();
      root.AddSubtree(startNode);
      return new SymbolicExpressionTree(root);
    }

    private ISymbolicExpressionTreeNode CreateRegressionTreeRec(double[] trees, int offset, int k, Number numSy, VariableCondition varCondSy) {

      // alglib source for evaluation of one tree (dfprocessinternal)
      // offs = 0
      //
      // Set pointer to the root
      //
      // k = offs + 1;
      // 
      // //
      // // Navigate through the tree
      // //
      // while (true) {
      //   if ((double)(df.trees[k]) == (double)(-1)) {
      //     if (df.nclasses == 1) {
      //       y[0] = y[0] + df.trees[k + 1];
      //     } else {
      //       idx = (int)Math.Round(df.trees[k + 1]);
      //       y[idx] = y[idx] + 1;
      //     }
      //     break;
      //   }
      //   if ((double)(x[(int)Math.Round(df.trees[k])]) < (double)(df.trees[k + 1])) {
      //     k = k + innernodewidth;
      //   } else {
      //     k = offs + (int)Math.Round(df.trees[k + 2]);
      //   }
      // }

      if (trees[k] == -1) {
        var numNode = (NumberTreeNode)numSy.CreateTreeNode();
        numNode.Value = trees[k + 1];
        return numNode;
      } else {
        var condNode = (VariableConditionTreeNode)varCondSy.CreateTreeNode();
        condNode.VariableName = inputVariables[(int)Math.Round(trees[k])];
        condNode.Threshold = trees[k + 1];
        condNode.Slope = double.PositiveInfinity;

        var left = CreateRegressionTreeRec(trees, offset, k + 3, numSy, varCondSy);
        var right = CreateRegressionTreeRec(trees, offset, offset + (int)Math.Round(trees[k + 2]), numSy, varCondSy);

        condNode.AddSubtree(left); // not 100% correct because interpreter uses: if(x <= thres) left() else right() and RF uses if(x < thres) left() else right() (see above)
        condNode.AddSubtree(right);
        return condNode;
      }
    }
  }
}