using System;
using UnityEngine;
using SimpleJSON;


class CurrentUser
{

    private string m_InGameLog = "";
    public static string Path = Application.persistentDataPath + "/User.json";

    public static void AddPerson(Person p)
    {
        string json = JsonUtility.ToJson(p);
        JSONNode person = JSON.Parse(json);
        Debug.Log(person["FirstName"]);
//        JSONNode high = Game.Get().Highscores;
//        high["Game"]["Version"] = Game.Get().Version;
//        high["Game"]["" + level]["Score"].Add("" + score);
//        if (high["Game"]["" + level]["Highscore"] == null || high["Game"]["" + level]["Highscore"].AsInt < score)
//        {
//            high["Game"]["" + level]["Highscore"].AsInt = score;
//        }
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

//    public static int GetLevelHighscore(int level)
//    {
//        JSONNode high;
//        if (Game.Get() == null)
//        {
//            high = GetFileInfo();
//        }
//        else
//        {
//            high = Game.Get().Highscores;
//
//        }
//        return high["Game"]["" + level]["Highscore"].AsInt;
//    }

    //public static void Test()
    //{
    //    JSONNode N = new JSONClass();
    //    N["Test"]["TestV"] = "willem";
    //    N["Test2"] = "Bla";
    //    N["Test"]["TestD"]["TestB"] = "4";
    //    Debug.Log(N);
    //    N.SaveToFile(Application.persistentDataPath + "/Saves/Test.json");
    //    N = JSONNode.LoadFromFile(Application.persistentDataPath + "/Saves/Test.json");
    //    N["Test123"] = "waarpo";
    //    Debug.Log(N);
    //}

//    public static void PlayerName(string name)
//    {
//        JSONNode high;
//        if (Game.Get() == null)
//        {
//            high = GetFileInfo();
//        }
//        else
//        {
//            high = Game.Get().Highscores;
//
//        }
//        high["Name"] = name;
//        high.SaveToFile(Path);
//    }
//
//    public static JSONNode GetFileInfo()
//    {
//        if (System.IO.File.Exists(Path))
//        {
//            return JSONNode.LoadFromFile(Path);
//        }
//        else
//        {
//            return new JSONClass();
//        }
//    }

//    public static string GetPlayerName()
//    {
//        JSONNode high;
//        if (Game.Get() == null)
//        {
//            high = GetFileInfo();
//        }
//        else
//        {
//            high = Game.Get().Highscores;
//
//        }
//        return high["Name"];
//    }
//
//    public static int GetHighScore(int level)
//    {
//        JSONNode high;
//        if (Game.Get() == null)
//        {
//            high = GetFileInfo();
//        }
//        else
//        {
//            high = Game.Get().Highscores;
//
//        }
//        return high["Game"][level]["Highscore"].AsInt;
//    }

    //public void AddHighscore(int score, string name){
    //    JSONNode high = Game.Get().Highscores;
    //    high["Game"]["Version"] = "0.0.1";
    //    high["Game"]["Highscores"]["Name"].Add(name);
    //    high["Game"]["Highscores"]["Score"].Add("" + score);
    //    if (high["Game"]["HighScore"]["Score"] == null || high["Game"]["HighScore"]["Score"].AsInt < score)
    //    {
    //        high["Game"]["HighScore"]["Score"].AsInt = score;
    //        high["Game"]["HighScore"]["Name"] = name;
    //    }
    //    high.SaveToFile(Path);
    //}
}