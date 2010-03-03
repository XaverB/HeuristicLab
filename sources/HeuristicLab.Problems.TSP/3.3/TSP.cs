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

using System;
using System.Drawing;
using System.Linq;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Encodings.Permutation;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.PluginInfrastructure;

namespace HeuristicLab.Problems.TSP {
  [Item("TSP", "Represents a symmetric Traveling Salesman Problem.")]
  [Creatable("Problems")]
  [EmptyStorableClass]
  public sealed class TSP : ParameterizedNamedItem, ISingleObjectiveProblem {
    public override Image ItemImage {
      get { return HeuristicLab.Common.Resources.VS2008ImageLibrary.Type; }
    }

    private IValueParameter<BoolData> MaximizationParameter {
      get { return (IValueParameter<BoolData>)Parameters["Maximization"]; }
    }
    public IValueParameter<DoubleMatrixData> CoordinatesParameter {
      get { return (IValueParameter<DoubleMatrixData>)Parameters["Coordinates"]; }
    }
    public IValueParameter<DoubleData> BestKnownQualityParameter {
      get { return (IValueParameter<DoubleData>)Parameters["BestKnownQuality"]; }
    }
    public IValueParameter<IPermutationCreator> SolutionCreatorParameter {
      get { return (IValueParameter<IPermutationCreator>)Parameters["SolutionCreator"]; }
    }
    public IValueParameter<ITSPEvaluator> EvaluatorParameter {
      get { return (IValueParameter<ITSPEvaluator>)Parameters["Evaluator"]; }
    }

    public BoolData Maximization {
      get { return MaximizationParameter.Value; }
    }
    public ISolutionCreator SolutionCreator {
      get { return SolutionCreatorParameter.Value; }
    }
    public ISingleObjectiveEvaluator Evaluator {
      get { return EvaluatorParameter.Value; }
    }
    IEvaluator IProblem.Evaluator {
      get { return EvaluatorParameter.Value; }
    }
    private OperatorSet operators;
    public OperatorSet Operators {
      get { return operators; }
    }

    public TSP()
      : base() {
      RandomPermutationCreator creator = new RandomPermutationCreator();
      TSPRoundedEuclideanPathEvaluator evaluator = new TSPRoundedEuclideanPathEvaluator();

      Parameters.Add(new ValueParameter<BoolData>("Maximization", "Set to false as the Traveling Salesman Problem is a minimization problem.", new BoolData(false)));
      Parameters.Add(new ValueParameter<DoubleMatrixData>("Coordinates", "The x- and y-Coordinates of the cities.", new DoubleMatrixData(0, 0)));
      Parameters.Add(new ValueParameter<IPermutationCreator>("SolutionCreator", "The operator which should be used to create new TSP solutions.", creator));
      Parameters.Add(new ValueParameter<ITSPEvaluator>("Evaluator", "The operator which should be used to evaluate TSP solutions.", evaluator));
      Parameters.Add(new OptionalValueParameter<DoubleData>("BestKnownQuality", "The quality of the best known solution of this TSP instance."));

      creator.PermutationParameter.ActualName = "TSPTour";
      creator.LengthParameter.Value = new IntData(0);
      evaluator.CoordinatesParameter.ActualName = CoordinatesParameter.Name;
      evaluator.PermutationParameter.ActualName = creator.PermutationParameter.ActualName;
      evaluator.QualityParameter.ActualName = "TSPTourLength";

      MaximizationParameter.ValueChanged += new EventHandler(MaximizationParameter_ValueChanged);
      SolutionCreatorParameter.ValueChanged += new EventHandler(SolutionCreatorParameter_ValueChanged);
      EvaluatorParameter.ValueChanged += new EventHandler(EvaluatorParameter_ValueChanged);

      operators = new OperatorSet();
      if (ApplicationManager.Manager != null) {
        var ops = ApplicationManager.Manager.GetInstances<IPermutationOperator>();
        foreach (IPermutationCrossover op in ops.OfType<IPermutationCrossover>()) {
          op.ParentsParameter.ActualName = creator.PermutationParameter.ActualName;
          op.ChildParameter.ActualName = creator.PermutationParameter.ActualName;
        }
        foreach (IPermutationManipulator op in ops.OfType<IPermutationManipulator>()) {
          op.PermutationParameter.ActualName = creator.PermutationParameter.ActualName;
        }
        foreach (IPermutationOperator op in ops)
          operators.Add(op);
      }
    }

    public void ImportFromTSPLIB(string filename) {
      TSPLIBParser parser = new TSPLIBParser(filename);
      parser.Parse();
      CoordinatesParameter.Value = new DoubleMatrixData(parser.Vertices);
      int cities = CoordinatesParameter.Value.Rows;
      SolutionCreatorParameter.Value.LengthParameter.Value = new IntData(cities);
    }

    private void MaximizationParameter_ValueChanged(object sender, EventArgs e) {
      OnMaximizationChanged();
    }
    private void SolutionCreatorParameter_ValueChanged(object sender, EventArgs e) {
      OnSolutionCreatorChanged();
    }
    private void EvaluatorParameter_ValueChanged(object sender, EventArgs e) {
      OnEvaluatorChanged();
    }

    public event EventHandler MaximizationChanged;
    private void OnMaximizationChanged() {
      if (MaximizationChanged != null)
        MaximizationChanged(this, EventArgs.Empty);
    }
    public event EventHandler SolutionCreatorChanged;
    private void OnSolutionCreatorChanged() {
      if (SolutionCreatorChanged != null)
        SolutionCreatorChanged(this, EventArgs.Empty);
    }
    public event EventHandler EvaluatorChanged;
    private void OnEvaluatorChanged() {
      if (EvaluatorChanged != null)
        EvaluatorChanged(this, EventArgs.Empty);
    }
  }
}
