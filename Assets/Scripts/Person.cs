using System;
using SimpleJSON;
using UnityEngine;

[Serializable]
public class Person
{
    public string Email, FirstName, LastName, CompanyName, JobFunction;
    public Sprite Picture;

    public static Person GetFromJSON(JSONNode n)
    {
        Person p = new Person();
        p.FirstName = n["FirstName"];
        p.LastName = n["LastName"];
        p.Email = n["Email"];
        p.CompanyName = n["CompanyName"];
        p.JobFunction = n["JobFunction"];
//        p.Picture = n["Picture"]; convert dit naar sprite vanaf iets...
        if (p.Picture == null)
        {
            p.Picture = Game.Get().StandardPerson;
        }
        return p;
    }
}