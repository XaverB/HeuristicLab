using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeuristicLab.Core;
using HeuristicLab.PluginInfrastructure;

namespace HeuristicLab.RemoteControl.TestApp {
  class TreeNode {
    public string Name { get; set; }

    public List<TreeNode> Nodes;

    public List<TreeNode> Find(string nameToFind, bool recursive) {

      List<TreeNode> nodes = new List<TreeNode>();

      nodes.AddRange(Nodes.Where(n => n.Name == nameToFind).ToList());

      if(recursive)
      foreach (var node in Nodes) {
        nodes.AddRange(node.Find(nameToFind, true));
      }
      return nodes;
    }

    public TreeNode() {

    }

    public TreeNode(string name) {
      Name = name;
    }


  }

  class Program {
    static void Main(string[] args) {

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

      foreach(var node in rootNode.Nodes) {
        Console.WriteLine($"Found node: {node}");
      }

      //foreach (TreeNode topNode in rootNode.Nodes)
      //  treeNodes.Add(topNode);
      //foreach (var node in treeNodes)
      //  typesTreeView.Nodes.Add((TreeNode)node.Clone());
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
        if (rootNode.Find(fullName, true).Count > 0)
          continue;

        var categoryNode = new TreeNode(name) {
          Name = fullName
          //,Tag = rawName
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
            Name = fullName
            //,
            //Tag = rawName
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
