using System;
using UnityEngine;
using SimpleJSON;


class CurrentUser
{

    public static string Path = Application.persistentDataPath + "/User.json";

    public static void AddPerson(Person p)
    {
        string json = JsonUtility.ToJson(p);
        JSONNode person = JSON.Parse(json);
        Debug.Log(person["FirstName"]);
        person.SaveToCompressedFile(Path);
    }

    public static Person GetPerson()
    {
        try
        {
            JSONNode p = JSONNode.LoadFromCompressedFile(Path);
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