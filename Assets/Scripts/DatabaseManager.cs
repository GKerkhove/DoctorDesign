using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class DatabaseManager : MonoBehaviour
{

    public static DatabaseManager instance;

    public void Awake()
    {
        instance = this;
    }

    public static DatabaseManager Get()
    {
        return instance;
    }

    public void searchUser(string url, System.Action<List<Person>> callback)
    {
        WWW www = new WWW(url);
        List<Person> persons = new List<Person>();

        StartCoroutine(WaitForRequest(www, data =>
        {
            if (data.error == null)
            {
                JSONNode n = JSON.Parse(data.text);
                for (int i = 0; i < n.Count; i++)
                {
                    persons.Add(Person.GetFromJSON(n[i]));
                }
                callback(persons);
            }
        })); 
    }

    public void retrieveAll(System.Action<List<Person>> callback)
    {
        WWW www = new WWW("http://jimiverhoeven.nl:8080/users?user=DocterDesign");
        List<Person> persons = new List<Person>();

        StartCoroutine(WaitForRequest(www, data =>
        {
            if (data.error == null)
            {
                JSONNode n = JSON.Parse(data.text);
                for (int i = 0; i < n.Count; i++)
                {
                    persons.Add(Person.GetFromJSON(n[i]));
                }
                callback(persons);
            }
            else
            {
                print("empty callback");
                callback(null);
            }
        }));
    }

    public void retrieveByEmail(string email, System.Action<Person> callback)
    {
        WWW www = new WWW("http://jimiverhoeven.nl:8080/users/"+email+"?user=DocterDesign");

        StartCoroutine(WaitForRequest(www, data =>
        {
            if (data.error == null)
            {
                JSONNode n = JSON.Parse(data.text);
                callback(Person.GetFromJSON(n));
            }
            else
            {
                print("empty callback");
                callback(null);
            }
        }));
    }

    public void uploadImage(Texture2D snap)
    {
        WWWForm form = new WWWForm();
        form.AddField("image", System.Convert.ToBase64String(snap.EncodeToPNG()));
        StartCoroutine(UploadPNG(snap, form, data =>
        {
            if (data.error == null)
            {
                Debug.Log(data.text);
            }
            else
            {
                Debug.Log(data.error);
            }
        }));
           
    }
    IEnumerator UploadPNG(Texture2D snap, WWWForm form, System.Action<WWW> callback)
    {
        Debug.Log("Started the IE");

        WWW w = new WWW("http://jimiverhoeven.nl:8080/uploadImage?user=DocterDesign", form);
        yield return w;
        callback(w);
    }

    IEnumerator WaitForRequest(WWW www, System.Action<WWW> callback)
    {
        yield return www;

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: ");
            yield return null;
            callback(www);
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
            yield return null;
            callback(www);
        }
    }
}
