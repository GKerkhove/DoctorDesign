using System;
using System.IO;
using UnityEngine;
using SimpleJSON;


class CurrentUser
{

    public static string Path = Application.persistentDataPath + "/User.json";

    public static void AddPerson(Person p)
    {
        string json = JsonUtility.ToJson(p);
        JSONNode person = JSON.Parse(json);
        person.SaveToCompressedFile(Path);
    }

    public static void SaveBobState(Person p)
    {
        p.Bobstate = Game.Get().BobState;
        string json = JsonUtility.ToJson(p);
        JSONNode person = JSON.Parse(json);
        person.SaveToCompressedFile(Path);
    }

    public static Person GetPerson()
    {
        Debug.Log(Path);
        try
        {
            JSONNode p = JSONNode.LoadFromCompressedFile(Path);
            Debug.Log(p["FirstName"] + " " + p["LastName"]);
            if (p["Bobstate"] != null)
            {
                Game.Get().BobState = p["Bobstate"].AsInt;
            }
            else
            {
                Game.Get().BobState = 0;
            }
            return Person.GetFromJSON(p);
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public static bool HasPerson()
    {
        return GetPerson() != null;
    }

}