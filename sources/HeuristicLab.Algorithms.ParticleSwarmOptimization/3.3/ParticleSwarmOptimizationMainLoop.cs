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

using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Operators;
using HeuristicLab.Optimization;
using HeuristicLab.Optimization.Operators;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Algorithms.ParticleSwarmOptimization {
  [Item("ParticleSwarmOptimizationMainLoop", "An operator which represents the main loop of a particle swarm optimization algorithm.")]
  [StorableClass]
  public class ParticleSwarmOptimizationMainLoop : AlgorithmOperator {

    #region Parameter Properties
    public IValueLookupParameter<IRandom> RandomParameter {
      get { return (IValueLookupParameter<IRandom>)Parameters["Random"]; }
    }
    public IValueLookupParameter<IntValue> SwarmSizeParameter {
      get { return (IValueLookupParameter<IntValue>)Parameters["SwarmSize"]; }
    }
    public IValueLookupParameter<IntValue> MaxIterationsParameter {
      get { return (IValueLookupParameter<IntValue>)Parameters["MaxIterations"]; }
    }
    public IValueLookupParameter<IOperator> AnalyzerParameter {
      get { return (IValueLookupParameter<IOperator>)Parameters["Analyzer"]; }
    }
    public IValueLookupParameter<DoubleValue> InertiaParameter {
      get { return (IValueLookupParameter<DoubleValue>)Parameters["Inertia"]; }
    }
    public IValueLookupParameter<DoubleValue> PersonalBestAttractionParameter {
      get { return (IValueLookupParameter<DoubleValue>)Parameters["PersonalBestAttraction"]; }
    }
    public IValueLookupParameter<DoubleValue> NeighborBestAttractionParameter {
      get { return (IValueLookupParameter<DoubleValue>)Parameters["NeighborBestAttraction"]; }
    }
    public IValueLookupParameter<DoubleMatrix> VelocityBoundsParameter {
      get { return (IValueLookupParameter<DoubleMatrix>)Parameters["VelocityBounds"]; }
    }
    public IValueLookupParameter<IOperator> ParticleUpdaterParameter {
      get { return (IValueLookupParameter<IOperator>)Parameters["ParticleUpdater"]; }
    }
    public IValueLookupParameter<IOperator> TopologyUpdaterParameter {
      get { return (IValueLookupParameter<IOperator>)Parameters["TopologyUpdater"]; }
    }
    public IValueLookupParameter<IOperator> InertiaUpdaterParameter {
      get { return (IValueLookupParameter<IOperator>)Parameters["InertiaUpdater"]; }
    }
    public IValueLookupParameter<ResultCollection> ResultsParameter {
      get { return (IValueLookupParameter<ResultCollection>)Parameters["Results"]; }
    }
    public IValueLookupParameter<IOperator> EvaluatorParameter {
      get { return (IValueLookupParameter<IOperator>)Parameters["Evaluator"]; }
    }
    public ValueLookupParameter<ISwarmUpdater> SwarmUpdaterParameter {
      get { return (ValueLookupParameter<ISwarmUpdater>)Parameters["SwarmUpdater"]; }
    }
    #endregion

    public ParticleSwarmOptimizationMainLoop()
      : base() {
      Initialize();
    }

    [StorableConstructor]
    protected ParticleSwarmOptimizationMainLoop(bool deserializing) : base(deserializing) { }
    protected ParticleSwarmOptimizationMainLoop(ParticleSwarmOptimizationMainLoop original, Cloner cloner)
      : base(original, cloner) {
    }

    public override IDeepCloneable Clone(Cloner cloner) {
      return new ParticleSwarmOptimizationMainLoop(this, cloner);
    }

    private void Initialize() {
      #region Create parameters
      Parameters.Add(new ValueLookupParameter<IRandom>("Random", "A pseudo random number generator."));
      Parameters.Add(new ValueLookupParameter<IntValue>("SwarmSize", "Size of the particle swarm."));
      Parameters.Add(new ValueLookupParameter<IntValue>("MaxIterations", "Maximal number of iterations."));

      Parameters.Add(new ValueLookupParameter<IOperator>("Analyzer", "The operator used to analyze each generation."));

      Parameters.Add(new ValueLookupParameter<DoubleValue>("Inertia", "Inertia weight on a particle's movement (omega)."));
      Parameters.Add(new ValueLookupParameter<DoubleValue>("PersonalBestAttraction", "Weight for particle's pull towards its personal best soution (phi_p)."));
      Parameters.Add(new ValueLookupParameter<DoubleValue>("NeighborBestAttraction", "Weight for pull towards the neighborhood best solution or global best solution in case of a totally connected topology (phi_g)."));

      Parameters.Add(new ValueLookupParameter<IOperator>("ParticleUpdater", "Operator that calculates new position and velocity of a particle"));
      Parameters.Add(new ValueLookupParameter<IOperator>("TopologyUpdater", "Updates the neighborhood description vectors"));
      Parameters.Add(new ValueLookupParameter<IOperator>("InertiaUpdater", "Updates the omega parameter"));
      Parameters.Add(new ValueLookupParameter<IOperator>("Evaluator", "Evaluates a particle solution."));

      Parameters.Add(new ValueLookupParameter<ResultCollection>("Results", "The variable collection where results should be stored."));

      Parameters.Add(new ValueLookupParameter<ISwarmUpdater>("SwarmUpdater", "The encoding-specific swarm updater."));
      #endregion

      #region Create operators
      ResultsCollector resultsCollector = new ResultsCollector();
      Placeholder swarmUpdaterPlaceholer1 = new Placeholder();
      Placeholder evaluatorPlaceholder = new Placeholder();
      Placeholder analyzerPlaceholder = new Placeholder();
      UniformSubScopesProcessor uniformSubScopeProcessor = new UniformSubScopesProcessor();
      Placeholder particleUpdaterPlaceholder = new Placeholder();
      Placeholder topologyUpdaterPlaceholder = new Placeholder();
      UniformSubScopesProcessor uniformSubscopesProcessor2 = new UniformSubScopesProcessor();
      UniformSubScopesProcessor evaluationProcessor = new UniformSubScopesProcessor();
      NeighborUpdater neighborUpdater = new NeighborUpdater();
      Placeholder swarmUpdater = new Placeholder();
      IntCounter currentIterationCounter = new IntCounter();
      Comparator currentIterationComparator = new Comparator();
      ConditionalBranch conditionalBranch = new ConditionalBranch();
      Placeholder velocityBoundsUpdaterPlaceholder = new Placeholder();
      Placeholder inertiaUpdaterPlaceholder = new Placeholder();
      #endregion

      #region Create operator graph
      OperatorGraph.InitialOperator = resultsCollector;
      resultsCollector.CollectedValues.Add(new LookupParameter<IntValue>("Iterations", null, "CurrentIteration"));
      //resultsCollector.CollectedValues.Add(new LookupParameter<IntValue>("Current Inertia", null, "Inertia"));
      //resultsCollector.CollectedValues.Add(new LookupParameter<IntValue>("Evaluated Solutions", null, "EvaluatedSolutions"));
      resultsCollector.ResultsParameter.ActualName = "Results";
      resultsCollector.Successor = swarmUpdaterPlaceholer1;

      swarmUpdaterPlaceholer1.Name = "(Swarm Updater)";
      swarmUpdaterPlaceholer1.OperatorParameter.ActualName = SwarmUpdaterParameter.ActualName;
      swarmUpdaterPlaceholer1.Successor = analyzerPlaceholder;

      analyzerPlaceholder.Name = "(Analyzer)";
      analyzerPlaceholder.OperatorParameter.ActualName = AnalyzerParameter.Name;
      analyzerPlaceholder.Successor = uniformSubScopeProcessor;

      uniformSubScopeProcessor.Operator = particleUpdaterPlaceholder;
      uniformSubScopeProcessor.Successor = evaluationProcessor;

      particleUpdaterPlaceholder.Name = "(ParticleUpdater)";
      particleUpdaterPlaceholder.OperatorParameter.ActualName = ParticleUpdaterParameter.Name;

      evaluationProcessor.Parallel = new BoolValue(true);
      evaluationProcessor.Operator = evaluatorPlaceholder;
      evaluationProcessor.Successor = topologyUpdaterPlaceholder;

      evaluatorPlaceholder.Name = "(Evaluator)";
      evaluatorPlaceholder.OperatorParameter.ActualName = EvaluatorParameter.Name;

      topologyUpdaterPlaceholder.Name = "(TopologyUpdater)";
      topologyUpdaterPlaceholder.OperatorParameter.ActualName = TopologyUpdaterParameter.Name;
      topologyUpdaterPlaceholder.Successor = swarmUpdater;

      swarmUpdater.Name = "Swarm Updater";
      swarmUpdater.OperatorParameter.ActualName = SwarmUpdaterParameter.ActualName;
      swarmUpdater.Successor = inertiaUpdaterPlaceholder;

      inertiaUpdaterPlaceholder.Name = "(Inertia Updater)";
      inertiaUpdaterPlaceholder.OperatorParameter.ActualName = InertiaUpdaterParameter.ActualName;
      inertiaUpdaterPlaceholder.Successor = currentIterationCounter;

      currentIterationCounter.Name = "CurrentIteration++";
      currentIterationCounter.ValueParameter.ActualName = "CurrentIteration";
      currentIterationCounter.Successor = currentIterationComparator;

      currentIterationComparator.LeftSideParameter.ActualName = "CurrentIteration";
      currentIterationComparator.Comparison = new Comparison(ComparisonType.Less);
      currentIterationComparator.RightSideParameter.ActualName = "MaxIterations";
      currentIterationComparator.ResultParameter.ActualName = "ContinueIteration";
      currentIterationComparator.Successor = conditionalBranch;

      conditionalBranch.Name = "ContinueIteration?";
      conditionalBranch.ConditionParameter.ActualName = "ContinueIteration";
      conditionalBranch.TrueBranch = analyzerPlaceholder;
      #endregion
    }

    public override IOperation Apply() {
      if (this.ParticleUpdaterParameter.ActualValue == null)
        return null;
      return base.Apply();
    }
  }
}
