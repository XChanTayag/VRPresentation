using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine;
//using UnityEditor;

/*
    TODO: Class should be independent and decoupled
    Should just read and write, data shall be
    handled by a separate 'specialized' class
*/

public static class JSONParser
{
    // TODO: throw error if given filename does not exist in Resources folder
    public static JToken ReadJson(string filename)
    {
        TextAsset file = Resources.Load(filename) as TextAsset;                 // Loads file as TextAsset from filename
        string content = (file) ? file.ToString() : "[]";                       // Check if file is null and assign appropriate value
        var token = JToken.Parse(content);                                      // Parse string content to JToken
        return token;
    }

    public static void WriteJson(string filename, JToken content)
    {
        string path = "Assets/Resources/" + filename + ".json";                 // File to write JSON to
        File.WriteAllText(@path, content.ToString());                           // Does the actual writing

        // Re-import the file to update the reference in the editor
        //AssetDatabase.ImportAsset(path);
    }
}
