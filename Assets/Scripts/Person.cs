using System;
using SimpleJSON;

[Serializable]
public class Person
{
    public string Email, Linkedin, YoutubeLink, FirstName, LastName, CompanyName, JobFunction;


    public static Person GetFromJSON(JSONNode n)
    {
        Person p = new Person();
        p.FirstName = n["FirstName"];
        p.LastName = n["LastName"];
        p.Email = n["Email"];
        p.CompanyName = n["CompanyName"];
        p.JobFunction = n["JobFunction"];
        p.Linkedin = n["Linkedin"];
        p.YoutubeLink = n["YoutubeLink"];
        return p;
    }
}