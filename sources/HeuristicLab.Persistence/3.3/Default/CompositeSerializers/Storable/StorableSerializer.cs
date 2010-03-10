﻿using System;
using System.Collections.Generic;
using System.Linq;
using HeuristicLab.Persistence.Interfaces;
using HeuristicLab.Persistence.Core;
using System.Reflection;
using HeuristicLab.Persistence.Auxiliary;

namespace HeuristicLab.Persistence.Default.CompositeSerializers.Storable {

  [EmptyStorableClass]
  public class StorableSerializer : ICompositeSerializer {

    public int Priority {
      get { return 200; }
    }

    public bool CanSerialize(Type type) {
      if (!ReflectionTools.HasDefaultConstructor(type))
        return false;
      while (type != null) {
        if (StorableAttribute.GetStorableMembers(type, false).Count() == 0 &&
            !EmptyStorableClassAttribute.IsEmptyStorable(type))
          return false;
        type = type.BaseType;
      }
      return true;
    }

    public IEnumerable<Tag> CreateMetaInfo(object o) {
      StorableHookAttribute.InvokeHook(HookType.BeforeSerialization, o);
      return new Tag[] { };
    }

    public IEnumerable<Tag> Decompose(object obj) {
      foreach (var accessor in StorableAttribute.GetStorableAccessors(obj)) {
        yield return new Tag(accessor.Name, accessor.Get());
      }
    }

    private static readonly object[] defaultArgs = new object[] { true };

    public object CreateInstance(Type type, IEnumerable<Tag> metaInfo) {
      try {
        ConstructorInfo constructor = StorableConstructorAttribute.GetStorableConstructor(type);
        return constructor != null ? constructor.Invoke(defaultArgs) :  Activator.CreateInstance(type, true);        
      } catch (TargetInvocationException x) {
        throw new PersistenceException(
          "Could not instantiate storable object: Encountered exception during constructor call",
          x.InnerException);
      }
    }

    public void Populate(object instance, IEnumerable<Tag> objects, Type type) {
      var memberDict = new Dictionary<string, Tag>();
      IEnumerator<Tag> iter = objects.GetEnumerator();
      while (iter.MoveNext()) {
        memberDict.Add(iter.Current.Name, iter.Current);
      }
      foreach (var accessor in StorableAttribute.GetStorableAccessors(instance)) {
        if (memberDict.ContainsKey(accessor.Name)) {
          accessor.Set(memberDict[accessor.Name].Value);
        } else if (accessor.DefaultValue != null) {
          accessor.Set(accessor.DefaultValue);
        }
      }
      StorableHookAttribute.InvokeHook(HookType.AfterDeserialization, instance);
    }
  }
}