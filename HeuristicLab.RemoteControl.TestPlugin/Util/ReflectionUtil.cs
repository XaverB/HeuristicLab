using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeuristicLab.RemoteControl.TestPlugin.Util {

  
  internal class ReflectionUtil {
    /// <summary>
    /// Check if a generic type is a subclass of a specific generic type.
    /// </summary>
    /// <param name="generic">The type we are looking for.</param>
    /// <param name="toCheck">The type to check</param>
    /// <returns>True if toCheck is a subclass of generic</returns>
    // from https://stackoverflow.com/a/457708 by JaredPar
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


    // from https://stackoverflow.com/a/11811046 by peyman
    // visited on 31.07.2022
    /// <summary>
    /// Looks in all assemblies of the current application domain for a desired type.
    /// </summary>
    /// <param name="typeName">The full qualified type name</param>
    /// <returns>The desired type if found, else null</returns>
    public static Type GetType(string typeName) {
      // value types
      var type = Type.GetType(typeName);
      if (type != null) return type;
      // search for the type in every assembly which is located in the current application domain
      foreach (var a in AppDomain.CurrentDomain.GetAssemblies()) {
        type = a.GetType(typeName);
        if (type != null)
          return type;
      }
      return null;
    }
  }
}
