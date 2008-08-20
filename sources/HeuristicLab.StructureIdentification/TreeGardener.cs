#region License Information
/* HeuristicLab
 * Copyright (C) 2002-2008 Heuristic and Evolutionary Algorithms Laboratory (HEAL)
 *
 * This file is part of HeuristicLab.
 *
 * HeuristicLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * HeuristicLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with HeuristicLab. If not, see <http://www.gnu.org/licenses/>.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using HeuristicLab.Core;
using HeuristicLab.Constraints;
using System.Diagnostics;
using HeuristicLab.Data;
using System.Linq;
using HeuristicLab.Random;
using HeuristicLab.Operators;
using HeuristicLab.Selection;
using HeuristicLab.Functions;
using System.Collections;

namespace HeuristicLab.StructureIdentification {
  internal class TreeGardener {
    private IRandom random;
    private GPOperatorLibrary funLibrary;
    private List<IFunction> functions;

    private List<IFunction> terminals;
    internal IList<IFunction> Terminals {
      get { return terminals; }
    }

    private List<IFunction> allFunctions;
    internal IList<IFunction> AllFunctions {
      get { return allFunctions; }
    }

    #region constructors
    internal TreeGardener(IRandom random, GPOperatorLibrary funLibrary) {
      this.random = random;
      this.funLibrary = funLibrary;
      this.allFunctions = new List<IFunction>();
      terminals = new List<IFunction>();
      functions = new List<IFunction>();
      // init functions and terminals based on constraints
      foreach(IFunction fun in funLibrary.Group.Operators) {
        if(fun.MaxArity == 0) {
          terminals.Add(fun);
          allFunctions.Add(fun);
        } else {
          functions.Add(fun);
          allFunctions.Add(fun);
        }
      }
    }
    #endregion

    #region random initialization
    /// <summary>
    /// Creates a random balanced tree with a maximal size and height. When the max-height or max-size are 1 it will return a random terminal.
    /// In other cases it will return either a terminal (tree of size 1) or any other tree with a function in it's root (at least height 2).
    /// </summary>
    /// <param name="maxTreeSize">Maximal size of the tree (number of nodes).</param>
    /// <param name="maxTreeHeight">Maximal height of the tree.</param>
    /// <returns></returns>
    internal IFunctionTree CreateBalancedRandomTree(int maxTreeSize, int maxTreeHeight) {
      IFunction rootFunction = GetRandomRoot(maxTreeSize, maxTreeHeight);
      IFunctionTree tree = MakeBalancedTree(rootFunction, maxTreeHeight - 1);
      return tree;
    }

    /// <summary>
    /// Creates a random (unbalanced) tree with a maximal size and height. When the max-height or max-size are 1 it will return a random terminal.
    /// In other cases it will return either a terminal (tree of size 1) or any other tree with a function in it's root (at least height 2).
    /// </summary>
    /// <param name="maxTreeSize">Maximal size of the tree (number of nodes).</param>
    /// <param name="maxTreeHeight">Maximal height of the tree.</param>
    /// <returns></returns>
    internal IFunctionTree CreateUnbalancedRandomTree(int maxTreeSize, int maxTreeHeight) {
      IFunction rootFunction = GetRandomRoot(maxTreeSize, maxTreeHeight);
      IFunctionTree tree = MakeUnbalancedTree(rootFunction, maxTreeHeight - 1);
      return tree;
    }

    internal IFunctionTree PTC2(IRandom random, int size, int maxDepth) {
      return PTC2(random, GetRandomRoot(size, maxDepth), size, maxDepth);
    }

    internal IFunctionTree PTC2(IRandom random, IFunction rootF, int size, int maxDepth) {
      IFunctionTree root = rootF.GetTreeNode();
      if(size <= 1 || maxDepth <= 1) return root;
      List<object[]> list = new List<object[]>();
      int currentSize = 1;
      int totalListMinSize = 0;
      int minArity = root.Function.MinArity;
      int maxArity = root.Function.MaxArity;
      if(maxArity >= size) {
        maxArity = size;
      }
      int actualArity = random.Next(minArity, maxArity + 1);
      totalListMinSize += GetMinimalTreeSize(root.Function) - 1;
      for(int i = 0; i < actualArity; i++) {
        // insert a dummy sub-tree and add the pending extension to the list
        root.AddSubTree(null);
        list.Add(new object[] { root, i, 2 });
      }

      while(list.Count > 0 && totalListMinSize + currentSize < size) {
        int randomIndex = random.Next(list.Count);
        object[] nextExtension = list[randomIndex];
        list.RemoveAt(randomIndex);
        IFunctionTree parent = (IFunctionTree)nextExtension[0];
        int a = (int)nextExtension[1];
        int d = (int)nextExtension[2];
        if(d == maxDepth) {
          parent.RemoveSubTree(a);
          IFunctionTree branch = CreateRandomTree(GetAllowedSubFunctions(parent.Function, a), 1, 1);
          parent.InsertSubTree(a, branch); // insert a smallest possible tree
          currentSize += branch.Size;
          totalListMinSize -= branch.Size;
        } else {
          IFunction selectedFunction = RandomSelect(GetAllowedSubFunctions(parent.Function, a).Where(
            f => !IsTerminal(f) && GetMinimalTreeHeight(f) + (d - 1) <= maxDepth).ToArray());
          IFunctionTree newTree = selectedFunction.GetTreeNode();
          parent.RemoveSubTree(a);
          parent.InsertSubTree(a, newTree);
          currentSize++;
          totalListMinSize--;

          minArity = selectedFunction.MinArity;
          maxArity = selectedFunction.MaxArity;
          if(maxArity >= size) {
            maxArity = size;
          }
          actualArity = random.Next(minArity, maxArity + 1);
          for(int i = 0; i < actualArity; i++) {
            // insert a dummy sub-tree and add the pending extension to the list
            newTree.AddSubTree(null);
            list.Add(new object[] { newTree, i, d + 1 });
          }
          totalListMinSize += GetMinimalTreeSize(newTree.Function) - 1;
        }
      }
      while(list.Count > 0) {
        int randomIndex = random.Next(list.Count);
        object[] nextExtension = list[randomIndex];
        list.RemoveAt(randomIndex);
        IFunctionTree parent = (IFunctionTree)nextExtension[0];
        int a = (int)nextExtension[1];
        int d = (int)nextExtension[2];
        parent.RemoveSubTree(a);
        parent.InsertSubTree(a,
          CreateRandomTree(GetAllowedSubFunctions(parent.Function, a), 1, 1)); // append a tree with minimal possible height
      }
      return root;
    }

    /// <summary>
    /// selects a random function from allowedFunctions and creates a random (unbalanced) tree with maximal size and height.
    /// </summary>
    /// <param name="allowedFunctions">Set of allowed functions.</param>
    /// <param name="maxTreeSize">Maximal size of the tree (number of nodes).</param>
    /// <param name="maxTreeHeight">Maximal height of the tree.</param>
    /// <returns>New random unbalanced tree</returns>
    internal IFunctionTree CreateRandomTree(ICollection<IFunction> allowedFunctions, int maxTreeSize, int maxTreeHeight) {
      // get the minimal needed height based on allowed functions and extend the max-height if necessary
      int minTreeHeight = allowedFunctions.Select(f => GetMinimalTreeHeight(f)).Min();
      if(minTreeHeight > maxTreeHeight)
        maxTreeHeight = minTreeHeight;
      // get the minimal needed size based on allowed functions and extend the max-size if necessary
      int minTreeSize = allowedFunctions.Select(f => GetMinimalTreeSize(f)).Min();
      if(minTreeSize > maxTreeSize)
        maxTreeSize = minTreeSize;

      // select a random value for the size and height
      int treeHeight = random.Next(minTreeHeight, maxTreeHeight + 1);
      int treeSize = random.Next(minTreeSize, maxTreeSize + 1);

      // filter the set of allowed functions and select only from those that fit into the given maximal size and height limits
      IFunction[] possibleFunctions = allowedFunctions.Where(f => GetMinimalTreeHeight(f) <= treeHeight &&
        GetMinimalTreeSize(f) <= treeSize).ToArray();
      IFunction selectedFunction = RandomSelect(possibleFunctions);

      // build the tree 
      IFunctionTree root;
      root = PTC2(random, selectedFunction, maxTreeSize, maxTreeHeight);
      return root;
    }

    internal CompositeOperation CreateInitializationOperation(ICollection<IFunctionTree> trees, IScope scope) {
      // needed for the parameter shaking operation
      CompositeOperation initializationOperation = new CompositeOperation();
      Scope tempScope = new Scope("Temp. initialization scope");

      var parametricTrees = trees.Where(t => t.Function.GetVariable(FunctionBase.INITIALIZATION) != null);
      foreach(IFunctionTree tree in parametricTrees) {
        // enqueue an initialization operation for each operator with local variables 
        IOperator initialization = (IOperator)tree.Function.GetVariable(FunctionBase.INITIALIZATION).Value;
        Scope initScope = new Scope();
        // copy the local variables into a temporary scope used for initialization
        foreach(IVariable variable in tree.LocalVariables) {
          initScope.AddVariable(variable);
        }
        tempScope.AddSubScope(initScope);
        initializationOperation.AddOperation(new AtomicOperation(initialization, initScope));
      }
      Scope backupScope = new Scope("backup");
      foreach(Scope subScope in scope.SubScopes) {
        backupScope.AddSubScope(subScope);
      }
      scope.AddSubScope(tempScope);
      scope.AddSubScope(backupScope);
      // add an operation to remove the temporary scopes        
      initializationOperation.AddOperation(new AtomicOperation(new RightReducer(), scope));
      return initializationOperation;
    }
    #endregion

    #region tree information gathering
    internal IFunctionTree GetRandomParentNode(IFunctionTree tree) {
      List<IFunctionTree> parentNodes = new List<IFunctionTree>();

      // add null for the parent of the root node
      parentNodes.Add(null);

      TreeForEach(tree, delegate(IFunctionTree possibleParentNode) {
        if(possibleParentNode.SubTrees.Count > 0) {
          parentNodes.Add(possibleParentNode);
        }
      });

      return parentNodes[random.Next(parentNodes.Count)];
    }

    internal ICollection<IFunctionTree> GetAllSubTrees(IFunctionTree root) {
      List<IFunctionTree> allTrees = new List<IFunctionTree>();
      TreeForEach(root, t => { allTrees.Add(t); });
      return allTrees;
    }

    /// <summary>
    /// returns the height level of branch in the tree
    /// if the branch == tree => 1
    /// if branch is in the sub-trees of tree => 2
    /// ...
    /// if branch is not found => -1
    /// </summary>
    /// <param name="tree">root of the function tree to process</param>
    /// <param name="branch">branch that is searched in the tree</param>
    /// <returns></returns>
    internal int GetBranchLevel(IFunctionTree tree, IFunctionTree branch) {
      return GetBranchLevelHelper(tree, branch, 1);
    }

    // 'tail-recursive' helper
    private int GetBranchLevelHelper(IFunctionTree tree, IFunctionTree branch, int level) {
      if(branch == tree) return level;

      foreach(IFunctionTree subTree in tree.SubTrees) {
        int result = GetBranchLevelHelper(subTree, branch, level + 1);
        if(result != -1) return result;
      }

      return -1;
    }

    internal bool IsValidTree(IFunctionTree tree) {
      for(int i = 0; i < tree.SubTrees.Count; i++) {
        if(!tree.Function.AllowedSubFunctions(i).Contains(tree.SubTrees[i].Function)) return false;
      }

      if(tree.SubTrees.Count < tree.Function.MinArity || tree.SubTrees.Count > tree.Function.MaxArity)
        return false;
      foreach(IFunctionTree subTree in tree.SubTrees) {
        if(!IsValidTree(subTree)) return false;
      }
      return true;
    }

    // returns a random branch from the specified level in the tree
    internal IFunctionTree GetRandomBranch(IFunctionTree tree, int level) {
      if(level == 0) return tree;
      List<IFunctionTree> branches = GetBranchesAtLevel(tree, level);
      return branches[random.Next(branches.Count)];
    }
    #endregion

    #region function information (arity, allowed childs and parents)
    internal ICollection<IFunction> GetPossibleParents(List<IFunction> list) {
      List<IFunction> result = new List<IFunction>();
      foreach(IFunction f in functions) {
        if(IsPossibleParent(f, list)) {
          result.Add(f);
        }
      }
      return result;
    }

    private bool IsPossibleParent(IFunction f, List<IFunction> children) {
      int minArity = f.MinArity;
      int maxArity = f.MaxArity;
      // note: we can't assume that the operators in the children list have different types!

      // when the maxArity of this function is smaller than the list of operators that
      // should be included as sub-operators then it can't be a parent
      if(maxArity < children.Count()) {
        return false;
      }
      int nSlots = Math.Max(minArity, children.Count);

      List<HashSet<IFunction>> slotSets = new List<HashSet<IFunction>>();

      // we iterate through all slots for sub-trees and calculate the set of 
      // allowed functions for this slot.
      // we only count those slots that can hold at least one of the children that we should combine
      for(int slot = 0; slot < nSlots; slot++) {
        HashSet<IFunction> functionSet = new HashSet<IFunction>(f.AllowedSubFunctions(slot));
        if(functionSet.Count() > 0) {
          slotSets.Add(functionSet);
        }
      }

      // ok at the end of this operation we know how many slots of the parent can actually
      // hold one of our children.
      // if the number of slots is smaller than the number of children we can be sure that
      // we can never combine all children as sub-trees of the function and thus the function
      // can't be a parent.
      if(slotSets.Count() < children.Count()) {
        return false;
      }

      // finally we sort the sets by size and beginning from the first set select one
      // function for the slot and thus remove it as possible sub-tree from the remaining sets.
      // when we can successfully assign all available children to a slot the function is a valid parent
      // when only a subset of all children can be assigned to slots the function is no valid parent
      slotSets.Sort((p, q) => p.Count() - q.Count());

      int assignments = 0;
      for(int i = 0; i < slotSets.Count() - 1; i++) {
        if(slotSets[i].Count > 0) {
          IFunction selected = slotSets[i].ElementAt(0);
          assignments++;
          for(int j = i + 1; j < slotSets.Count(); j++) {
            slotSets[j].Remove(selected);
          }
        }
      }

      // sanity check 
      if(assignments > children.Count) throw new InvalidProgramException();
      return assignments == children.Count - 1;
    }
    internal IList<IFunction> GetAllowedParents(IFunction child, int childIndex) {
      List<IFunction> parents = new List<IFunction>();
      foreach(IFunction function in functions) {
        ICollection<IFunction> allowedSubFunctions = GetAllowedSubFunctions(function, childIndex);
        if(allowedSubFunctions.Contains(child)) {
          parents.Add(function);
        }
      }
      return parents;
    }
    internal bool IsTerminal(IFunction f) {
      return f.MinArity == 0 && f.MaxArity == 0;
    }
    internal IList<IFunction> GetAllowedSubFunctions(IFunction f, int index) {
      if(f == null) {
        return allFunctions;
      } else {
        return f.AllowedSubFunctions(index);
      }
    }
    #endregion

    #region private utility methods
    private IFunction GetRandomRoot(int maxTreeSize, int maxTreeHeight) {
      if(maxTreeHeight == 1 || maxTreeSize == 1) {
        IFunction selectedTerminal = RandomSelect(terminals);
        return selectedTerminal;
      } else {
        IFunction[] possibleFunctions = functions.Where(f => GetMinimalTreeHeight(f) <= maxTreeHeight &&
          GetMinimalTreeSize(f) <= maxTreeSize).ToArray();
        IFunction selectedFunction = RandomSelect(possibleFunctions);
        return selectedFunction;
      }
    }


    private IFunctionTree MakeUnbalancedTree(IFunction parent, int maxTreeHeight) {
      if(maxTreeHeight == 0) return parent.GetTreeNode();
      int minArity = parent.MinArity;
      int maxArity = parent.MaxArity;
      int actualArity = random.Next(minArity, maxArity + 1);
      if(actualArity > 0) {
        IFunctionTree parentTree = parent.GetTreeNode();
        for(int i = 0; i < actualArity; i++) {
          IFunction[] possibleFunctions = GetAllowedSubFunctions(parent, i).Where(f => GetMinimalTreeHeight(f) <= maxTreeHeight).ToArray();
          IFunction selectedFunction = RandomSelect(possibleFunctions);
          IFunctionTree newSubTree = MakeUnbalancedTree(selectedFunction, maxTreeHeight - 1);
          parentTree.InsertSubTree(i, newSubTree);
        }
        return parentTree;
      }
      return parent.GetTreeNode();
    }


    // NOTE: this method doesn't build fully balanced trees because we have constraints on the
    // types of possible sub-functions which can indirectly impose a limit for the depth of a given sub-tree
    private IFunctionTree MakeBalancedTree(IFunction parent, int maxTreeHeight) {
      if(maxTreeHeight == 0) return parent.GetTreeNode();
      int minArity = parent.MinArity;
      int maxArity = parent.MaxArity;
      int actualArity = random.Next(minArity, maxArity + 1);
      if(actualArity > 0) {
        IFunctionTree parentTree = parent.GetTreeNode();
        for(int i = 0; i < actualArity; i++) {
          // first try to find a function that fits into the maxHeight limit
          IFunction[] possibleFunctions = GetAllowedSubFunctions(parent, i).Where(f => GetMinimalTreeHeight(f) <= maxTreeHeight &&
            !IsTerminal(f)).ToArray();
          // no possible function found => extend function set to terminals
          if(possibleFunctions.Length == 0) {
            possibleFunctions = GetAllowedSubFunctions(parent, i).Where(f => IsTerminal(f)).ToArray();
            IFunction selectedTerminal = RandomSelect(possibleFunctions);
            IFunctionTree newTree = selectedTerminal.GetTreeNode();
            parentTree.InsertSubTree(i, newTree);
          } else {
            IFunction selectedFunction = RandomSelect(possibleFunctions);
            IFunctionTree newTree = MakeBalancedTree(selectedFunction, maxTreeHeight - 1);
            parentTree.InsertSubTree(i, newTree);
          }
        }
        return parentTree;
      }
      return parent.GetTreeNode();
    }

    private int GetMinimalTreeHeight(IOperator op) {
      return ((IntData)op.GetVariable(GPOperatorLibrary.MIN_TREE_HEIGHT).Value).Data;
    }

    private int GetMinimalTreeSize(IOperator op) {
      return ((IntData)op.GetVariable(GPOperatorLibrary.MIN_TREE_SIZE).Value).Data;
    }

    private void TreeForEach(IFunctionTree tree, Action<IFunctionTree> action) {
      action(tree);
      foreach(IFunctionTree subTree in tree.SubTrees) {
        TreeForEach(subTree, action);
      }
    }

    private List<IFunctionTree> GetBranchesAtLevel(IFunctionTree tree, int level) {
      if(level == 1) return new List<IFunctionTree>(tree.SubTrees);

      List<IFunctionTree> branches = new List<IFunctionTree>();
      foreach(IFunctionTree subTree in tree.SubTrees) {
        if(subTree.Height >= level - 1)
          branches.AddRange(GetBranchesAtLevel(subTree, level - 1));
      }
      return branches;
    }

    private IFunction RandomSelect(IList<IFunction> functionSet) {
      double[] accumulatedTickets = new double[functionSet.Count];
      double ticketAccumulator = 0;
      int i = 0;
      // precalculate the slot-sizes
      foreach(IFunction function in functionSet) {
        ticketAccumulator += ((DoubleData)function.GetVariable(GPOperatorLibrary.TICKETS).Value).Data;
        accumulatedTickets[i] = ticketAccumulator;
        i++;
      }
      // throw ball
      double r = random.NextDouble() * ticketAccumulator;
      // find the slot that has been hit
      for(i = 0; i < accumulatedTickets.Length; i++) {
        if(r < accumulatedTickets[i]) return functionSet[i];
      }
      // sanity check
      throw new InvalidProgramException(); // should never happen
    }

    #endregion

  }
}
