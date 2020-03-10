﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeuristicLab.JsonInterface {
  public interface IValueJsonItem : IJsonItem {
    object Value { get; set; }
    //IEnumerable<object> Range { get; set; }
  }

  public interface IValueJsonItem<T> : IValueJsonItem {
    new T Value { get; set; }
    //new IEnumerable<T> Range { get; set; }
  }
}
