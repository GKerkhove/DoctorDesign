﻿using System;
using SimpleJSON;
using UnityEngine;

[Serializable]
public class Person
{
    public string Email, FirstName, LastName, CompanyName, JobFunction;
    public Sprite Picture;
    public int Bobstate; //This is only for the current user

    public static Person GetFromJSON(JSONNode n)
    {
        Person p = new Person();
        p.FirstName = n["FirstName"];
        p.LastName = n["LastName"];
        p.Email = n["Email"];
        p.CompanyName = n["CompanyName"];
        p.JobFunction = n["JobFunction"];
//        p.Picture = n["Picture"]; convert dit naar sprite vanaf iets...
//        DatabaseManager.Get().RetrieveImage(p.Email, data =>
//        {
//            Debug.Log("Ik heb image gevonden");
//            p.Picture = Sprite.Create(data,new Rect(0,0,data.width,data.height),new Vector2(1,1) );
//        });
        if (p.Picture == null)
        {
            p.Picture = Game.Get().StandardPerson;
        }
        return p;
    }
}