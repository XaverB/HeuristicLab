using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeuristicLab.Core;

namespace HeuristicLab.RemoteControl.TestPlugin.Util {
  internal static class IndexParser {

    /// <summary>
    /// Try to parse a string with a index postfix e.g. [x,y] or [x]. The index can be an array index or an matrix index.
    /// </summary>
    /// <param name="input">The string to be parsed</param>
    /// <param name="inputWithoutIndex">The input string without the index postfix.</param>
    /// <param name="index">An array containing the index(s). If length == 1 it is an array index. If length == 2 it is an matrix index</param>
    /// <returns>True if the input string contains an index.</returns>
    public static bool TryParseIndex(string input, out string inputWithoutIndex, out int[] index) {
      index = null;
      input = input.Trim();
      bool isStringWithIndexer = input.Contains("[") && input.Contains("]");

      var substrings = input.Split('[');
      if (substrings.Length > 2)
        throw new ArgumentException("Could not parse indexer string. More than one '[' is not allowed.");

      inputWithoutIndex = substrings[0];

      if (!isStringWithIndexer) {
        return false;
      }

      string indexerString = substrings[1].Remove(substrings[1].Length - 1, 1);
      bool isMatrixIndexer = indexerString.Contains(",");

      index = isMatrixIndexer ? ParseMatrixIndex(indexerString) : ParseArrayIndex(indexerString);

      return true;
    }

    private static int[] ParseArrayIndex(string indexerString) {
      int[] index = new int[1];
      if (!int.TryParse(indexerString, out index[0])) {
        throw new Exception($"Could not parse second index. We assume it is a matrix indexer since we found ','.");
      }

      return index;
    }

    private static int[] ParseMatrixIndex(string indexerString) {
      int[] index = new int[2];

      var substrings = indexerString.Split(',');
      if (substrings.Length != 2)
        throw new ArgumentException($"Could not parse matrix indexer. It must contain ',' as index separator.");

      if (!int.TryParse(substrings[0], out index[0])) {
        throw new ArgumentException($"Could not parse first index. We assume it is a matrix indexer since we found ','.");
      }

      if (!int.TryParse(substrings[1], out index[1])) {
        throw new Exception($"Could not parse second index. We assume it is a matrix indexer since we found ','.");
      }

      return index;
    }
  }
}
