﻿using System;
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
  /// Plugin class for HeuristicLab.Problems.VehicleRouting.Views plugin
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


      server = RestServerBuilder.UseDefaults().Build();
      //var firewallPolicy = new FirewallPolicy {
      //  AppExecutablePath = @"D:\FH\HL\HeuristicLab\bin\HeuristicLab 3.3.exe", //Application Executable Path
      //  Description = "Description of your firewall rule",
      //  Name = "Heuristic Lab" // Your Application Name
      //};

      //server.UseFirewallPolicy(firewallPolicy);

      server.Prefixes.Add("http://localhost:8080/");
      server.Prefixes.Add("http://127.0.0.1:8080/");

      server.AfterStarting += (s) => {
        // This will produce a weird name in the output like `<Main>b__0_2` or something unless you add a name argument to the route constructor.
        s.Router.Register(new Route(async (ctx) => {

          dynamic json = new ExpandoObject();
          json.Algorithms = rootNode;


          ctx.Response.ContentType = "application/json";
          string serializedJson = JsonSerializer.Serialize(json);
          await ctx.Response.SendResponseAsync(serializedJson);
        }, "Get", "/getAlgorithms"));
      };

      server.AfterStarting += (s) => {
        // This will produce a weird name in the output like `<Main>b__0_2` or something unless you add a name argument to the route constructor.
        s.Router.Register(new Route(async (ctx) => {

          dynamic json = new ExpandoObject();
          json.Name = $"Problem {problem.Name} - {problem.Description}";

          List<String> parameterList = problem.Parameters.Select(x => x.Name).ToList();
          json.Parameter = parameterList;


          ctx.Response.ContentType = "application/json";
          string serializedJson = JsonSerializer.Serialize(json);
          await ctx.Response.SendResponseAsync(serializedJson);
        }, "Get", "/getProblemParameter"));
      };

      server.AfterStarting += (s) => {
        // This will produce a weird name in the output like `<Main>b__0_2` or something unless you add a name argument to the route constructor.
        s.Router.Register(new Route(async (ctx) => {

          dynamic json = new ExpandoObject();
          var param = problem.Parameters.LastOrDefault();
          json.Name = param.Name;
          json.Description = param.Description;
          json.DataType = param.DataType.ToString();
          json.Value = param.ActualValue.ToString();


          ctx.Response.ContentType = "application/json";
          string serializedJson = JsonSerializer.Serialize(json);
          await ctx.Response.SendResponseAsync(serializedJson);
        }, "Get", "/getParameterInfo"));
      };

      server.AfterStarting += (s) => {
        // This will produce a weird name in the output like `<Main>b__0_2` or something unless you add a name argument to the route constructor.
        s.Router.Register(new Route(async (ctx) => {

          dynamic json = new ExpandoObject();
          List<dynamic> jsons = new List<dynamic>();
          foreach (var param in problem.Parameters) {
            dynamic jsonInner = new ExpandoObject();
            jsonInner.Name = param.Name;
            jsonInner.Description = param.Description;
            jsonInner.DataType = param.DataType?.ToString();
            jsonInner.Value = param.ActualValue?.ToString();
            jsons.Add(jsonInner);
          }

          json.parameters = jsons;

          string serializedJson = JsonSerializer.Serialize(json);
          await ctx.Response.SendResponseAsync(serializedJson);
        }, "Get", "/getParameterInfos"));
      };

      server.AfterStarting += (s) => {
        s.Router.Register(new Route(async (ctx) => {
          var pathParameters = ctx.Request.PathParameters;
          var queryStrings = ctx.Request.QueryString;
          var dataTypeString = queryStrings["dataType"];

          var type = GetType(dataTypeString);
          dynamic json = new ExpandoObject();
          var possibleTypes = ApplicationManager.Manager.GetTypes(type);


          json.PossibleTypes = possibleTypes.Select(x => x.FullName).ToList();
          json.dataType = dataTypeString;

          ctx.Response.ContentType = "application/json";
          string serializedJson = JsonSerializer.Serialize(json);
          await ctx.Response.SendResponseAsync(serializedJson);
        }, "Get", "/getPossibleParameterValues"));
      };

      server.AfterStarting += (s) => {
        s.Router.Register(new Route(async (ctx) => {
          // https://github.com/scottoffen/grapevine/discussions/64
          //JsonConvert
          //dynamic requestBody = await JsonSerializer.DeserializeAsync<ExpandoObject>(ctx.Request.InputStream);
          StreamReader stream = new StreamReader(ctx.Request.InputStream);
          string requestBodyAsString = stream.ReadToEnd();
          var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(requestBodyAsString);

          // Baseklasse für Valuetypes = ValueTypeValue
          string propertyName = dict["PropertyName"];
          string propertyTypeName = dict["DataType"];
          Type propertyType = GetType(propertyTypeName);
          string value = dict["Value"];

          // 1. create property
          var property = Activator.CreateInstance(propertyType);
          dynamic newPropertyValue = Convert.ChangeType(property, propertyType);

          var propertyFromProblem = problem.Parameters[propertyName];

          //propertyFromProblem.ActualValue = newPropertyValue;

          // 2. create type to set
          // check if SimpleValue or complex parameter
          var genericParam = property.GetType().GenericTypeArguments?.FirstOrDefault();
          var problemParam = problem.Parameters[propertyName];
          bool isSimpleValue = IsSubclassOfRawGeneric(typeof(ValueTypeValue<>), property.GetType()); //  property.GetType().IsSubclassOf(ValueTypeValue.GetType());
          bool isOptionaConstrainedValueParameter = IsSubclassOfRawGeneric(typeof(OptionalConstrainedValueParameter<>), problemParam.GetType());

          if (isSimpleValue) {
            if (newPropertyValue.Value is bool b) {
              b = bool.Parse(value);
              (property as BoolValue).Value = b;
              propertyFromProblem.ActualValue = property as BoolValue;
              //propertyFromProblem.ActualValue = b;
            } else if (newPropertyValue.Value is double d) {
              d = double.Parse(value);
              (property as DoubleValue).Value = d;
              propertyFromProblem.ActualValue = property as DoubleValue;
              //propertyFromProblem.ActualValue = d;
            }
          } else {
            // complex value
            //problem.Parameters[propertyName].DataType = property.GetType();
            IItem propertyAsItem = (IItem)property;

            // we need to use the property with the same memory adress as in the validValues ItemCollection
            if (isOptionaConstrainedValueParameter) {
              // we need to find T so we can cast it to the right type
              var genericArguments = problemParam.GetType().GetGenericArguments();
              var genericArgument = genericArguments[0];

              // we need to call the method with reflection, because we can't cast to OptionalConstrainedValueParameter with a generic type argument
              var validValues2 = (problemParam as dynamic).ValidValues;

              // using dynamic because we can not instant a variable who we don't know the generic type at compile time
              // dynamic disables type checking

              //var enumerator = ((ItemSet<>)validValues2).GetEnumerator();
              dynamic enumerator = validValues2.GetEnumerator();

              while (enumerator.MoveNext()) {
                object current = enumerator.Current;
                if (current.GetType() == property.GetType()) {
                  propertyAsItem = (IItem)current;
                  break;
                }
              }
            }
            problem.Parameters[propertyName].ActualValue = propertyAsItem;
          }

          ctx.Response.ContentType = "application/json";
          await ctx.Response.SendResponseAsync("ok");
        }, "Post", "/setProblemParameter"));
      };

      server.AfterStarting += (s) => {
        s.Router.Register(new Route(async (ctx) => {

          string parameterName = ctx.Request.QueryString["ParameterName"];
          var parameter = problem.Parameters[parameterName];

          dynamic json = new ExpandoObject();
          json.Problem = problem.Name;
          json.ProblemType = problem.GetType().FullName;
          json.ParameterName = parameterName;
          json.DataType = parameter.DataType.FullName;

          var value = parameter.ActualValue;
          if (value is ParameterizedNamedItem pmi) {
            IDictionary<string, IItem> values = new Dictionary<string, IItem>();

            List<dynamic> parameterInfos = new List<dynamic>();

            pmi.CollectParameterValues(values);
            json.Parameters = values;
            // the item got paramters => add them to the json
            foreach (var keyValuePair in values) {
              dynamic current = new ExpandoObject();
              current.Key = keyValuePair.Key;
              current.DataType = keyValuePair.Value.GetType().FullName;
              current.ItemDescription = keyValuePair.Value.ItemDescription;
              current.ItemName = keyValuePair.Value.ItemName;
              parameterInfos.Add(current);
            }
            json.Parameters = parameterInfos;
          }

          string serializedJson = JsonSerializer.Serialize(json);
          ctx.Response.ContentType = "application/json";
          await ctx.Response.SendResponseAsync(serializedJson);
        }, "Get", "/problem/parameter/info"));
      };

      server.AfterStarting += (s) => {
        s.Router.Register(new Route(async (ctx) => {
          // https://github.com/scottoffen/grapevine/discussions/64
          //JsonConvert
          //dynamic requestBody = await JsonSerializer.DeserializeAsync<ExpandoObject>(ctx.Request.InputStream);
          StreamReader stream = new StreamReader(ctx.Request.InputStream);
          string requestBodyAsString = stream.ReadToEnd();
          var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(requestBodyAsString);

          // Baseklasse für Valuetypes = ValueTypeValue
          string problemParameterName = dict["ProblemParameterName"];
          string propertyName = dict["PropertyName"];
          string propertyTypeName = dict["DataType"];
          Type propertyType = GetType(propertyTypeName);
          string value = dict["Value"];

          // 1. create property
          var property = Activator.CreateInstance(propertyType);
          dynamic newPropertyValue = Convert.ChangeType(property, propertyType);

          var problemParameter = problem.Parameters[problemParameterName];

          if (problemParameter.ActualValue is ParameterizedNamedItem pmi) {
            IDictionary<string, IItem> values = new Dictionary<string, IItem>();
            pmi.CollectParameterValues(values);

            var propertyToModify = values.Where(x => x.Key == propertyName).FirstOrDefault();
            bool isSimpleValue = IsSubclassOfRawGeneric(typeof(ValueTypeValue<>), propertyToModify.Value.GetType()); //  property.GetType().IsSubclassOf(ValueTypeValue.GetType());
            bool isOptionaConstrainedValueParameter = IsSubclassOfRawGeneric(typeof(OptionalConstrainedValueParameter<>), propertyToModify.Value.GetType());

            // TODO: setzen funktioniert nicht. Wir benötigen bei GET Param Info auch noch den aktuellen Wert

            if (isSimpleValue) {
              if (newPropertyValue.Value is bool b) {
                var valueToModify = propertyToModify.Value;
                b = bool.Parse(value);
                (property as BoolValue).Value = b;
                valueToModify = property as BoolValue;
                //propertyFromProblem.ActualValue = b;
              } else if (newPropertyValue.Value is double d) {
                d = double.Parse(value);
                (property as DoubleValue).Value = d;
                var valueToModify = propertyToModify.Value;
                valueToModify = property as BoolValue;
                //propertyFromProblem.ActualValue = d;
              }
            } else {
              // complex value
              //problem.Parameters[propertyName].DataType = property.GetType();
              IItem propertyAsItem = (IItem)property;

              // we need to use the property with the same memory adress as in the validValues ItemCollection
              if (isOptionaConstrainedValueParameter) {
                // we need to find T so we can cast it to the right type
                var genericArguments = propertyToModify.GetType().GetGenericArguments();
                var genericArgument = genericArguments[0];

                // we need to call the method with reflection, because we can't cast to OptionalConstrainedValueParameter with a generic type argument
                var validValues2 = (propertyToModify as dynamic).ValidValues;

                // using dynamic because we can not instant a variable who we don't know the generic type at compile time
                // dynamic disables type checking

                //var enumerator = ((ItemSet<>)validValues2).GetEnumerator();
                dynamic enumerator = validValues2.GetEnumerator();

                while (enumerator.MoveNext()) {
                  object current = enumerator.Current;
                  if (current.GetType() == property.GetType()) {
                    propertyAsItem = (IItem)current;
                    break;
                  }
                }
              }
              problem.Parameters[propertyName].ActualValue = propertyAsItem;
            }
            ctx.Response.ContentType = "application/json";
            await ctx.Response.SendResponseAsync("ok");

          } else {
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = HttpStatusCode.BadRequest;
            await ctx.Response.SendResponseAsync($"Problem.Parameter {problemParameter} is not a ParameterizedNamedItem. It does not have a ParameterCollection.");
            return;
          }
        }, "Post", "/problem/parameter"));
      };

      server.AfterStarting += (s) => {
        s.Router.Register(new Route(async (ctx) => {
          Dictionary<string, IItem> results = new Dictionary<string, IItem>();
          algo.Results.CollectResultValues(results);


          ctx.Response.ContentType = "application/json";
          string serializedJson = JsonSerializer.Serialize(results);
          await ctx.Response.SendResponseAsync(serializedJson);
        }, "Get", "/result"));
      };

      server.Start();
      server.AfterStopping += (e) => { Console.WriteLine("=== === Server stopped === ==="); };
      Console.WriteLine($"* Server listening on {string.Join(", ", server.Prefixes)}{Environment.NewLine}");
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
