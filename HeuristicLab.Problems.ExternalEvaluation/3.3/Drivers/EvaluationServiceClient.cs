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
using HeuristicLab.Common;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Parameters;
using HeuristicLab.Persistence.Default.CompositeSerializers.Storable;

namespace HeuristicLab.Problems.ExternalEvaluation {
  [Item("EvaluationServiceClient", "An RPC client that evaluates a solution.")]
  [StorableClass]
  public class EvaluationServiceClient : ParameterizedNamedItem, IEvaluationServiceClient {
    public override bool CanChangeName { get { return false; } }
    public override bool CanChangeDescription { get { return false; } }

    public IValueParameter<IEvaluationChannel> ChannelParameter {
      get { return (IValueParameter<IEvaluationChannel>)Parameters["Channel"]; }
    }
    public IValueParameter<IntValue> RetryParameter {
      get { return (IValueParameter<IntValue>)Parameters["Retry"]; }
    }

    private IEvaluationChannel Channel {
      get { return ChannelParameter.Value; }
    }


    [StorableConstructor]
    protected EvaluationServiceClient(bool deserializing) : base(deserializing) { }
    protected EvaluationServiceClient(EvaluationServiceClient original, Cloner cloner) : base(original, cloner) { }
    public override IDeepCloneable Clone(Cloner cloner) {
      return new EvaluationServiceClient(this, cloner);
    }
    public EvaluationServiceClient()
      : base() {
      Parameters.Add(new ValueParameter<IEvaluationChannel>("Channel", "The channel over which to call the remote function."));
      Parameters.Add(new ValueParameter<IntValue>("Retry", "How many times the client should retry obtaining a quality in case there is an exception. Note that it immediately aborts when the channel cannot be opened.", new IntValue(10)));
    }

    #region IEvaluationServiceClient Members

    public QualityMessage Evaluate(SolutionMessage solution) {
      int tries = 0, maxTries = RetryParameter.Value.Value;
      bool success = false;
      QualityMessage result = null;
      while (!success) {
        try {
          tries++;
          CheckAndOpenChannel();
          Channel.Send(solution);
          result = (QualityMessage)Channel.Receive(QualityMessage.CreateBuilder());
          success = true;
        }
        catch (InvalidOperationException) {
          throw;
        }
        catch {
          if (tries >= maxTries)
            throw;
        }
      }
      return result;
    }

    public void EvaluateAsync(SolutionMessage solution, Action<QualityMessage> callback) {
      int tries = 0, maxTries = RetryParameter.Value.Value;
      bool success = false;
      while (!success) {
        try {
          tries++;
          CheckAndOpenChannel();
          Channel.Send(solution);
          success = true;
        }
        catch (InvalidOperationException) {
          throw;
        }
        catch {
          if (tries >= maxTries)
            throw;
        }
      }
      System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ReceiveAsync), callback);
    }

    #endregion

    private void CheckAndOpenChannel() {
      if (Channel == null) throw new InvalidOperationException(Name + ": The channel is not defined.");
      if (!Channel.IsInitialized) {
        try {
          Channel.Open();
        }
        catch (Exception e) {
          throw new InvalidOperationException(Name + ": The channel could not be opened.", e);
        }
      }
    }

    private void ReceiveAsync(object callback) {
      QualityMessage message = null;
      try {
        message = (QualityMessage)Channel.Receive(QualityMessage.CreateBuilder());
      }
      catch { }
      ((Action<QualityMessage>)callback).Invoke(message);
    }
  }
}
