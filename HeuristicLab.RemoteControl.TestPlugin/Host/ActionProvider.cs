using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Grapevine;
using HeuristicLab.Core;
using HeuristicLab.Data;
using HeuristicLab.Optimization;
using HeuristicLab.Parameters;
using HeuristicLab.PluginInfrastructure;
using HeuristicLab.RemoteControl.TestPlugin.Util;

namespace HeuristicLab.RemoteControl.TestPlugin.Host {
  public class ActionProvider : IActionProvider {

    private readonly IProblem Problem;
    private readonly IAlgorithm Algorithm;

    public ActionProvider(IProblem problem, IAlgorithm algorithm) {
      Problem = problem;
      Algorithm = algorithm;
    }

    public async Task GetParameterInfo(IHttpContext ctx) {
      dynamic json = new ExpandoObject();
      var param = Problem.Parameters.LastOrDefault();
      json.Name = param.Name;
      json.Description = param.Description;
      json.DataType = param.DataType.ToString();
      json.Value = param.ActualValue.ToString();


      ctx.Response.ContentType = "application/json";
      string serializedJson = JsonSerializer.Serialize(json);
      await ctx.Response.SendResponseAsync(serializedJson);
    }

    public async Task GetParameterInfoInfo(IHttpContext ctx) {
      string parameterName = ctx.Request.QueryString["ParameterName"];
      var parameter = Problem.Parameters[parameterName];

      dynamic json = new ExpandoObject();
      json.Problem = Problem.Name;
      json.ProblemType = Problem.GetType().FullName;
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
    }

    public async Task GetParameterInfos(IHttpContext ctx) {
      dynamic json = new ExpandoObject();
      List<dynamic> jsons = new List<dynamic>();
      foreach (var param in Problem.Parameters) {
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
    }

    public async Task GetPossibleParameterValues(IHttpContext ctx) {
      var pathParameters = ctx.Request.PathParameters;
      var queryStrings = ctx.Request.QueryString;
      var dataTypeString = queryStrings["dataType"];

      var type = ReflectionUtil.GetType(dataTypeString);
      dynamic json = new ExpandoObject();
      var possibleTypes = ApplicationManager.Manager.GetTypes(type);


      json.PossibleTypes = possibleTypes.Select(x => x.FullName).ToList();
      json.dataType = dataTypeString;

      ctx.Response.ContentType = "application/json";
      string serializedJson = JsonSerializer.Serialize(json);
      await ctx.Response.SendResponseAsync(serializedJson);
    }

    public async Task GetProblemParameter(IHttpContext ctx) {
      throw new NotImplementedException();
    }

    public async Task GetProblemParameterAction(IHttpContext ctx) {
      dynamic json = new ExpandoObject();
      json.Name = $"Problem {Problem.Name} - {Problem.Description}";

      List<String> parameterList = Problem.Parameters.Select(x => x.Name).ToList();
      json.Parameter = parameterList;


      ctx.Response.ContentType = "application/json";
      string serializedJson = JsonSerializer.Serialize(json);
      await ctx.Response.SendResponseAsync(serializedJson);
    }

    public async Task GetResult(IHttpContext ctx) {
      Dictionary<string, IItem> results = new Dictionary<string, IItem>();
      Algorithm.Results.CollectResultValues(results);


      ctx.Response.ContentType = "application/json";
      string serializedJson = JsonSerializer.Serialize(results);
      await ctx.Response.SendResponseAsync(serializedJson);
    }

    public async Task PostProblemParameter(IHttpContext ctx) {
      // https://github.com/scottoffen/grapevine/discussions/64
      //JsonConvert
      //dynamic requestBody = await JsonSerializer.DeserializeAsync<ExpandoObject>(ctx.Request.InputStream);
      StreamReader stream = new StreamReader(ctx.Request.InputStream);
      string requestBodyAsString = stream.ReadToEnd();
      var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(requestBodyAsString);

      // Baseklasse für Valuetypes = ValueTypeValue
      string propertyName = dict["PropertyName"];
      string propertyTypeName = dict["DataType"];
      Type propertyType = ReflectionUtil.GetType(propertyTypeName);
      string value = dict["Value"];

      // 1. create property
      var property = Activator.CreateInstance(propertyType);
      dynamic newPropertyValue = Convert.ChangeType(property, propertyType);

      var propertyFromProblem = Problem.Parameters[propertyName];

      //propertyFromProblem.ActualValue = newPropertyValue;

      // 2. create type to set
      // check if SimpleValue or complex parameter
      var genericParam = property.GetType().GenericTypeArguments?.FirstOrDefault();
      var problemParam = Problem.Parameters[propertyName];
      bool isSimpleValue = ReflectionUtil.IsSubclassOfRawGeneric(typeof(ValueTypeValue<>), property.GetType()); //  property.GetType().IsSubclassOf(ValueTypeValue.GetType());
      bool isOptionaConstrainedValueParameter = ReflectionUtil.IsSubclassOfRawGeneric(typeof(OptionalConstrainedValueParameter<>), problemParam.GetType());

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
        Problem.Parameters[propertyName].ActualValue = propertyAsItem;
      }

      ctx.Response.ContentType = "application/json";
      await ctx.Response.SendResponseAsync("ok");
    }

    public async Task SetProblemParameter(IHttpContext ctx) {
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
      Type propertyType = ReflectionUtil.GetType(propertyTypeName);
      string value = dict["Value"];

      // 1. create property
      var property = Activator.CreateInstance(propertyType);
      dynamic newPropertyValue = Convert.ChangeType(property, propertyType);

      var problemParameter = Problem.Parameters[problemParameterName];

      if (problemParameter.ActualValue is ParameterizedNamedItem pmi) {
        IDictionary<string, IItem> values = new Dictionary<string, IItem>();
        pmi.CollectParameterValues(values);

        var propertyToModify = values.Where(x => x.Key == propertyName).FirstOrDefault();
        bool isSimpleValue = ReflectionUtil.IsSubclassOfRawGeneric(typeof(ValueTypeValue<>), propertyToModify.Value.GetType()); //  property.GetType().IsSubclassOf(ValueTypeValue.GetType());
        bool isOptionaConstrainedValueParameter = ReflectionUtil.IsSubclassOfRawGeneric(typeof(OptionalConstrainedValueParameter<>), propertyToModify.Value.GetType());

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
          Problem.Parameters[propertyName].ActualValue = propertyAsItem;
        }
        ctx.Response.ContentType = "application/json";
        await ctx.Response.SendResponseAsync("ok");

      } else {
        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = HttpStatusCode.BadRequest;
        await ctx.Response.SendResponseAsync($"Problem.Parameter {problemParameter} is not a ParameterizedNamedItem. It does not have a ParameterCollection.");
        return;
      }
    }
  }
}
