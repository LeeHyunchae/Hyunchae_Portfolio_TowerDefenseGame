using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class FileIO : MonoBehaviour
{
    //using (FileStream fs = new FileStream("txtTest.txt", FileMode.Createm, FileAccess.ReadWrite)
    //{

    //}

    string directory = "Assets/TestFiles/";
    

    private void Awake()
    {
        DirectoryInfo di = new DirectoryInfo(directory + "TestDirectoryInfo");
        di.Create();

        
        Person p = new Person
        {
            name = "lee",
            age = 3,
            height = 5
        };

        string jsonstr = JsonUtility.ToJson(p);

        File.WriteAllText(directory + "jsonTest.json", jsonstr);

        string jsonread = File.ReadAllText(directory + "jsonTest.json");

        Person p2 = JsonUtility.FromJson<Person>(jsonread);

        Debug.Log(p2.name);
        Debug.Log(p2.age);
        Debug.Log(p2.height);


        Item[] itemarray = new Item[2];

        Item item = new Item
        {
            ItemName = "Sword",
            value = 2
        };

        itemarray[0] = item;
        item = new Item
        {
            ItemName = "Bow",
            value = 3
        };

        itemarray[1] = item;

        string jc = JsonConvert.SerializeObject(itemarray);

        File.WriteAllText(directory + "ItemData.json", jc);
    }

}


class Person
{
    public string name;
    public int age;
    public float height;
}

class Item
{
    public string ItemName;
    public int value;
}


/*
        //#region txtIO
        //FileStream fs = new FileStream(directory + "FilestreamtxtTest.txt", FileMode.Create, FileAccess.ReadWrite);
        //FileStream fs2 = File.Create(directory + "FilestreamtxtTest2.txt");

        //fs.Close();
        //fs2.Close();


        //using (StreamWriter sw = new StreamWriter(new FileStream(directory + "StreamWritertestTxt.txt", FileMode.Create, FileAccess.ReadWrite)))
        //{
        //    sw.Write("FirstWrite.");
        //    sw.WriteLine("FirstWriteLine.");
        //    sw.Write("SecondWrite.");
        //}


        //FileInfo finfo = new FileInfo(directory + "FileInfotxt.txt");

        //using(StreamWriter sw = finfo.CreateText())
        //{
        //    sw.WriteLine("파일인포클래스");
        //    sw.WriteLine("쓰기테스트 \n 123123");
        //}

        //if(finfo.Exists)
        //{
        //    using (StreamReader sreader1 = finfo.OpenText())
        //    {
        //        string line;
        //        while((line = sreader1.ReadLine()) != null)
        //        {
        //            Debug.Log(line);
        //        }

        //    }
        //}
        //#endregion

        //#region CSVIO

        //using (StreamWriter swriter2 = new StreamWriter(directory + "csvTest.csv"))
        //{
        //    swriter2.WriteLine("A,B,C");

        //    swriter2.WriteLine("A1,B1,C1");
        //    swriter2.WriteLine("A2,B2,C2");
        //}

        //using (StreamReader sreader2 = new StreamReader(directory + "csvTest.csv"))
        //{
        //    string line;
        //    while ((line = sreader2.ReadLine()) != null)
        //    {
        //        Debug.Log(line);
        //    }
        //}



        //#endregion

        //#region binIO

        //using (BinaryWriter bwriter = new BinaryWriter(File.Open(directory + "binTest.bin", FileMode.OpenOrCreate)))
        //{
        //    bwriter.Write(25);
        //    bwriter.Write(3.141592);
        //    bwriter.Write("abc");
        //}

        //using (BinaryReader breader = new BinaryReader(File.Open(directory + "binTest.bin", FileMode.Open)))
        //{
        //    int intdata = breader.ReadInt32();
        //    double doubledata = breader.ReadDouble();
        //    string stringdata = breader.ReadString();

        //    Debug.Log(intdata);
        //    Debug.Log(doubledata);
        //    Debug.Log(stringdata);
        //}
        //#endregion
        */