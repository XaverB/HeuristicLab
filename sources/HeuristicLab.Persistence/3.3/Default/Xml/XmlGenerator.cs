﻿using System.Collections.Generic;
using System;
using System.Text;
using HeuristicLab.Persistence.Interfaces;
using HeuristicLab.Persistence.Core;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using HeuristicLab.Tracing;
using HeuristicLab.Persistence.Core.Tokens;
using System.IO.Compression;

namespace HeuristicLab.Persistence.Default.Xml {


  /// <summary>
  /// Main entry point of persistence to XML. Use the static methods to serialize
  /// to a file or to a stream.
  /// </summary>
  public class XmlGenerator : GeneratorBase<string> {

    private int depth;
    private int Depth {
      get {
        return depth;
      }
      set {
        depth = value;
        prefix = new string(' ', depth * 2);
      }
    }

    private string prefix;


    /// <summary>
    /// Initializes a new instance of the <see cref="XmlGenerator"/> class.
    /// </summary>
    public XmlGenerator() {
      Depth = 0;
    }

    private enum NodeType { Start, End, Inline } ;

    private static void AddXmlTagContent(StringBuilder sb, string name, Dictionary<string, object> attributes) {
      sb.Append(name);
      foreach (var attribute in attributes) {
        if (attribute.Value != null && !string.IsNullOrEmpty(attribute.Value.ToString())) {
          sb.Append(' ');
          sb.Append(attribute.Key);
          sb.Append("=\"");
          sb.Append(attribute.Value);
          sb.Append('"');
        }
      }
    }

    private static void AddXmlStartTag(StringBuilder sb, string name, Dictionary<string, object> attributes) {
      sb.Append('<');
      AddXmlTagContent(sb, name, attributes);
      sb.Append('>');
    }

    private static void AddXmlInlineTag(StringBuilder sb, string name, Dictionary<string, object> attributes) {
      sb.Append('<');
      AddXmlTagContent(sb, name, attributes);
      sb.Append("/>");
    }

    private static void AddXmlEndTag(StringBuilder sb, string name) {
      sb.Append("</");
      sb.Append(name);
      sb.Append(">");
    }

    private string CreateNodeStart(string name, Dictionary<string, object> attributes) {
      StringBuilder sb = new StringBuilder();
      sb.Append(prefix);
      Depth += 1;
      AddXmlStartTag(sb, name, attributes);
      sb.Append("\r\n");
      return sb.ToString();
    }

    private string CreateNodeStart(string name) {
      return CreateNodeStart(name, new Dictionary<string, object>());
    }

    private string CreateNodeEnd(string name) {
      Depth -= 1;
      StringBuilder sb = new StringBuilder();
      sb.Append(prefix);
      AddXmlEndTag(sb, name);
      sb.Append("\r\n");
      return sb.ToString();
    }

    private string CreateNode(string name, Dictionary<string, object> attributes) {
      StringBuilder sb = new StringBuilder();
      sb.Append(prefix);
      AddXmlInlineTag(sb, name, attributes);
      sb.Append("\r\n");
      return sb.ToString();
    }

    private string CreateNode(string name, Dictionary<string, object> attributes, string content) {
      StringBuilder sb = new StringBuilder();
      sb.Append(prefix);
      AddXmlStartTag(sb, name, attributes);
      sb.Append(content);
      sb.Append("</").Append(name).Append(">\r\n");
      return sb.ToString();
    }

    protected override string Format(BeginToken beginToken) {
      var dict = new Dictionary<string, object> {
          {"name", beginToken.Name},
          {"typeId", beginToken.TypeId},
          {"id", beginToken.Id}};
      AddTypeInfo(beginToken.TypeId, dict);
      return CreateNodeStart(XmlStringConstants.COMPOSITE, dict);
        
    }

    private void AddTypeInfo(int typeId, Dictionary<string, object> dict) {
      if (lastTypeToken != null) {
        if (typeId == lastTypeToken.Id) {
          dict.Add("typeName", lastTypeToken.TypeName);
          dict.Add("serializer", lastTypeToken.Serializer);
          lastTypeToken = null;
        } else {
          FlushTypeToken();
        }
      }
    }

    protected override string Format(EndToken endToken) {
      return CreateNodeEnd(XmlStringConstants.COMPOSITE);
    }

    protected override string Format(PrimitiveToken dataToken) {
      var dict = new Dictionary<string, object> {
            {"typeId", dataToken.TypeId},
            {"name", dataToken.Name},
            {"id", dataToken.Id}};
      AddTypeInfo(dataToken.TypeId, dict);
      return CreateNode(XmlStringConstants.PRIMITIVE, dict,
        ((XmlString)dataToken.SerialData).Data);
    }

    protected override string Format(ReferenceToken refToken) {
      return CreateNode(XmlStringConstants.REFERENCE,
        new Dictionary<string, object> {
          {"ref", refToken.Id},
          {"name", refToken.Name}});
    }

    protected override string Format(NullReferenceToken nullRefToken) {
      return CreateNode(XmlStringConstants.NULL,
        new Dictionary<string, object>{
          {"name", nullRefToken.Name}});
    }

    protected override string Format(MetaInfoBeginToken metaInfoBeginToken) {
      return CreateNodeStart(XmlStringConstants.METAINFO);
    }

    protected override string Format(MetaInfoEndToken metaInfoEndToken) {
      return CreateNodeEnd(XmlStringConstants.METAINFO);
    }

    private TypeToken lastTypeToken;
    protected override string Format(TypeToken token) {
      lastTypeToken = token;
      return "";
    }

    private string FlushTypeToken() {
      if (lastTypeToken == null)
        return "";
      try {
        return CreateNode(XmlStringConstants.TYPE,
          new Dictionary<string, object> {
          {"id", lastTypeToken.Id},
          {"typeName", lastTypeToken.TypeName },
          {"serializer", lastTypeToken.Serializer }});
      } finally {
        lastTypeToken = null;
      }
    }

    public IEnumerable<string> Format(List<TypeMapping> typeCache) {
      yield return CreateNodeStart(XmlStringConstants.TYPECACHE);
      foreach (var mapping in typeCache)
        yield return CreateNode(
          XmlStringConstants.TYPE,
          mapping.GetDict());
      yield return CreateNodeEnd(XmlStringConstants.TYPECACHE);
    }

    /// <summary>
    /// Serialize an object into a file.
    /// 
    /// The XML configuration is obtained from the <c>ConfigurationService</c>.
    /// The file is actually a ZIP file.
    /// Compression level is set to 5 and needed assemblies are not included.
    /// </summary>    
    public static void Serialize(object o, string filename) {
      Serialize(o, filename, ConfigurationService.Instance.GetConfiguration(new XmlFormat()), false, 5);
    }

    /// <summary>
    /// Serialize an object into a file.
    /// 
    /// The XML configuration is obtained from the <c>ConfigurationService</c>.
    /// Needed assemblies are not included.
    /// </summary>
    /// <param name="compression">ZIP file compression level</param>
    public static void Serialize(object o, string filename, int compression) {
      Serialize(o, filename, ConfigurationService.Instance.GetConfiguration(new XmlFormat()), false, compression);
    }

    public static void Serialize(object obj, string filename, Configuration config) {
      Serialize(obj, filename, config, false, 5);
    }

    public static void Serialize(object obj, string filename, Configuration config, bool includeAssemblies, int compression) {      
      try {
        string tempfile = Path.GetTempFileName();
        DateTime start = DateTime.Now;
        using (FileStream stream = File.Create(tempfile)) {
          Serializer serializer = new Serializer(obj, config);
          serializer.InterleaveTypeInformation = false;
          XmlGenerator generator = new XmlGenerator();
          using (ZipOutputStream zipStream = new ZipOutputStream(stream)) {
            zipStream.IsStreamOwner = false;
            zipStream.SetLevel(compression);
            zipStream.PutNextEntry(new ZipEntry("data.xml") { DateTime = DateTime.MinValue });
            StreamWriter writer = new StreamWriter(zipStream);
            foreach (ISerializationToken token in serializer) {
              string line = generator.Format(token);
              writer.Write(line);
            }
            writer.Flush();
            zipStream.PutNextEntry(new ZipEntry("typecache.xml") { DateTime = DateTime.MinValue });
            foreach (string line in generator.Format(serializer.TypeCache)) {
              writer.Write(line);
            }
            writer.Flush();
            if (includeAssemblies) {
              foreach (string name in serializer.RequiredFiles) {
                Uri uri = new Uri(name);
                if (!uri.IsFile) {
                  Logger.Warn("cannot read non-local files");
                  continue;
                }
                zipStream.PutNextEntry(new ZipEntry(Path.GetFileName(uri.PathAndQuery)));
                FileStream reader = File.OpenRead(uri.PathAndQuery);
                byte[] buffer = new byte[1024 * 1024];
                while (true) {
                  int bytesRead = reader.Read(buffer, 0, 1024 * 1024);
                  if (bytesRead == 0)
                    break;
                  zipStream.Write(buffer, 0, bytesRead);
                }
                writer.Flush();
              }
            }
          }
        }
        Logger.Info(String.Format("serialization took {0} seconds with compression level {1}",
          (DateTime.Now - start).TotalSeconds, compression));
        File.Copy(tempfile, filename, true);
        File.Delete(tempfile);
      } catch (Exception) {
        Logger.Warn("Exception caught, no data has been written.");
        throw;
      }
    }

    public static void Serialize(object obj, Stream stream) {
      Serialize(obj, stream, ConfigurationService.Instance.GetConfiguration(new XmlFormat()));
    }


    public static void Serialize(object obj, Stream stream, Configuration config) {
      Serialize(obj, stream, config, false);
    }

    public static void Serialize(object obj, Stream stream, Configuration config, bool includeAssemblies) {
      Serialize(obj, stream, config, includeAssemblies, true);
    }

    public static void Serialize(object obj, Stream stream, Configuration config, bool includeAssemblies, bool interleaveTypeInfo) {      
      try {
        using (StreamWriter writer = new StreamWriter(new GZipStream(stream, CompressionMode.Compress))) {
          Serializer serializer = new Serializer(obj, config);
          serializer.InterleaveTypeInformation = true;
          XmlGenerator generator = new XmlGenerator();
          foreach (ISerializationToken token in serializer) {
            string line = generator.Format(token);
            writer.Write(line);
          }
          writer.Flush();
        }
      } catch (PersistenceException) {
        throw;
      } catch (Exception e) {
        throw new PersistenceException("Unexpected exception during Serialization.", e);
      }
    }
  }
}