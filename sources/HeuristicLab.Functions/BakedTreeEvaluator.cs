﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeuristicLab.DataAnalysis;

namespace HeuristicLab.Functions {
  internal class BakedTreeEvaluator {
    private const int ADDITION = 10010;
    private const int AND = 10020;
    private const int AVERAGE = 10030;
    private const int CONSTANT = 10040;
    private const int COSINUS = 10050;
    private const int DIVISION = 10060;
    private const int EQU = 10070;
    private const int EXP = 10080;
    private const int GT = 10090;
    private const int IFTE = 10100;
    private const int LT = 10110;
    private const int LOG = 10120;
    private const int MULTIPLICATION = 10130;
    private const int NOT = 10140;
    private const int OR = 10150;
    private const int POWER = 10160;
    private const int SIGNUM = 10170;
    private const int SINUS = 10180;
    private const int SQRT = 10190;
    private const int SUBSTRACTION = 10200;
    private const int TANGENS = 10210;
    private const int VARIABLE = 10220;
    private const int XOR = 10230;

    private static int nextFunctionSymbol = 10240;
    private static Dictionary<int, IFunction> symbolTable;
    private static Dictionary<IFunction, int> reverseSymbolTable;
    private static Dictionary<Type, int> staticTypes;
    private static int MAX_CODE_LENGTH = 4096;
    private static int MAX_DATA_LENGTH = 4096;
    private static int[] codeArr = new int[MAX_CODE_LENGTH];
    private static double[] dataArr = new double[MAX_DATA_LENGTH];

    static BakedTreeEvaluator() {
      symbolTable = new Dictionary<int, IFunction>();
      reverseSymbolTable = new Dictionary<IFunction, int>();
      staticTypes = new Dictionary<Type, int>();
      staticTypes[typeof(Addition)] = ADDITION;
      staticTypes[typeof(And)] = AND;
      staticTypes[typeof(Average)] = AVERAGE;
      staticTypes[typeof(Constant)] = CONSTANT;
      staticTypes[typeof(Cosinus)] = COSINUS;
      staticTypes[typeof(Division)] = DIVISION;
      staticTypes[typeof(Equal)] = EQU;
      staticTypes[typeof(Exponential)] = EXP;
      staticTypes[typeof(GreaterThan)] = GT;
      staticTypes[typeof(IfThenElse)] = IFTE;
      staticTypes[typeof(LessThan)] = LT;
      staticTypes[typeof(Logarithm)] = LOG;
      staticTypes[typeof(Multiplication)] = MULTIPLICATION;
      staticTypes[typeof(Not)] = NOT;
      staticTypes[typeof(Or)] = OR;
      staticTypes[typeof(Power)] = POWER;
      staticTypes[typeof(Signum)] = SIGNUM;
      staticTypes[typeof(Sinus)] = SINUS;
      staticTypes[typeof(Sqrt)] = SQRT;
      staticTypes[typeof(Substraction)] = SUBSTRACTION;
      staticTypes[typeof(Tangens)] = TANGENS;
      staticTypes[typeof(Variable)] = VARIABLE;
      staticTypes[typeof(Xor)] = XOR;
    }

    internal BakedTreeEvaluator(List<int> code, List<double> data) {
      code.CopyTo(codeArr);
      data.CopyTo(dataArr);
    }

    internal static int MapFunction(IFunction function) {
      if(!reverseSymbolTable.ContainsKey(function)) {
        int curFunctionSymbol;
        if(staticTypes.ContainsKey(function.GetType())) curFunctionSymbol = staticTypes[function.GetType()];
        else {
          curFunctionSymbol = nextFunctionSymbol;
          nextFunctionSymbol++;
        }
        reverseSymbolTable[function] = curFunctionSymbol;
        symbolTable[curFunctionSymbol] = function;
      }
      return reverseSymbolTable[function];
    }

    internal static IFunction MapSymbol(int symbol) {
      return symbolTable[symbol];
    }


    private int PC;
    private int DP;
    private Dataset dataset;
    private int sampleIndex;

    internal double Evaluate(Dataset _dataset, int _sampleIndex) {
      PC = 0;
      DP = 0;
      sampleIndex = _sampleIndex;
      dataset = _dataset;
      return EvaluateBakedCode();
    }

    private double EvaluateBakedCode() {
      int arity = codeArr[PC];
      int functionSymbol = codeArr[PC+1];
      int nLocalVariables = codeArr[PC+2];
      PC += 3;
      switch(functionSymbol) {
        case VARIABLE: {
            int var = (int)dataArr[DP];
            double weight = dataArr[DP+1];
            int row = sampleIndex + (int)dataArr[DP+2];
            DP += 3;
            if(row < 0 || row >= dataset.Rows) return double.NaN;
            else return weight * dataset.GetValue(row, var);
          }
        case CONSTANT: {
            return dataArr[DP++];
          }
        case MULTIPLICATION: {
            double result = EvaluateBakedCode();
            for(int i = 1; i < arity; i++) {
              result *= EvaluateBakedCode();
            }
            return result;
          }
        case ADDITION: {
            double sum = EvaluateBakedCode();
            for(int i = 1; i < arity; i++) {
              sum += EvaluateBakedCode();
            }
            return sum;
          }
        case SUBSTRACTION: {
            if(arity == 1) {
              return -EvaluateBakedCode();
            } else {
              double result = EvaluateBakedCode();
              for(int i = 1; i < arity; i++) {
                result -= EvaluateBakedCode();
              }
              return result;
            }
          }
        case DIVISION: {
            double result;
            if(arity == 1) {
              result = 1.0 / EvaluateBakedCode();
            } else {
              result = EvaluateBakedCode();
              for(int i = 1; i < arity; i++) {
                result /= EvaluateBakedCode();
              }
            }
            if(double.IsInfinity(result)) return 0.0;
            else return result;
          }
        case AVERAGE: {
            double sum = EvaluateBakedCode();
            for(int i = 1; i < arity; i++) {
              sum += EvaluateBakedCode();
            }
            return sum / arity;
          }
        case COSINUS: {
            return Math.Cos(EvaluateBakedCode());
          }
        case SINUS: {
            return Math.Sin(EvaluateBakedCode());
          }
        case EXP: {
            return Math.Exp(EvaluateBakedCode());
          }
        case LOG: {
            return Math.Log(EvaluateBakedCode());
          }
        case POWER: {
            double x = EvaluateBakedCode();
            double p = EvaluateBakedCode();
            return Math.Pow(x, p);
          }
        case SIGNUM: {
            double value = EvaluateBakedCode();
            if(double.IsNaN(value)) return double.NaN;
            else return Math.Sign(value);
          }
        case SQRT: {
            return Math.Sqrt(EvaluateBakedCode());
          }
        case TANGENS: {
            return Math.Tan(EvaluateBakedCode());
          }
        case AND: {
            double result = 1.0;
            // have to evaluate all sub-trees, skipping would probably not lead to a big gain because 
            // we have to iterate over the linear structure anyway
            for(int i = 0; i < arity; i++) {
              double x = Math.Round(EvaluateBakedCode());
              if(x == 0 || x==1.0) result *= x;
              else result = double.NaN;
            }
            return result;
          }
        case EQU: {
            double x = EvaluateBakedCode();
            double y = EvaluateBakedCode();
            if(x == y) return 1.0; else return 0.0;
          }
        case GT: {
            double x = EvaluateBakedCode();
            double y = EvaluateBakedCode();
            if(x > y) return 1.0;
            else return 0.0;
          }
        case IFTE: {
            double condition = Math.Round(EvaluateBakedCode());
            double x = EvaluateBakedCode();
            double y = EvaluateBakedCode();
            if(condition < .5) return x;
            else if(condition >= .5) return y;
            else return double.NaN;
          }
        case LT: {
            double x = EvaluateBakedCode();
            double y = EvaluateBakedCode();
            if(x < y) return 1.0;
            else return 0.0;
          }
        case NOT: {
            double result = Math.Round(EvaluateBakedCode());
            if(result == 0.0) return 1.0;
            else if(result == 1.0) return 0.0;
            else return double.NaN;
          }
        case OR: {
            double result = 0.0; // default is false
            for(int i = 0; i < arity; i++) {
              double x = Math.Round(EvaluateBakedCode());
              if(x == 1.0 && result == 0.0) result = 1.0; // found first true (1.0) => set to true
              else if(x != 0.0) result = double.NaN; // if it was not true it can only be false (0.0) all other cases are undefined => (NaN)
            }
            return result;
          }
        case XOR: {
            double x = Math.Round(EvaluateBakedCode());
            double y = Math.Round(EvaluateBakedCode());
            if(x == 0.0 && y == 0.0) return 0.0;
            if(x == 1.0 && y == 0.0) return 1.0;
            if(x == 0.0 && y == 1.0) return 1.0;
            if(x == 1.0 && y == 1.0) return 0.0;
            return double.NaN;
          }
        default: {
            IFunction function = symbolTable[functionSymbol];
            double[] args = new double[nLocalVariables + arity];
            for(int i = 0; i < nLocalVariables; i++) {
              args[i] = dataArr[DP++];
            }
            for(int j = 0; j < arity; j++) {
              args[nLocalVariables + j] = EvaluateBakedCode();
            }
            return function.Apply(dataset, sampleIndex, args);
          }
      }
    }
  }
}
