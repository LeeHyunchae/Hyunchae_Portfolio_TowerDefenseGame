using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.IO;

public static class TableLoader
{
    private static readonly string APP_PATH = Application.dataPath;    
    private static readonly string JSON_DATA_PATH = "JsonData";
    private static readonly string EXTENSION = ".json";

    public static void SaveToJson<T>(string _directory,T _data,string _fileName)
    {
        string json = JsonConvert.SerializeObject(_data,Formatting.Indented);
        Debug.Log(json);

        string path = Path.Combine(APP_PATH, JSON_DATA_PATH ,_directory,_fileName+EXTENSION);

        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate,FileAccess.Write);
        StreamWriter writer = new StreamWriter(fileStream);

        writer.Write(string.Empty);
        writer.Write(json);
        
        writer.Close();
        fileStream.Close();
    }

    public static T LoadFromFile<T>(string _directory)
    {
        string path = Path.Combine(APP_PATH, JSON_DATA_PATH, _directory+EXTENSION);

        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(fileStream);

        string json = reader.ReadToEnd();

        reader.Close();
        fileStream.Close();

        return JsonConvert.DeserializeObject<T>(json);
    }
}
