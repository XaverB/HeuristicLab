﻿using HeuristicLab.Permutation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeuristicLab.Core;

namespace HeuristicLab.Permutation.Tests
{
    
    
    /// <summary>
    ///This is a test class for OrderCrossoverTest and is intended
    ///to contain all OrderCrossoverTest Unit Tests
    ///</summary>
  [TestClass()]
  public class OrderCrossoverTest {


    private TestContext testContextInstance;

    /// <summary>
    ///Gets or sets the test context which provides
    ///information about and functionality for the current test run.
    ///</summary>
    public TestContext TestContext {
      get {
        return testContextInstance;
      }
      set {
        testContextInstance = value;
      }
    }

    #region Additional test attributes
    // 
    //You can use the following additional attributes as you write your tests:
    //
    //Use ClassInitialize to run code before running the first test in the class
    //[ClassInitialize()]
    //public static void MyClassInitialize(TestContext testContext)
    //{
    //}
    //
    //Use ClassCleanup to run code after all tests in a class have run
    //[ClassCleanup()]
    //public static void MyClassCleanup()
    //{
    //}
    //
    //Use TestInitialize to run code before running each test
    //[TestInitialize()]
    //public void MyTestInitialize()
    //{
    //}
    //
    //Use TestCleanup to run code after each test has run
    //[TestCleanup()]
    //public void MyTestCleanup()
    //{
    //}
    //
    #endregion


    /// <summary>
    ///A test for Cross
    ///</summary>
    [TestMethod()]
    [DeploymentItem("HeuristicLab.Permutation-3.3.dll")]
    public void OrderCrossoverCrossTest() {
      TestRandom random = new TestRandom();
      Permutation parent1, parent2, expected, actual;
      ItemArray<Permutation> parents;
      OrderCrossover_Accessor target = new OrderCrossover_Accessor(new PrivateObject(typeof(OrderCrossover)));
      // The following test is based on an example from Eiben, A.E. and Smith, J.E. 2003. Introduction to Evolutionary Computation. Natural Computing Series, Springer-Verlag Berlin Heidelberg, pp. 55-56
      random.Reset();
      random.IntNumbers = new int[] { 3, 6 };
      parent1 = new Permutation(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
      Assert.IsTrue(parent1.Validate());
      parent2 = new Permutation(new int[] { 8, 2, 6, 7, 1, 5, 4, 0, 3 });
      Assert.IsTrue(parent2.Validate());
      parents = new ItemArray<Permutation>(new Permutation[] { parent1, parent2 });
      expected = new Permutation(new int[] { 2, 7, 1, 3, 4, 5, 6, 0, 8 });
      Assert.IsTrue(expected.Validate());
      actual = target.Cross(random, parents);
      Assert.IsTrue(actual.Validate());
      Assert.IsTrue(Auxiliary.PermutationIsEqualByPosition(expected, actual));
      // The following test is based on an example from Larranaga, P. et al. 1999. Genetic Algorithms for the Travelling Salesman Problem: A Review of Representations and Operators. Artificial Intelligence Review, 13, pp. 129-170.
      random.Reset();
      random.IntNumbers = new int[] { 2, 4 };
      parent1 = new Permutation(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });
      Assert.IsTrue(parent1.Validate());
      parent2 = new Permutation(new int[] { 1, 3, 5, 7, 6, 4, 2, 0 });
      Assert.IsTrue(parent2.Validate());
      parents = new ItemArray<Permutation>(new Permutation[] { parent1, parent2 });
      expected = new Permutation(new int[] { 7, 6, 2, 3, 4, 0, 1, 5 });
      actual = target.Cross(random, parents);
      Assert.IsTrue(actual.Validate());
      Assert.IsTrue(Auxiliary.PermutationIsEqualByPosition(expected, actual));
      // The following test is based on an example from Talbi, E.G. 2009. Metaheuristics - From Design to Implementation. Wiley, p. 218.
      random.Reset();
      random.IntNumbers = new int[] { 2, 5 };
      parent1 = new Permutation(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
      Assert.IsTrue(parent1.Validate());
      parent2 = new Permutation(new int[] { 7, 3, 0, 4, 8, 2, 5, 1, 6 });
      Assert.IsTrue(parent2.Validate());
      parents = new ItemArray<Permutation>(new Permutation[] { parent1, parent2 });
      expected = new Permutation(new int[] { 0, 8, 2, 3, 4, 5, 1, 6, 7 });
      Assert.IsTrue(expected.Validate());
      actual = target.Cross(random, parents);
      Assert.IsTrue(actual.Validate());
      Assert.IsTrue(Auxiliary.PermutationIsEqualByPosition(expected, actual));
      // The following test is not based on published examples
      random.Reset();
      random.IntNumbers = new int[] { 0, 5 };
      parent1 = new Permutation(new int[] { 2, 1, 4, 3, 7, 8, 6, 0, 5, 9 });
      Assert.IsTrue(parent1.Validate());
      parent2 = new Permutation(new int[] { 5, 3, 4, 0, 9, 8, 2, 7, 1, 6 });
      Assert.IsTrue(parent2.Validate());
      parents = new ItemArray<Permutation>(new Permutation[] { parent1, parent2 });
      expected = new Permutation(new int[] { 2, 1, 4, 3, 7, 8, 6, 5, 0, 9 });
      Assert.IsTrue(expected.Validate());
      actual = target.Cross(random, parents);
      Assert.IsTrue(actual.Validate());
      Assert.IsTrue(Auxiliary.PermutationIsEqualByPosition(expected, actual));
      // based on the previous with changed breakpoints
      random.Reset();
      random.IntNumbers = new int[] { 6, 9 };
      expected = new Permutation(new int[] { 3, 4, 8, 2, 7, 1, 6, 0, 5, 9 });
      Assert.IsTrue(expected.Validate());
      actual = target.Cross(random, parents);
      Assert.IsTrue(actual.Validate());
      Assert.IsTrue(Auxiliary.PermutationIsEqualByPosition(expected, actual));
      // another one based on the previous with changed breakpoints
      random.Reset();
      random.IntNumbers = new int[] { 0, 9 };
      expected = new Permutation(new int[] { 2, 1, 4, 3, 7, 8, 6, 0, 5, 9 });
      Assert.IsTrue(expected.Validate());
      actual = target.Cross(random, parents);
      Assert.IsTrue(actual.Validate());
      Assert.IsTrue(Auxiliary.PermutationIsEqualByPosition(expected, actual));
      // perform a test with more than two parents
      random.Reset();
      bool exceptionFired = false;
      try {
        target.Cross(random, new ItemArray<Permutation>(new Permutation[] { new Permutation(4), new Permutation(4), new Permutation(4)}));
      } catch (System.InvalidOperationException) {
        exceptionFired = true;
      }
      Assert.IsTrue(exceptionFired);
      // perform a test when two permutations are of unequal length
      random.Reset();
      exceptionFired = false;
      try {
        target.Cross(random, new ItemArray<Permutation>(new Permutation[] { new Permutation(8), new Permutation(6) }));
      } catch (System.ArgumentException) {
        exceptionFired = true;
      }
      Assert.IsTrue(exceptionFired);
    }

    /// <summary>
    ///A test for Apply
    ///</summary>
    [TestMethod()]
    public void OrderCrossoverApplyTest() {
      TestRandom random = new TestRandom();
      Permutation parent1, parent2, expected, actual;
      // The following test is based on an example from Eiben, A.E. and Smith, J.E. 2003. Introduction to Evolutionary Computation. Natural Computing Series, Springer-Verlag Berlin Heidelberg, pp. 55-56
      random.Reset();
      random.IntNumbers = new int[] { 3, 6 };
      parent1 = new Permutation(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
      Assert.IsTrue(parent1.Validate());
      parent2 = new Permutation(new int[] { 8, 2, 6, 7, 1, 5, 4, 0, 3 });
      Assert.IsTrue(parent2.Validate());
      expected = new Permutation(new int[] { 2, 7, 1, 3, 4, 5, 6, 0, 8 });
      Assert.IsTrue(expected.Validate());
      actual = OrderCrossover.Apply(random, parent1, parent2);
      Assert.IsTrue(actual.Validate());
      Assert.IsTrue(Auxiliary.PermutationIsEqualByPosition(expected, actual));
      // The following test is based on an example from Larranaga, P. et al. 1999. Genetic Algorithms for the Travelling Salesman Problem: A Review of Representations and Operators. Artificial Intelligence Review, 13, pp. 129-170.
      random.Reset();
      random.IntNumbers = new int[] { 2, 4 };
      parent1 = new Permutation(new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });
      Assert.IsTrue(parent1.Validate());
      parent2 = new Permutation(new int[] { 1, 3, 5, 7, 6, 4, 2, 0 });
      Assert.IsTrue(parent2.Validate());
      expected = new Permutation(new int[] { 7, 6, 2, 3, 4, 0, 1, 5 });
      actual = OrderCrossover.Apply(random, parent1, parent2);
      Assert.IsTrue(actual.Validate());
      Assert.IsTrue(Auxiliary.PermutationIsEqualByPosition(expected, actual));
      // The following test is based on an example from Talbi, E.G. 2009. Metaheuristics - From Design to Implementation. Wiley, p. 218.
      random.Reset();
      random.IntNumbers = new int[] { 2, 5 };
      parent1 = new Permutation(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
      Assert.IsTrue(parent1.Validate());
      parent2 = new Permutation(new int[] { 7, 3, 0, 4, 8, 2, 5, 1, 6 });
      Assert.IsTrue(parent2.Validate());
      expected = new Permutation(new int[] { 0, 8, 2, 3, 4, 5, 1, 6, 7 });
      Assert.IsTrue(expected.Validate());
      actual = OrderCrossover.Apply(random, parent1, parent2);
      Assert.IsTrue(actual.Validate());
      Assert.IsTrue(Auxiliary.PermutationIsEqualByPosition(expected, actual));
      // The following test is not based on published examples
      random.Reset();
      random.IntNumbers = new int[] { 0, 5 };
      parent1 = new Permutation(new int[] { 2, 1, 4, 3, 7, 8, 6, 0, 5, 9 });
      Assert.IsTrue(parent1.Validate());
      parent2 = new Permutation(new int[] { 5, 3, 4, 0, 9, 8, 2, 7, 1, 6 });
      Assert.IsTrue(parent2.Validate());
      expected = new Permutation(new int[] { 2, 1, 4, 3, 7, 8, 6, 5, 0, 9 });
      Assert.IsTrue(expected.Validate());
      actual = OrderCrossover.Apply(random, parent1, parent2);
      Assert.IsTrue(actual.Validate());
      Assert.IsTrue(Auxiliary.PermutationIsEqualByPosition(expected, actual));
      // based on the previous with changed breakpoints
      random.Reset();
      random.IntNumbers = new int[] { 6, 9 };
      expected = new Permutation(new int[] { 3, 4, 8, 2, 7, 1, 6, 0, 5, 9 });
      Assert.IsTrue(expected.Validate());
      actual = OrderCrossover.Apply(random, parent1, parent2);
      Assert.IsTrue(actual.Validate());
      Assert.IsTrue(Auxiliary.PermutationIsEqualByPosition(expected, actual));
      // another one based on the previous with changed breakpoints
      random.Reset();
      random.IntNumbers = new int[] { 0, 9 };
      expected = new Permutation(new int[] { 2, 1, 4, 3, 7, 8, 6, 0, 5, 9 });
      Assert.IsTrue(expected.Validate());
      actual = OrderCrossover.Apply(random, parent1, parent2);
      Assert.IsTrue(actual.Validate());
      Assert.IsTrue(Auxiliary.PermutationIsEqualByPosition(expected, actual));
      // perform a test when the two permutations are of unequal length
      random.Reset();
      bool exceptionFired = false;
      try {
        OrderCrossover.Apply(random, new Permutation(8), new Permutation(6));
      } catch (System.ArgumentException) {
        exceptionFired = true;
      }
      Assert.IsTrue(exceptionFired);
    }

    /// <summary>
    ///A test for OrderCrossover Constructor
    ///</summary>
    [TestMethod()]
    public void OrderCrossoverConstructorTest() {
      OrderCrossover target = new OrderCrossover();
    }
  }
}
