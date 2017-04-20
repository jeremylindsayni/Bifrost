using System.Collections.Generic;

namespace Bifrost.DictionaryComparer
{
    public interface IComparer
    {
        ISet<string> GetFileDifferences(IDictionary<string, string> oldDictionary, IDictionary<string, string> newDictionary);

        IDictionary<string, string> GetFilesRecursively(string directoryPath);
    }
}
