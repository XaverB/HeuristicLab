using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HeuristicLab.RemoteControl.TestPlugin {
  public class TreeNode {
    public string Name { get; set; }
    [JsonIgnore]
    public object Tag { get; set; }

    public List<TreeNode> Nodes { get; set; }

    public List<TreeNode> Find(string nameToFind, bool recursive) {

      List<TreeNode> nodes = new List<TreeNode>();

      nodes.AddRange(Nodes.Where(n => n.Name == nameToFind));

      if (recursive)
        foreach (var node in Nodes) {
          nodes.AddRange(node.Find(nameToFind, true));
        }

      return nodes;
    }

    public TreeNode() {
      Nodes = new List<TreeNode>();
    }

    public TreeNode(string name) {
      Nodes = new List<TreeNode>();
      Name = name;
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder();
      sb.Append($"- {Name}\n");
      foreach (var node in Nodes)
        sb.Append($"-- {node}\n");
     

      return sb.ToString();
    }

  }
}
