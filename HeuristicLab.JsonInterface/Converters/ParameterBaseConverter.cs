﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeuristicLab.Core;

namespace HeuristicLab.JsonInterface {
  public abstract class ParameterBaseConverter : BaseConverter {
    public override JsonItem ExtractData(IItem value) {
      IParameter param = value.Cast<IParameter>();
      JsonItem comp = ExtractData(param);
      return comp;
    }
    public abstract JsonItem ExtractData(IParameter value);

    public override void InjectData(IItem item, JsonItem data) => InjectData(item.Cast<IParameter>(), data);

    public abstract void InjectData(IParameter parameter, JsonItem data);
  }
}
