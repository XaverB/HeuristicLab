﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeuristicLab.JsonInterface.OptimizerIntegration {

  public class DoubleMatrixValueVM : MatrixValueVM<double, DoubleMatrixJsonItem> {
    public override Type TargetedJsonItemType => typeof(DoubleMatrixJsonItem);
    public override UserControl Control =>
      new JsonItemDoubleMatrixValueControl(this);

    public override double[][] Value {
      get => ((DoubleMatrixJsonItem)Item).Value;
      set {
        ((DoubleMatrixJsonItem)Item).Value = value;
        OnPropertyChange(this, nameof(Value));
      }
    }

    protected override double MinTypeValue => double.MinValue;

    protected override double MaxTypeValue => double.MaxValue;
  }

  public abstract class MatrixValueVM<T, JsonItemType> : RangedValueBaseVM<T, JsonItemType>, IMatrixJsonItemVM
    where T : IComparable
    where JsonItemType : class, IMatrixJsonItem, IIntervalRestrictedJsonItem<T> {
    public abstract T[][] Value { get; set; }
    public bool RowsResizable {
      get => ((IMatrixJsonItem)Item).RowsResizable; 
      set {
        ((IMatrixJsonItem)Item).RowsResizable = value;
        OnPropertyChange(this, nameof(RowsResizable));
      }
    }

    public bool ColumnsResizable {
      get => ((IMatrixJsonItem)Item).ColumnsResizable;
      set {
        ((IMatrixJsonItem)Item).ColumnsResizable = value;
        OnPropertyChange(this, nameof(ColumnsResizable));
      }
    }

    public IEnumerable<string> RowNames {
      get => ((JsonItemType)Item).RowNames;
      set {
        ((JsonItemType)Item).RowNames = value;
        OnPropertyChange(this, nameof(RowNames));
      }
    }
    public IEnumerable<string> ColumnNames {
      get => ((JsonItemType)Item).ColumnNames;
      set {
        ((JsonItemType)Item).ColumnNames = value;
        OnPropertyChange(this, nameof(ColumnNames));
      }
    }

    public void SetCellValue(T data, int row, int col) {
      
      T[][] tmp = Value;
      
      // increase y
      if (row >= tmp.Length) { // increasing array
        T[][] newArr = new T[row + 1][];
        Array.Copy(tmp, 0, newArr, 0, tmp.Length);
        newArr[row] = new T[0];
        tmp = newArr;
      }

      // increase x
      for(int i = 0; i < tmp.Length; ++i) {
        if(col >= tmp[i].Length) {
          T[] newArr = new T[col + 1];
          Array.Copy(tmp[i], 0, newArr, 0, tmp[i].Length);
          tmp[i] = newArr;
        }
      }

      tmp[row][col] = data;
      Value = tmp;
    }
  }
}
