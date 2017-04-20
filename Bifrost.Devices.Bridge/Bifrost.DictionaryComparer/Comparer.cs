using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Bifrost.DictionaryComparer
{ 
    public class Comparer : IComparer
    {
        public ISet<string> GetFileDifferences(IDictionary<string, string> oldDictionary, IDictionary<string, string> newDictionary)
        {
            var changes = new HashSet<string>();

            foreach (var key in oldDictionary.Keys)
            {
                var oldDictionaryValue = oldDictionary[key];
                var newDictionaryValue = newDictionary[key];

                if (oldDictionaryValue != newDictionaryValue)
                {
                    var fileMetaData = new KeyValuePair<string, string>(GetFileName(key), newDictionaryValue);
                    changes.Add(key);
                }
            }

            foreach (var key in newDictionary.Keys)
            {
                string oldDictionaryValue;

                oldDictionary.TryGetValue(key, out oldDictionaryValue);

                if (oldDictionaryValue == null)
                {
                    var fileMetaData = new KeyValuePair<string, string>(GetFileName(key), newDictionary[key]);
                    changes.Add(key);
                }
            }

            return changes;
        }

        private static string GetFileName(string path)
        {
            var file = new FileInfo(path);

            return file.Name;
        }

        public IDictionary<string, string> GetFilesRecursively(string directoryPath)
        {
            var fileProperties = new Dictionary<string, string>();

            try
            {
                foreach (string d in Directory.GetDirectories(directoryPath))
                {
                    foreach (string f in Directory.GetFiles(d))
                    {
                        string fileContents = File.ReadAllText(f);

                        fileProperties.Add(f, fileContents);
                    }

                    GetFilesRecursively(d);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return fileProperties;
        }
    }
}
