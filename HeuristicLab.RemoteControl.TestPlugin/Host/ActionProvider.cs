using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
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
    private const string CONTENT_TYPE_JSON = "application/json";
    private readonly IProblem Problem;
    private readonly IAlgorithm Algorithm;

    public ActionProvider(IProblem problem, IAlgorithm algorithm) {
      Problem = problem;
      Algorithm = algorithm;
    }

    object FindProperty(IKeyedItemCollection<string, IParameter> rootParameter, string previousPath, string desiredProperty) {
      foreach (var parameter in rootParameter) {
        bool isValueParameter2 = parameter.GetType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValueParameter<>));
        bool isConstrainedValueParameter = ReflectionUtil.IsSubclassOfRawGeneric(typeof(ConstrainedValueParameter<>), parameter.GetType());
        if (isValueParameter2 | isConstrainedValueParameter) {

          string currentPath = $"{previousPath}.{parameter.Name}";
          if (currentPath == desiredProperty)
            return parameter;

          //currentJson.Value = parameter.ActualValue;
          bool isParameterizedItem = parameter.GetType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IParameterizedItem));

          bool isGenericParameterizedItem2 = false;
          var genericArguments = parameter.GetType().GetGenericArguments();
          foreach (var genericArgument in genericArguments) {
            var interfaces = genericArgument.GetInterfaces();

            var isParameterizedItem2 = interfaces.Any(x => x == typeof(IParameterizedItem));
            if (isParameterizedItem2) {
              isGenericParameterizedItem2 = true;
              break;
            }
          }

          if (isParameterizedItem || isGenericParameterizedItem2) {
            var actualValue = parameter.ActualValue;
            object item = FindProperty((actualValue as IParameterizedItem).Parameters, $"{previousPath}.{parameter.Name}", desiredProperty); // maybe we need to note that this is a generic argument
            if (item != null)
              return item;
          }
        }
      }

      return null;
    }


    dynamic GetProblemParameter(IKeyedItemCollection<string, IParameter> rootParameter, string previousPath) {
      var parameters = rootParameter;
      dynamic json = new ExpandoObject();
      List<dynamic> parameterJson = new List<dynamic>();
      json.Parameters = parameterJson;

      foreach (var parameter in parameters) {
        bool isValueParameter2 = parameter.GetType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValueParameter<>));
        if (isValueParameter2) {
          dynamic currentJson = new ExpandoObject();
          currentJson.Type = parameter.GetType().FullName;
          currentJson.Name = parameter.Name;
          currentJson.Path = $"{previousPath}.{parameter.Name}";
          //currentJson.Value = parameter.ActualValue;
          bool isParameterizedItem = parameter.GetType().GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IParameterizedItem));

          // check if the value is a interface so we can deliver possible values
          // TODO
          Type genericTypeArgument = null;
          genericTypeArgument = parameter.GetType().GetGenericArguments().FirstOrDefault();
          bool isGenericTypeArgumentInterface = genericTypeArgument?.IsInterface ?? false;
          if (isGenericTypeArgumentInterface) {
            var possibleInstances = ApplicationManager.Manager.GetInstances(genericTypeArgument);
            currentJson.Note = "This parameters generic type argument is a interface. Take a look at PossibleInstances";
            currentJson.PossibleInstances = possibleInstances.Select(x => x.GetType().FullName);
            //var genericTypeArgument = property.GetType().GetGenericArguments().FirstOrDefault();
            //bool isGenericTypeArgumentInterface = genericTypeArgument.IsInterface;
          }

          // check if the value is a constrainedvalue, so we can deliver possible values
          bool isConstraintValue = ReflectionUtil.IsSubclassOfRawGeneric(typeof(ConstrainedValueParameter<>), parameter.GetType());
          if(isConstraintValue) {
            currentJson.Note = "This parameter is a ConstrainedValueParameter. Take a look at the ValidValues";

            var validValuesMethod = parameter.GetType().GetMethod("get_ValidValues", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            object itemSet = validValuesMethod.Invoke(parameter, null);
            var getEnumeratorMethod = itemSet.GetType().GetMethod("GetEnumerator");
            IEnumerator<object> enumerator = (IEnumerator<object>)getEnumeratorMethod.Invoke(itemSet, null);
            List<string> validValues = new List<string>();
            while (enumerator.MoveNext()) {
              validValues.Add(enumerator.Current.GetType().FullName);
            }
            currentJson.ValidValues = validValues;
          }
          // TODO


          bool isGenericParameterizedItem2 = false;
          var genericArguments = parameter.GetType().GetGenericArguments();
          foreach (var genericArgument in genericArguments) {
            var interfaces = genericArgument.GetInterfaces();

            var isParameterizedItem2 = interfaces.Any(x => x == typeof(IParameterizedItem));
            if (isParameterizedItem2) {
              isGenericParameterizedItem2 = true;
              break;
            }
          }

          if (isParameterizedItem || isGenericParameterizedItem2) {
            var actualValue = parameter.ActualValue;
            currentJson.Parameters = GetProblemParameter((actualValue as IParameterizedItem).Parameters, $"{previousPath}.{parameter.Name}"); // maybe we need to note that this is a generic argument
          }

          parameterJson.Add(currentJson);
        }
      }

      return json;
    }

    public async Task PostParameterValue(IHttpContext ctx) {
      StreamReader stream = new StreamReader(ctx.Request.InputStream);
      string requestBodyAsString = stream.ReadToEnd();
      var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(requestBodyAsString);

      string path = dict["path"];
      string typeToSet = dict["datatype"];
      Type propertyType = ReflectionUtil.GetType(typeToSet);
      string valueToSet = dict["value"];


      object property = FindProperty(Problem.Parameters, "Problem", path);
      if (property == null)
        property = FindProperty(Algorithm.Parameters, "Algorithm", path);

      //var newProperty = Activator.CreateInstance(propertyType, valueToSet);


      //TypeConverter typeConverter = TypeDescriptor.GetConverter(typeToSet);
      //object propValue = typeConverter.ConvertFromString(valueToSet);

      // !! alles valueparameter, constrainedvalueparameter mit validvalues, fixedvalueparameter könen vermutlich nicht geädnert werden


      // for ValueTypeValue:
      //parameter.GetType().GetMethod("SetValue", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Invoke(parameter,
      // fetch method from ValueType. This setter wants a BoolValue for bools and so on
      bool isValueParameter = ReflectionUtil.IsSubclassOfRawGeneric(typeof(ValueParameter<>), propertyType);
      bool isConstrainedValueParameter = ReflectionUtil.IsSubclassOfRawGeneric(typeof(ConstrainedValueParameter<>), propertyType);
      bool isFixedValueParameter = ReflectionUtil.IsSubclassOfRawGeneric(typeof(FixedValueParameter<>), propertyType);

      if (isValueParameter) {
        // we have to distinct between 'values' like BoolValue and between Interface values
        var genericTypeArgument = property.GetType().GetGenericArguments().FirstOrDefault();
        bool isGenericTypeArgumentInterface = genericTypeArgument.IsInterface;
        var valueTypeSetter = property.GetType().GetMethod("set_Value", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        // INTERFACE begin
        // need to get possible tyoes from endpoint getPossibleParameterValues before
        if (isGenericTypeArgumentInterface) {
          var possibleInstances = ApplicationManager.Manager.GetInstances(genericTypeArgument);
          var instanceToSet = possibleInstances.Where(x => x.GetType().FullName == valueToSet).FirstOrDefault();
          valueTypeSetter?.Invoke(property, new object[] { instanceToSet });

          // INTERFACE end
        } else {
          // VALUE begin
          
          // e.g. BoolValue

          var typeArgumentInstance = Activator.CreateInstance(genericTypeArgument);
          typeArgumentInstance.GetType().GetMethod("SetValue", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).Invoke(typeArgumentInstance, new object[] { valueToSet });
          valueTypeSetter?.Invoke(property, new object[] { typeArgumentInstance });
          // VALUE END
        }
      } else if (isConstrainedValueParameter) {
        // we have to get a enumerator for validValues with reflection, search for the right item by type and then we can assign it
        var validValuesMethod = property.GetType().GetMethod("get_ValidValues", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        object itemSet = validValuesMethod.Invoke(property, null);
        var getEnumeratorMethod = itemSet.GetType().GetMethod("GetEnumerator");
        IEnumerator<object> enumerator = (IEnumerator<object>)getEnumeratorMethod.Invoke(itemSet, null);
        object current = null;
        while (enumerator.MoveNext()) {
          current = enumerator.Current;
          Type currentType = current.GetType();
          if (currentType.FullName == valueToSet)
            break;
        }
        if (current.GetType().FullName != valueToSet) {
          ctx.Response.StatusCode = HttpStatusCode.BadRequest;
          await ctx.Response.SendResponseAsync($"Could not find type {valueToSet} in ValidValues from {path}.");
          return;
        }

        var setMethod = property.GetType().GetMethod("set_Value");
        setMethod.Invoke(property, new object[] { current });
        ;
        // todo
      }

      await ctx.Response.SendResponseAsync("ok");

    }

    public async Task GetParameterValue(IHttpContext ctx) {
      var parameterToLookFor = ctx.Request.QueryString["parameter"];
      object parameter = FindProperty(Problem.Parameters, "Problem", parameterToLookFor);
      if (parameter == null)
        parameter = FindProperty(Algorithm.Parameters, "Algorithm", parameterToLookFor);

      ctx.Response.ContentType = CONTENT_TYPE_JSON;
      if (parameter == null) {
        ctx.Response.StatusCode = HttpStatusCode.BadRequest;
        await ctx.Response.SendResponseAsync($"Parameter {parameterToLookFor} could not be found");
      }

      // is it safe to assume that every parameter is IValueParameter?
      IValueParameter valueParameter = parameter as IValueParameter;
      if (valueParameter == null) {
        ctx.Response.StatusCode = HttpStatusCode.BadRequest;
        await ctx.Response.SendResponseAsync($"Parameter {parameterToLookFor} is not a IValueParameter. Type: {parameter.GetType().FullName}");
      }


      dynamic json = new ExpandoObject();
      json.Name = valueParameter.Name;
      // for valuetypevalues we need to fetch their value by accessing the value of the property before..
      bool isValueValue = ReflectionUtil.IsSubclassOfRawGeneric(typeof(ValueTypeValue<>), valueParameter.Value?.GetType());
      if (isValueValue) {
        // we need to use reflection because we don't know the generic type and therefore can't cast the object
        var value = valueParameter.Value.GetType().GetProperty(nameof(IValueParameter.Value),
          BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(valueParameter.Value);
        json.Value = value;
      } else
        json.Value = valueParameter.Value;

      string serializedJson = JsonSerializer.Serialize(json);




      await ctx.Response.SendResponseAsync(serializedJson);
    }

    /// <summary>
    /// Returns all property paths and their datatype.
    /// </summary>
    public async Task GetPropertyPaths(IHttpContext ctx) {

      // Versuch

      dynamic json = new ExpandoObject();
      json.ProblemParameters = GetProblemParameter((IKeyedItemCollection<string, IParameter>)Problem.Parameters, "Problem");
      json.AlgorithmParameters = GetProblemParameter((IKeyedItemCollection<string, IParameter>)Algorithm.Parameters, "Algorithm");

      ctx.Response.ContentType = CONTENT_TYPE_JSON;
      string serializedJson = JsonSerializer.Serialize(json);
      await ctx.Response.SendResponseAsync(serializedJson);
      return;


      // Versucht Ende
      //dynamic json = new ExpandoObject();
      //var param = Problem.Parameters.LastOrDefault();
      //json.Name = param.Name;
      //json.Description = param.Description;
      //json.DataType = param.DataType.ToString();
      //json.Value = param.ActualValue.ToString();


      //ctx.Response.ContentType = CONTENT_TYPE_JSON;
      //string serializedJson = JsonSerializer.Serialize(json);
      //await ctx.Response.SendResponseAsync(serializedJson);
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
      ctx.Response.ContentType = CONTENT_TYPE_JSON;
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
      // TODO if type implements interface IConstrainedValueParameter use ValidValues for suggestions

      var pathParameters = ctx.Request.PathParameters;
      var queryStrings = ctx.Request.QueryString;
      var dataTypeString = queryStrings["dataType"];

      var type = ReflectionUtil.GetType(dataTypeString);
      dynamic json = new ExpandoObject();
      var possibleTypes = ApplicationManager.Manager.GetTypes(type);


      var superInstance = ApplicationManager.Manager.GetInstances(type);
      foreach (var i in superInstance) {
        bool isOptionaConstrainedValueParameter2 = ReflectionUtil.IsSubclassOfRawGeneric(typeof(OptionalConstrainedValueParameter<>), i.GetType());
        if (isOptionaConstrainedValueParameter2) {
          dynamic dasd = i;
          var ding = dasd.ValidValues;
        }
      }

      var hasValidVlaues = type.GetProperty(nameof(IConstrainedValueParameter<IItem>.ValidValues)) != null;
      if (hasValidVlaues) {
        ;
      }

      bool isOptionaConstrainedValueParameter = ReflectionUtil.IsSubclassOfRawGeneric(typeof(OptionalConstrainedValueParameter<>), type);
      if (isOptionaConstrainedValueParameter) {
        ;
        var instances = ApplicationManager.Manager.GetInstances(type);
        foreach (var instance in instances) {
          dynamic dynamicInstance = instance;
          var values = dynamicInstance.ValidValues;
          if (values != null) {
            ;
          }


        }
        ;
      }

      bool isTypeImplementingConstrainedValueParameters =
        type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConstrainedValueParameter<>));
      if (isTypeImplementingConstrainedValueParameters) {
        // TODO create object form type and append object.ValidValues to the json
        // TODO check if the ValidValues are the same things as json.PossibleTypes
        var instances = ApplicationManager.Manager.GetInstances(type);

        //json.recommendations = 
      }

      json.PossibleTypes = possibleTypes.Select(x => x.FullName).ToList();
      json.dataType = dataTypeString;

      ctx.Response.ContentType = CONTENT_TYPE_JSON;
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


      ctx.Response.ContentType = CONTENT_TYPE_JSON;
      string serializedJson = JsonSerializer.Serialize(json);
      await ctx.Response.SendResponseAsync(serializedJson);
    }

    public async Task GetResult(IHttpContext ctx) {
      Dictionary<string, IItem> results = new Dictionary<string, IItem>();
      Algorithm.Results.CollectResultValues(results);

      dynamic json = new ExpandoObject();
      List<dynamic> prettyResults = new List<dynamic>();

      int isValueTypeCount = 0;
      int isNotValueTypeCount = 0;
      foreach (var result in results) {

        bool isValueType = ReflectionUtil.IsSubclassOfRawGeneric(typeof(ValueTypeValue<>), result.Value.GetType());
        if (!isValueType) {
          isNotValueTypeCount++;
          continue;
        }
        isValueTypeCount++;
        dynamic current = new ExpandoObject();
        current.Key = result.Key;
        current.DataType = result.Value.GetType().FullName;
        current.ItemDescription = result.Value.ItemDescription;
        current.ItemName = result.Value.ItemName;
        current.Value = result.Value;
        prettyResults.Add(current);
      }

      json.Results = prettyResults;

      ctx.Response.ContentType = CONTENT_TYPE_JSON;

      var options = new JsonSerializerOptions {
        NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals
      };

      string serializedJson = JsonSerializer.Serialize(json, options);
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

      ctx.Response.ContentType = CONTENT_TYPE_JSON;
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
        bool isStringConvertibleArray = ReflectionUtil.IsSubclassOfRawGeneric(typeof(StringConvertibleArray<>), propertyToModify.Value.GetType());

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
          } else if (isStringConvertibleArray) {
            if (value.Contains(',')) {
              var newArray = Activator.CreateInstance(propertyToModify.Value.GetType(), new object[] { value.Split(',').Length });
              int i = 0;
              foreach (var val in value.Split(',')) {
                var setMethod = newArray.GetType().GetMethod("SetValue", BindingFlags.NonPublic | BindingFlags.Instance);
                var methods = newArray.GetType().GetMethods();
                //dynamic di = newArray;

                //var targetType = propertyToModify.Value.GetType().GenericTypeArguments.FirstOrDefault();
                //di[i] = Convert.ChangeType(val, targetType); 


                //di.SetValue(val, i);

                setMethod.Invoke(propertyToModify.Value, new object[] { val, i });
                propertyAsItem = (IItem)newArray;
              }
            }

          }  // TODO check if necessary - if nothing works anymore we need to remove the else
          Problem.Parameters[propertyName].ActualValue = propertyAsItem;
        }
        ctx.Response.ContentType = CONTENT_TYPE_JSON;
        await ctx.Response.SendResponseAsync("ok");

      } else {
        ctx.Response.ContentType = CONTENT_TYPE_JSON;
        ctx.Response.StatusCode = HttpStatusCode.BadRequest;
        await ctx.Response.SendResponseAsync($"Problem.Parameter {problemParameter} is not a ParameterizedNamedItem. It does not have a ParameterCollection.");
        return;
      }
    }

    public async Task GetExecutionState(IHttpContext ctx) {
      ctx.Response.ContentType = CONTENT_TYPE_JSON;
      ctx.Response.StatusCode = HttpStatusCode.Ok;
      await ctx.Response.SendResponseAsync(Algorithm.ExecutionState.ToString());
    }

    public async Task SetExecuteablePrepare(IHttpContext ctx) {
      Algorithm.Prepare();

      ctx.Response.ContentType = CONTENT_TYPE_JSON;
      ctx.Response.StatusCode = Algorithm.ExecutionState == ExecutionState.Prepared ? HttpStatusCode.Ok : HttpStatusCode.InternalServerError;
      await ctx.Response.SendResponseAsync("ok");
    }

    public async Task SetExecuteableStart(IHttpContext ctx) {
      // TODO maybe we should use the isStarted callback to provide a status to the user
      _ = Algorithm.StartAsync();

      ctx.Response.ContentType = CONTENT_TYPE_JSON;
      ctx.Response.StatusCode = Algorithm.ExecutionState == ExecutionState.Started ? HttpStatusCode.Ok : HttpStatusCode.InternalServerError;
      await ctx.Response.SendResponseAsync("ok");
    }

    public async Task SetExecuteablePause(IHttpContext ctx) {
      Algorithm.Pause();

      ctx.Response.ContentType = CONTENT_TYPE_JSON;
      ctx.Response.StatusCode = Algorithm.ExecutionState == ExecutionState.Paused ? HttpStatusCode.Ok : HttpStatusCode.InternalServerError;
      await ctx.Response.SendResponseAsync("ok");
    }

    public async Task SetExecuteableStop(IHttpContext ctx) {
      Algorithm.Stop();

      ctx.Response.ContentType = CONTENT_TYPE_JSON;
      ctx.Response.StatusCode = Algorithm.ExecutionState == ExecutionState.Stopped ? HttpStatusCode.Ok : HttpStatusCode.InternalServerError;
      await ctx.Response.SendResponseAsync("ok");
    }
  }
}
