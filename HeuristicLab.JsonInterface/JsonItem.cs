﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeuristicLab.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HeuristicLab.JsonInterface {
  /// <summary>
  /// Main data class for json interface.
  /// </summary>
  public class JsonItem {
    public class JsonItemValidator {
      private IDictionary<int, bool> Cache = new Dictionary<int, bool>();
      private JsonItem Root { get; set; }
      public JsonItemValidator(JsonItem root) {
        Root = root;
      }

      public bool Validate(ref IList<JsonItem> faultyItems) {
        faultyItems = new List<JsonItem>();
        return ValidateHelper(Root, ref faultyItems);
      }

      private bool ValidateHelper(JsonItem item, ref IList<JsonItem> faultyItems) {
        int hash = item.GetHashCode();
        if (Cache.TryGetValue(hash, out bool r))
          return r;

        bool res = true;
        if (item.Value != null && item.Range != null)
          res = item.IsInRange();
        if (!res) faultyItems.Add(item);
        Cache.Add(hash, res);
        foreach (var child in item.Children)
          res = res && ValidateHelper(child, ref faultyItems);
        return res;
      }
    }

    public virtual string Name { get; set; }

    public virtual string Path {
      get {
        JsonItem tmp = Parent;
        StringBuilder builder = new StringBuilder(this.Name);
        while(tmp != null) {
          builder.Insert(0, tmp.Name + ".");
          tmp = tmp.Parent;
        }
        return builder.ToString();
      }
    }

    [JsonIgnore]
    public virtual IList<JsonItem> Children { get; protected set; }

    [JsonIgnore]
    public virtual JsonItem Parent { get; set; }

    public virtual object Value { get; set; }

    public virtual IEnumerable<object> Range { get; set; }
    
    public virtual string ActualName { get; set; }

    #region Constructors
    public JsonItem() { }

    public JsonItem(IEnumerable<JsonItem> childs) {
      AddChilds(childs);
    }
    #endregion


    #region Public Static Methods
    public static void Merge(JsonItem target, JsonItem from) {
      target.Name = from.Name ?? target.Name;
      target.Range = from.Range ?? target.Range;
      target.Value = from.Value ?? target.Value;
      target.ActualName = from.ActualName ?? target.ActualName;
      if(target.Children != null) {
        if (from.Children != null)
          ((List<JsonItem>)from.Children).AddRange(target.Children); 
      } else {
        target.Children = from.Children;
      }
    }
    #endregion

    #region Public Methods
    public void AddChilds(params JsonItem[] childs) => 
      AddChilds(childs as IEnumerable<JsonItem>);

    public void AddChilds(IEnumerable<JsonItem> childs) {
      if (Children == null)
        Children = new List<JsonItem>();
      foreach (var child in childs) {
        Children.Add(child);
        child.Parent = this;
      }
    }

    public virtual JsonItemValidator GetValidator() => new JsonItemValidator(this);
    #endregion

    #region Helper
    protected bool IsInRange() {
      bool b1 = true, b2 = true;
      if (Value is IEnumerable && !(Value is string)) {
        foreach (var x in (IEnumerable)Value) {
          b1 = b1 ? IsInRangeList(x) : b1;
          b2 = b2 ? IsInNumericRange(x) : b2;
        }
      } 
      else {
        b1 = IsInRangeList(Value); 
        b2 = IsInNumericRange(Value);
      } 
      return b1 || b2;
    }

    protected bool IsInRangeList(object value) {
      foreach (var x in Range)
        if (x.Equals(value)) return true;
      return false;
    }

    protected bool IsInNumericRange(object value) =>
      IsInNumericRange<ulong>(value)
      || IsInNumericRange<uint>(value)
      || IsInNumericRange<ushort>(value)
      || IsInNumericRange<long>(value)
      || IsInNumericRange<int>(value)
      || IsInNumericRange<short>(value)
      || IsInNumericRange<byte>(value)
      || IsInNumericRange<float>(value)
      || IsInNumericRange<double>(value)
      || (value is float && float.IsNaN((float)value))
      || (value is double && double.IsNaN((double)value));

    protected bool IsInNumericRange<T>(object value) where T : IComparable {
      object min = Range.First(), max = Range.Last();
      return
        value != null && min != null && max != null && value is T && min is T && max is T &&
        (((T)min).CompareTo(value) == -1 || ((T)min).CompareTo(value) == 0) &&
        (((T)max).CompareTo(value) == 1 || ((T)max).CompareTo(value) == 0);
    }
    #endregion

    #region BuildJsonItemMethods
    public static JsonItem BuildJsonItem(JObject obj) {
      object val = obj[nameof(Value)]?.ToObject<object>();
      if (val is JContainer jContainer) // for resolving array values
        val = jContainer.ToObject<object[]>();
        
      return new JsonItem() {
        Name = obj[nameof(Name)]?.ToString(),
        Value = val,
        Range = obj[nameof(Range)]?.ToObject<object[]>(),
        ActualName = obj[nameof(ActualName)]?.ToString()
      };
    }
    #endregion
  }
}