using UnityEngine;
using UnityEditor;
using System.IO;

public class TextIO {
    public static void WriteString(string path, string contents) {
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, false);
        writer.WriteLine(contents);
        writer.Close();
        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
    }
    public static void ReadString(string path) {
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        //Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
}