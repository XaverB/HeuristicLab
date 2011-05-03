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
using System.Drawing;
using System.Linq;
using HeuristicLab.Collections;
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Optimization;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;
using HeuristicLab.Problems.DataAnalysis;
using HeuristicLab.Problems.DataAnalysis.Symbolic;

namespace HeuristicLab.Algorithms.DataAnalysis {
  [Item("Cross Validation", "Cross Validation wrapper for data analysis algorithms.")]
  [Creatable("Data Analysis")]
  [StorableClass]
  public sealed class CrossValidation : ParameterizedNamedItem, IAlgorithm, IStorableContent {
    public CrossValidation()
      : base() {
      name = ItemName;
      description = ItemDescription;

      executionState = ExecutionState.Stopped;
      runs = new RunCollection();
      runsCounter = 0;

      algorithm = null;
      clonedAlgorithms = new ItemCollection<IAlgorithm>();
      results = new ResultCollection();

      folds = new IntValue(2);
      numberOfWorkers = new IntValue(1);
      samplesStart = new IntValue(0);
      samplesEnd = new IntValue(0);
      storeAlgorithmInEachRun = false;

      RegisterEvents();
      if (Algorithm != null) RegisterAlgorithmEvents();
    }

    public string Filename { get; set; }

    #region persistence and cloning
    [StorableConstructor]
    private CrossValidation(bool deserializing)
      : base(deserializing) {
    }
    [StorableHook(HookType.AfterDeserialization)]
    private void AfterDeserialization() {
      RegisterEvents();
      if (Algorithm != null) RegisterAlgorithmEvents();
    }

    private CrossValidation(CrossValidation original, Cloner cloner)
      : base(original, cloner) {
      executionState = original.executionState;
      storeAlgorithmInEachRun = original.storeAlgorithmInEachRun;
      runs = cloner.Clone(original.runs);
      runsCounter = original.runsCounter;
      algorithm = cloner.Clone(original.algorithm);
      clonedAlgorithms = cloner.Clone(original.clonedAlgorithms);
      results = cloner.Clone(original.results);

      folds = cloner.Clone(original.folds);
      numberOfWorkers = cloner.Clone(original.numberOfWorkers);
      samplesStart = cloner.Clone(original.samplesStart);
      samplesEnd = cloner.Clone(original.samplesEnd);
      RegisterEvents();
      if (Algorithm != null) RegisterAlgorithmEvents();
    }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new CrossValidation(this, cloner);
    }

    #endregion

    #region properties
    [Storable]
    private IAlgorithm algorithm;
    public IAlgorithm Algorithm {
      get { return algorithm; }
      set {
        if (ExecutionState != ExecutionState.Prepared && ExecutionState != ExecutionState.Stopped)
          throw new InvalidOperationException("Changing the algorithm is only allowed if the CrossValidation is stopped or prepared.");
        if (algorithm != value) {
          if (value != null && value.Problem != null && !(value.Problem is IDataAnalysisProblem))
            throw new ArgumentException("Only algorithms with a DataAnalysisProblem could be used for the cross validation.");
          if (algorithm != null) DeregisterAlgorithmEvents();
          algorithm = value;
          Parameters.Clear();

          if (algorithm != null) {
            algorithm.StoreAlgorithmInEachRun = false;
            RegisterAlgorithmEvents();
            algorithm.Prepare(true);
            Parameters.AddRange(algorithm.Parameters);
          }
          OnAlgorithmChanged();
          if (algorithm != null) OnProblemChanged();
          Prepare();
        }
      }
    }


    [Storable]
    private IDataAnalysisProblem problem;
    public IDataAnalysisProblem Problem {
      get {
        if (algorithm == null)
          return null;
        return (IDataAnalysisProblem)algorithm.Problem;
      }
      set {
        if (ExecutionState != ExecutionState.Prepared && ExecutionState != ExecutionState.Stopped)
          throw new InvalidOperationException("Changing the problem is only allowed if the CrossValidation is stopped or prepared.");
        if (algorithm == null) throw new ArgumentNullException("Could not set a problem before an algorithm was set.");
        algorithm.Problem = value;
        problem = value;
      }
    }

    IProblem IAlgorithm.Problem {
      get { return Problem; }
      set {
        if (value != null && !ProblemType.IsInstanceOfType(value))
          throw new ArgumentException("Only DataAnalysisProblems could be used for the cross validation.");
        Problem = (IDataAnalysisProblem)value;
      }
    }
    public Type ProblemType {
      get { return typeof(IDataAnalysisProblem); }
    }

    [Storable]
    private ItemCollection<IAlgorithm> clonedAlgorithms;

    public IEnumerable<IOptimizer> NestedOptimizers {
      get {
        if (Algorithm == null) yield break;
        yield return Algorithm;
      }
    }

    [Storable]
    private ResultCollection results;
    public ResultCollection Results {
      get { return results; }
    }

    [Storable]
    private IntValue folds;
    public IntValue Folds {
      get { return folds; }
    }
    [Storable]
    private IntValue samplesStart;
    public IntValue SamplesStart {
      get { return samplesStart; }
    }
    [Storable]
    private IntValue samplesEnd;
    public IntValue SamplesEnd {
      get { return samplesEnd; }
    }
    [Storable]
    private IntValue numberOfWorkers;
    public IntValue NumberOfWorkers {
      get { return numberOfWorkers; }
    }

    [Storable]
    private bool storeAlgorithmInEachRun;
    public bool StoreAlgorithmInEachRun {
      get { return storeAlgorithmInEachRun; }
      set {
        if (storeAlgorithmInEachRun != value) {
          storeAlgorithmInEachRun = value;
          OnStoreAlgorithmInEachRunChanged();
        }
      }
    }

    [Storable]
    private int runsCounter;
    [Storable]
    private RunCollection runs;
    public RunCollection Runs {
      get { return runs; }
    }

    [Storable]
    private ExecutionState executionState;
    public ExecutionState ExecutionState {
      get { return executionState; }
      private set {
        if (executionState != value) {
          executionState = value;
          OnExecutionStateChanged();
          OnItemImageChanged();
        }
      }
    }
    public override Image ItemImage {
      get {
        if (ExecutionState == ExecutionState.Prepared) return HeuristicLab.Common.Resources.VSImageLibrary.ExecutablePrepared;
        else if (ExecutionState == ExecutionState.Started) return HeuristicLab.Common.Resources.VSImageLibrary.ExecutableStarted;
        else if (ExecutionState == ExecutionState.Paused) return HeuristicLab.Common.Resources.VSImageLibrary.ExecutablePaused;
        else if (ExecutionState == ExecutionState.Stopped) return HeuristicLab.Common.Resources.VSImageLibrary.ExecutableStopped;
        else return HeuristicLab.Common.Resources.VSImageLibrary.Event;
      }
    }

    public TimeSpan ExecutionTime {
      get {
        if (ExecutionState != ExecutionState.Prepared)
          return TimeSpan.FromMilliseconds(clonedAlgorithms.Select(x => x.ExecutionTime.TotalMilliseconds).Sum());
        return TimeSpan.Zero;
      }
    }
    #endregion

    public void Prepare() {
      if (ExecutionState == ExecutionState.Started)
        throw new InvalidOperationException(string.Format("Prepare not allowed in execution state \"{0}\".", ExecutionState));
      results.Clear();
      clonedAlgorithms.Clear();
      if (Algorithm != null) {
        Algorithm.Prepare();
        if (Algorithm.ExecutionState == ExecutionState.Prepared) OnPrepared();
      }
    }
    public void Prepare(bool clearRuns) {
      if (clearRuns) runs.Clear();
      Prepare();
    }

    private bool startPending;
    public void Start() {
      if ((ExecutionState != ExecutionState.Prepared) && (ExecutionState != ExecutionState.Paused))
        throw new InvalidOperationException(string.Format("Start not allowed in execution state \"{0}\".", ExecutionState));

      if (Algorithm != null && !startPending) {
        startPending = true;
        //create cloned algorithms
        if (clonedAlgorithms.Count == 0) {
          int testSamplesCount = (SamplesEnd.Value - SamplesStart.Value) / Folds.Value;

          for (int i = 0; i < Folds.Value; i++) {
            IAlgorithm clonedAlgorithm = (IAlgorithm)algorithm.Clone();
            clonedAlgorithm.Name = algorithm.Name + " Fold " + i;
            IDataAnalysisProblem problem = clonedAlgorithm.Problem as IDataAnalysisProblem;
            ISymbolicDataAnalysisProblem symbolicProblem = problem as ISymbolicDataAnalysisProblem;

            int testStart = (i * testSamplesCount) + SamplesStart.Value;
            int testEnd = (i + 1) == Folds.Value ? SamplesEnd.Value : (i + 1) * testSamplesCount + SamplesStart.Value;

            problem.ProblemData.TestPartition.Start = testStart;
            problem.ProblemData.TestPartition.End = testEnd;
            DataAnalysisProblemData problemData = problem.ProblemData as DataAnalysisProblemData;
            if (problemData != null) {
              problemData.TrainingPartitionParameter.Hidden = false;
              problemData.TestPartitionParameter.Hidden = false;
            }

            if (symbolicProblem != null) {
              symbolicProblem.FitnessCalculationPartition.Start = SamplesStart.Value;
              symbolicProblem.FitnessCalculationPartition.End = SamplesEnd.Value;
            }

            clonedAlgorithms.Add(clonedAlgorithm);
          }
        }

        //start prepared or paused cloned algorithms
        int startedAlgorithms = 0;
        foreach (IAlgorithm clonedAlgorithm in clonedAlgorithms) {
          if (startedAlgorithms < NumberOfWorkers.Value) {
            if (clonedAlgorithm.ExecutionState == ExecutionState.Prepared ||
                clonedAlgorithm.ExecutionState == ExecutionState.Paused) {
              clonedAlgorithm.Start();
              startedAlgorithms++;
            }
          }
        }
        OnStarted();
      }
    }

    private bool pausePending;
    public void Pause() {
      if (ExecutionState != ExecutionState.Started)
        throw new InvalidOperationException(string.Format("Pause not allowed in execution state \"{0}\".", ExecutionState));
      if (!pausePending) {
        pausePending = true;
        if (!startPending) PauseAllClonedAlgorithms();
      }
    }
    private void PauseAllClonedAlgorithms() {
      foreach (IAlgorithm clonedAlgorithm in clonedAlgorithms) {
        if (clonedAlgorithm.ExecutionState == ExecutionState.Started)
          clonedAlgorithm.Pause();
      }
    }

    private bool stopPending;
    public void Stop() {
      if ((ExecutionState != ExecutionState.Started) && (ExecutionState != ExecutionState.Paused))
        throw new InvalidOperationException(string.Format("Stop not allowed in execution state \"{0}\".",
                                                          ExecutionState));
      if (!stopPending) {
        stopPending = true;
        if (!startPending) StopAllClonedAlgorithms();
      }
    }
    private void StopAllClonedAlgorithms() {
      foreach (IAlgorithm clonedAlgorithm in clonedAlgorithms) {
        if (clonedAlgorithm.ExecutionState == ExecutionState.Started ||
            clonedAlgorithm.ExecutionState == ExecutionState.Paused)
          clonedAlgorithm.Stop();
      }
    }

    #region collect parameters and results
    public override void CollectParameterValues(IDictionary<string, IItem> values) {
      values.Add("Algorithm Name", new StringValue(Name));
      values.Add("Algorithm Type", new StringValue(GetType().GetPrettyName()));
      values.Add("Folds", new IntValue(Folds.Value));

      if (algorithm != null) {
        values.Add("CrossValidation Algorithm Name", new StringValue(Algorithm.Name));
        values.Add("CrossValidation Algorithm Type", new StringValue(Algorithm.GetType().GetPrettyName()));
        base.CollectParameterValues(values);
      }
      if (Problem != null) {
        values.Add("Problem Name", new StringValue(Problem.Name));
        values.Add("Problem Type", new StringValue(Problem.GetType().GetPrettyName()));
        Problem.CollectParameterValues(values);
      }
    }

    public void CollectResultValues(IDictionary<string, IItem> results) {
      Dictionary<string, List<double>> resultValues = new Dictionary<string, List<double>>();
      IEnumerable<IRun> runs = clonedAlgorithms.Select(alg => alg.Runs.FirstOrDefault()).Where(run => run != null);
      IEnumerable<KeyValuePair<string, IItem>> resultCollections = runs.Where(x => x != null).SelectMany(x => x.Results).ToList();

      foreach (IResult result in ExtractAndAggregateResults<IntValue>(resultCollections))
        results.Add(result.Name, result.Value);
      foreach (IResult result in ExtractAndAggregateResults<DoubleValue>(resultCollections))
        results.Add(result.Name, result.Value);
      foreach (IResult result in ExtractAndAggregateResults<PercentValue>(resultCollections))
        results.Add(result.Name, result.Value);

      results.Add("Execution Time", new TimeSpanValue(this.ExecutionTime));
      results.Add("CrossValidation Folds", new RunCollection(runs));
    }

    private static IEnumerable<IResult> ExtractAndAggregateResults<T>(IEnumerable<KeyValuePair<string, IItem>> results)
  where T : class, IItem, new() {
      Dictionary<string, List<double>> resultValues = new Dictionary<string, List<double>>();
      foreach (var resultValue in results.Where(r => r.Value.GetType() == typeof(T))) {
        if (!resultValues.ContainsKey(resultValue.Key))
          resultValues[resultValue.Key] = new List<double>();
        resultValues[resultValue.Key].Add(ConvertToDouble(resultValue.Value));
      }

      DoubleValue doubleValue;
      if (typeof(T) == typeof(PercentValue))
        doubleValue = new PercentValue();
      else if (typeof(T) == typeof(DoubleValue))
        doubleValue = new DoubleValue();
      else if (typeof(T) == typeof(IntValue))
        doubleValue = new DoubleValue();
      else
        throw new NotSupportedException();

      List<IResult> aggregatedResults = new List<IResult>();
      foreach (KeyValuePair<string, List<double>> resultValue in resultValues) {
        doubleValue.Value = resultValue.Value.Average();
        aggregatedResults.Add(new Result(resultValue.Key, (IItem)doubleValue.Clone()));
        doubleValue.Value = resultValue.Value.StandardDeviation();
        aggregatedResults.Add(new Result(resultValue.Key + " StdDev", (IItem)doubleValue.Clone()));
      }
      return aggregatedResults;
    }

    private static double ConvertToDouble(IItem item) {
      if (item is DoubleValue) return ((DoubleValue)item).Value;
      else if (item is IntValue) return ((IntValue)item).Value;
      else throw new NotSupportedException("Could not convert any item type to double");
    }
    #endregion

    #region events
    private void RegisterEvents() {
      Folds.ValueChanged += new EventHandler(Folds_ValueChanged);
      SamplesStart.ValueChanged += new EventHandler(SamplesStart_ValueChanged);
      SamplesEnd.ValueChanged += new EventHandler(SamplesEnd_ValueChanged);
      RegisterClonedAlgorithmsEvents();
    }
    private void Folds_ValueChanged(object sender, EventArgs e) {
      if (ExecutionState != ExecutionState.Prepared)
        throw new InvalidOperationException("Can not change number of folds if the execution state is not prepared.");
    }
    private void SamplesStart_ValueChanged(object sender, EventArgs e) {
      if (Problem != null) Problem.ProblemData.TrainingPartition.Start = SamplesStart.Value;
    }
    private void SamplesEnd_ValueChanged(object sender, EventArgs e) {
      if (Problem != null) Problem.ProblemData.TrainingPartition.End = SamplesEnd.Value;
    }

    #region template algorithms events
    public event EventHandler AlgorithmChanged;
    private void OnAlgorithmChanged() {
      EventHandler handler = AlgorithmChanged;
      if (handler != null) handler(this, EventArgs.Empty);
      OnProblemChanged();
      if (Problem == null) ExecutionState = ExecutionState.Stopped;
    }
    private void RegisterAlgorithmEvents() {
      algorithm.ProblemChanged += new EventHandler(Algorithm_ProblemChanged);
      algorithm.ExecutionStateChanged += new EventHandler(Algorithm_ExecutionStateChanged);
    }
    private void DeregisterAlgorithmEvents() {
      algorithm.ProblemChanged -= new EventHandler(Algorithm_ProblemChanged);
      algorithm.ExecutionStateChanged -= new EventHandler(Algorithm_ExecutionStateChanged);
    }
    private void Algorithm_ProblemChanged(object sender, EventArgs e) {
      if (algorithm.Problem != null && !(algorithm.Problem is IDataAnalysisProblem)) {
        algorithm.Problem = problem;
        throw new ArgumentException("A cross validation algorithm can only contain DataAnalysisProblems.");
      }
      problem = (IDataAnalysisProblem)algorithm.Problem;
      OnProblemChanged();
    }
    public event EventHandler ProblemChanged;
    private void OnProblemChanged() {
      EventHandler handler = ProblemChanged;
      if (handler != null) handler(this, EventArgs.Empty);

      SamplesStart.Value = 0;
      if (Problem != null) {
        Problem.ProblemDataChanged += (object sender, EventArgs e) => OnProblemChanged();
        SamplesEnd.Value = Problem.ProblemData.Dataset.Rows;

        DataAnalysisProblemData problemData = Problem.ProblemData as DataAnalysisProblemData;
        if (problemData != null) {
          problemData.TrainingPartitionParameter.Hidden = true;
          problemData.TestPartitionParameter.Hidden = true;
        }
        ISymbolicDataAnalysisProblem symbolicProblem = Problem as ISymbolicDataAnalysisProblem;
        if (symbolicProblem != null) {
          symbolicProblem.FitnessCalculationPartitionParameter.Hidden = true;
          symbolicProblem.FitnessCalculationPartition.Start = SamplesStart.Value;
          symbolicProblem.FitnessCalculationPartition.End = SamplesEnd.Value;
          symbolicProblem.ValidationPartitionParameter.Hidden = true;
          symbolicProblem.ValidationPartition.Start = 0;
          symbolicProblem.ValidationPartition.End = 0;
        }
      } else
        SamplesEnd.Value = 0;
    }

    private void Algorithm_ExecutionStateChanged(object sender, EventArgs e) {
      switch (Algorithm.ExecutionState) {
        case ExecutionState.Prepared: OnPrepared();
          break;
        case ExecutionState.Started: throw new InvalidOperationException("Algorithm template can not be started.");
        case ExecutionState.Paused: throw new InvalidOperationException("Algorithm template can not be paused.");
        case ExecutionState.Stopped: OnStopped();
          break;
      }
    }
    #endregion

    #region clonedAlgorithms events
    private void RegisterClonedAlgorithmsEvents() {
      clonedAlgorithms.ItemsAdded += new CollectionItemsChangedEventHandler<IAlgorithm>(ClonedAlgorithms_ItemsAdded);
      clonedAlgorithms.ItemsRemoved += new CollectionItemsChangedEventHandler<IAlgorithm>(ClonedAlgorithms_ItemsRemoved);
      clonedAlgorithms.CollectionReset += new CollectionItemsChangedEventHandler<IAlgorithm>(ClonedAlgorithms_CollectionReset);
      foreach (IAlgorithm algorithm in clonedAlgorithms)
        RegisterClonedAlgorithmEvents(algorithm);
    }
    private void DeregisterClonedAlgorithmsEvents() {
      clonedAlgorithms.ItemsAdded -= new CollectionItemsChangedEventHandler<IAlgorithm>(ClonedAlgorithms_ItemsAdded);
      clonedAlgorithms.ItemsRemoved -= new CollectionItemsChangedEventHandler<IAlgorithm>(ClonedAlgorithms_ItemsRemoved);
      clonedAlgorithms.CollectionReset -= new CollectionItemsChangedEventHandler<IAlgorithm>(ClonedAlgorithms_CollectionReset);
      foreach (IAlgorithm algorithm in clonedAlgorithms)
        DeregisterClonedAlgorithmEvents(algorithm);
    }
    private void ClonedAlgorithms_ItemsAdded(object sender, CollectionItemsChangedEventArgs<IAlgorithm> e) {
      foreach (IAlgorithm algorithm in e.Items)
        RegisterClonedAlgorithmEvents(algorithm);
    }
    private void ClonedAlgorithms_ItemsRemoved(object sender, CollectionItemsChangedEventArgs<IAlgorithm> e) {
      foreach (IAlgorithm algorithm in e.Items)
        DeregisterClonedAlgorithmEvents(algorithm);
    }
    private void ClonedAlgorithms_CollectionReset(object sender, CollectionItemsChangedEventArgs<IAlgorithm> e) {
      foreach (IAlgorithm algorithm in e.OldItems)
        DeregisterClonedAlgorithmEvents(algorithm);
      foreach (IAlgorithm algorithm in e.Items)
        RegisterClonedAlgorithmEvents(algorithm);
    }
    private void RegisterClonedAlgorithmEvents(IAlgorithm algorithm) {
      algorithm.ExceptionOccurred += new EventHandler<EventArgs<Exception>>(ClonedAlgorithm_ExceptionOccurred);
      algorithm.ExecutionTimeChanged += new EventHandler(ClonedAlgorithm_ExecutionTimeChanged);
      algorithm.Started += new EventHandler(ClonedAlgorithm_Started);
      algorithm.Paused += new EventHandler(ClonedAlgorithm_Paused);
      algorithm.Stopped += new EventHandler(ClonedAlgorithm_Stopped);
    }
    private void DeregisterClonedAlgorithmEvents(IAlgorithm algorithm) {
      algorithm.ExceptionOccurred -= new EventHandler<EventArgs<Exception>>(ClonedAlgorithm_ExceptionOccurred);
      algorithm.ExecutionTimeChanged -= new EventHandler(ClonedAlgorithm_ExecutionTimeChanged);
      algorithm.Started -= new EventHandler(ClonedAlgorithm_Started);
      algorithm.Paused -= new EventHandler(ClonedAlgorithm_Paused);
      algorithm.Stopped -= new EventHandler(ClonedAlgorithm_Stopped);
    }
    private void ClonedAlgorithm_ExceptionOccurred(object sender, EventArgs<Exception> e) {
      OnExceptionOccurred(e.Value);
    }
    private void ClonedAlgorithm_ExecutionTimeChanged(object sender, EventArgs e) {
      OnExecutionTimeChanged();
    }

    private readonly object locker = new object();
    private void ClonedAlgorithm_Started(object sender, EventArgs e) {
      lock (locker) {
        IAlgorithm algorithm = sender as IAlgorithm;
        if (algorithm != null && !results.ContainsKey(algorithm.Name))
          results.Add(new Result(algorithm.Name, "Contains results for the specific fold.", algorithm.Results));

        if (startPending) {
          int startedAlgorithms = clonedAlgorithms.Count(alg => alg.ExecutionState == ExecutionState.Started);
          if (startedAlgorithms == NumberOfWorkers.Value ||
             clonedAlgorithms.All(alg => alg.ExecutionState != ExecutionState.Prepared))
            startPending = false;

          if (pausePending) PauseAllClonedAlgorithms();
          if (stopPending) StopAllClonedAlgorithms();
        }
      }
    }

    private void ClonedAlgorithm_Paused(object sender, EventArgs e) {
      lock (locker) {
        if (pausePending && clonedAlgorithms.All(alg => alg.ExecutionState != ExecutionState.Started))
          OnPaused();
      }
    }

    private void ClonedAlgorithm_Stopped(object sender, EventArgs e) {
      lock (locker) {
        if (!stopPending && ExecutionState == ExecutionState.Started) {
          IAlgorithm preparedAlgorithm = clonedAlgorithms.Where(alg => alg.ExecutionState == ExecutionState.Prepared ||
                                                                       alg.ExecutionState == ExecutionState.Paused).FirstOrDefault();
          if (preparedAlgorithm != null) preparedAlgorithm.Start();
        }
        if (ExecutionState != ExecutionState.Stopped) {
          if (clonedAlgorithms.All(alg => alg.ExecutionState == ExecutionState.Stopped))
            OnStopped();
          else if (stopPending &&
                   clonedAlgorithms.All(
                     alg => alg.ExecutionState == ExecutionState.Prepared || alg.ExecutionState == ExecutionState.Stopped))
            OnStopped();
        }
      }
    }
    #endregion
    #endregion

    #region event firing
    public event EventHandler ExecutionStateChanged;
    private void OnExecutionStateChanged() {
      EventHandler handler = ExecutionStateChanged;
      if (handler != null) handler(this, EventArgs.Empty);
    }
    public event EventHandler ExecutionTimeChanged;
    private void OnExecutionTimeChanged() {
      EventHandler handler = ExecutionTimeChanged;
      if (handler != null) handler(this, EventArgs.Empty);
    }
    public event EventHandler Prepared;
    private void OnPrepared() {
      ExecutionState = ExecutionState.Prepared;
      EventHandler handler = Prepared;
      if (handler != null) handler(this, EventArgs.Empty);
      OnExecutionTimeChanged();
    }
    public event EventHandler Started;
    private void OnStarted() {
      startPending = false;
      ExecutionState = ExecutionState.Started;
      EventHandler handler = Started;
      if (handler != null) handler(this, EventArgs.Empty);
    }
    public event EventHandler Paused;
    private void OnPaused() {
      pausePending = false;
      ExecutionState = ExecutionState.Paused;
      EventHandler handler = Paused;
      if (handler != null) handler(this, EventArgs.Empty);
    }
    public event EventHandler Stopped;
    private void OnStopped() {
      stopPending = false;
      Dictionary<string, IItem> collectedResults = new Dictionary<string, IItem>();
      CollectResultValues(collectedResults);
      results.AddRange(collectedResults.Select(x => new Result(x.Key, x.Value)).Cast<IResult>().ToArray());
      runsCounter++;
      runs.Add(new Run(string.Format("{0} Run {1}", Name, runsCounter), this));
      ExecutionState = ExecutionState.Stopped;
      EventHandler handler = Stopped;
      if (handler != null) handler(this, EventArgs.Empty);
    }
    public event EventHandler<EventArgs<Exception>> ExceptionOccurred;
    private void OnExceptionOccurred(Exception exception) {
      EventHandler<EventArgs<Exception>> handler = ExceptionOccurred;
      if (handler != null) handler(this, new EventArgs<Exception>(exception));
    }
    public event EventHandler StoreAlgorithmInEachRunChanged;
    private void OnStoreAlgorithmInEachRunChanged() {
      EventHandler handler = StoreAlgorithmInEachRunChanged;
      if (handler != null) handler(this, EventArgs.Empty);
    }
    #endregion
  }
}
