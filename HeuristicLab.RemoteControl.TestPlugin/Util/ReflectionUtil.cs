using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeuristicLab.RemoteControl.TestPlugin.Util {
  internal class ReflectionUtil {
    public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
      while (toCheck != null && toCheck != typeof(object)) {
        var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
        if (generic == cur) {
          return true;
        }
        toCheck = toCheck.BaseType;
      }
      return false;
    }

    public T CastObject<T>(object input) {
      return (T)input;
    }

    public T ConvertObject<T>(object input) {
      return (T)Convert.ChangeType(input, typeof(T));
    }

    // from https://stackoverflow.com/a/11811046 by peyman
    // visited on 31.07.2022
    public static Type GetType(string typeName) {
      // value types
      var type = Type.GetType(typeName);
      if (type != null) return type;
      // search for the type in every assembly which is located in the current app domain
      foreach (var a in AppDomain.CurrentDomain.GetAssemblies()) {
        type = a.GetType(typeName);
        if (type != null)
          return type;
      }

      // complex objects

      return null;
    }
  }
}
