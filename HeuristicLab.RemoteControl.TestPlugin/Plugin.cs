using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeuristicLab.PluginInfrastructure;
using HeuristicLab.Core;
using HeuristicLab.MainForm;
using HeuristicLab.PluginInfrastructure.Manager;
using HeuristicLab.Optimization;
using Grapevine;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using System.Text.Json;
using System.Dynamic;
using HeuristicLab.Data;
using System.IO;
using HeuristicLab.Parameters;

namespace HeuristicLab.RemoteControl.TestPlugin {
  [RestResource]
  public class MyResource {
    [RestRoute("Get", "/api/test")]
    public async Task Test(IHttpContext context) {
      await context.Response.SendResponseAsync("Successfully hit the test route!");
    }
  }

  /// <summary>
  /// Plugin class for remote controlling HeuristicLab applicaiton Optimizer with a REST - service
  /// </summary>
  [Plugin("HeuristicLab.RemoteControl.TestPlugin", "1.0.0.0")]
  [PluginFile("HeuristicLab.RemoteControl.TestPlugin.dll", PluginFileType.Assembly)]
  [PluginDependency("HeuristicLab.Common", "3.3")]
  [PluginDependency("HeuristicLab.Common.Resources", "3.3")]
  [PluginDependency("HeuristicLab.Core", "3.3")]
  [PluginDependency("HeuristicLab.Data", "3.3")]
  [PluginDependency("HeuristicLab.Core.Views", "3.3")]
  [PluginDependency("HeuristicLab.MainForm", "3.3")]
  [PluginDependency("HeuristicLab.MainForm.WindowsForms", "3.3")]
  [PluginDependency("HeuristicLab.Optimization.Views", "3.3")]
  [PluginDependency("HeuristicLab.Problems.VehicleRouting", "3.4")]
  public class RemoteControlTestPlugin : PluginBase {

    IRestServer server;

    public override void OnLoad() {

      var types = ApplicationManager.Manager.GetTypes(typeof(IItem));
      var createableTypes = types.Where(x => CreatableAttribute.IsCreatable(x));

      var categories =
       from type in createableTypes
       let category = CreatableAttribute.GetCategory(type)
       let hasOrdering = category.Contains(CreatableAttribute.Categories.OrderToken)
       let name = ItemAttribute.GetName(type)
       let priority = CreatableAttribute.GetPriority(type)
       let version = ItemAttribute.GetVersion(type)
       orderby category, hasOrdering descending, priority, name, version ascending
       group type by category into categoryGroup
       select categoryGroup;


      var rootNode = CreateCategoryTree(categories);
      CreateItemNodes(rootNode, categories);

      foreach (var node in rootNode.Nodes) {
        Console.WriteLine($"Found node: {node}");
      }

      TreeNode selectedNode = rootNode.Nodes.FirstOrDefault();
      while (selectedNode.Nodes.Count > 0)
        selectedNode = selectedNode.Nodes.FirstOrDefault();

      Type selectedType = selectedNode.Tag as Type;
      IItem item = (IItem)Activator.CreateInstance(selectedType);

      //var algorithms = ApplicationManager.Manager.GetInstances<IAlgorithm>();
      var problems = ApplicationManager.Manager.GetInstances<IProblem>();
      Dictionary<string, IProblem> problemsDictionary = problems.ToDictionary(x => x.Name, x => x);

      var algo = (IAlgorithm)item;
      var problem = problems.LastOrDefault();
      algo.Problem = problem;//problems.LastOrDefault();

      // print param from problem
      System.Diagnostics.Debug.WriteLine($"=== Problem {problem} parameters ===");
      foreach (var parameter in problem.Parameters) {
        Console.WriteLine($"\tParameter: {parameter} ==");

        var parameterTypes = ApplicationManager.Manager.GetTypes(parameter.GetType());
        Console.WriteLine($"\t\tInstances:");
        foreach (var paramType in parameterTypes) {
          Console.WriteLine($"\t\t{paramType}");
        }

      }

      Task.Delay(10000).ContinueWith(e => {
        IView view = MainFormManager.MainForm.ShowContent(algo);
        algo.StartAsync();
      });
    }


    static bool IsSubclassOfRawGeneric(Type generic, Type toCheck) {
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

    private static TreeNode CreateCategoryTree(IEnumerable<IGrouping<string, Type>> categories) {

      var rootNode = new TreeNode();

      // CategoryNode
      // Tag: raw string, used for sorting, e.g. 1$$$Algorithms###2$$$Single Solution
      // Name: full name = combined category name with parent categories, used for finding nodes in tree, e.g. Algorithms###Single Solution
      // Text: category name, used for displaying on node itself, e.g. Single Solution

      foreach (var category in categories) {
        var rawName = category.Key;
        string fullName = CreatableAttribute.Categories.GetFullName(rawName);
        string name = CreatableAttribute.Categories.GetName(rawName);

        // Skip categories with same full name because the raw name can still be different (missing order)
        if (rootNode.Find(fullName, true)?.Count > 0)
          continue;

        var categoryNode = new TreeNode(name) {
          Name = fullName,
          Tag = rawName
        };

        var parents = CreatableAttribute.Categories.GetParentRawNames(rawName);
        var parentNode = FindOrCreateParentNode(rootNode, parents);
        if (parentNode != null)
          parentNode.Nodes.Add(categoryNode);
        else
          rootNode.Nodes.Add(categoryNode);
      }

      return rootNode;
    }
    private static TreeNode FindOrCreateParentNode(TreeNode node, IEnumerable<string> rawParentNames) {
      TreeNode parentNode = null;
      string rawName = null;
      foreach (string rawParentName in rawParentNames) {
        rawName = rawName == null ? rawParentName : rawName + CreatableAttribute.Categories.SplitToken + rawParentName;
        var fullName = CreatableAttribute.Categories.GetFullName(rawName);
        parentNode = node.Find(fullName, false).SingleOrDefault();
        if (parentNode == null) {
          var name = CreatableAttribute.Categories.GetName(rawName);
          parentNode = new TreeNode(name) {
            Name = fullName,
            //,
            Tag = rawName
          };
          node.Nodes.Add(parentNode);
        }
        node = parentNode;
      }
      return parentNode;
    }
    private static void CreateItemNodes(TreeNode node, IEnumerable<IGrouping<string, Type>> categories) {
      foreach (var category in categories) {
        var fullName = CreatableAttribute.Categories.GetFullName(category.Key);
        var categoryNode = node.Find(fullName, true).Single();
        foreach (var creatable in category) {
          var itemNode = CreateItemNode(creatable);
          itemNode.Name = itemNode.Name + ":" + fullName;
          itemNode.Tag = creatable;
          categoryNode.Nodes.Add(itemNode);
        }
      }
    }
    private static TreeNode CreateItemNode(Type creatable) {
      string name = ItemAttribute.GetName(creatable);

      var itemNode = new TreeNode(name) {
        //ImageIndex = 0,
        //Tag = creatable,
        Name = name
      };

      return itemNode;
    }
  }

}
