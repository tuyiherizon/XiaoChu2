
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text;

public class CommonTool : Editor
{

    [MenuItem("TyTools/ClearPref")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("TyTools/CopyTables")]
    public static void CopyTables()
    {
        string pathOrg = Path.Combine(Application.dataPath, "XiaoChu\\Script\\Common\\Script\\Tables\\Content");
        string pathRes = Path.Combine(Application.dataPath, "XiaoChu\\Resources\\Tables");

        var copyFiles = Directory.GetFiles(pathOrg);
        foreach (var sourceFile in copyFiles)
        {
            FileStream fs = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader streamReader = new StreamReader(fs, Encoding.Default);

            string descPath = Path.Combine(pathRes, Path.GetFileName(sourceFile));
            StreamWriter streamWrite = new StreamWriter(File.Create(descPath), Encoding.UTF8);

            var text = streamReader.ReadToEnd();
            streamWrite.Write(text);

            streamReader.Close();
            streamWrite.Close();
        }
            
    }
}
