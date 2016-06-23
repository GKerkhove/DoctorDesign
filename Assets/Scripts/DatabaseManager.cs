using System;
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

    public void SearchUser(string url, System.Action<List<Person>> callback)
    {
        WWW www = new WWW(url);
        List<Person> persons = new List<Person>();

        StartCoroutine(WaitForRequest(www, data =>
        {
            if (data.error == null)
            {
                //string[] text = data.text.Split(new [] {",ProfileImage"}, StringSplitOptions.None);
                JSONNode n = JSON.Parse(data.text);
                print(data.text);
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

    public void retrieveConnections(string email,System.Action<List<string>> callback)
    {
        print(email);
        WWW www = new WWW("http://jimiverhoeven.nl:8080/connectionsByYou/"+email+"?user=DocterDesign");
        List<string> emails = new List<string>();

        StartCoroutine(WaitForRequest(www, data =>
        {
            if (data.error == null)
            {
                JSONNode n = JSON.Parse(data.text);
                for (int i = 0; i < n.Count; i++)
                {
                    emails.Add(n[i]["To_email"]);
                }
                callback(emails);
            }
            else
            {
                print(data.error);
                callback(null);
            }
        }));
    }

    public void retrieveByEmail(string email, System.Action<Person> callback)
    {
        WWW www = new WWW("http://jimiverhoeven.nl:8080/users/" + email + "?user=DocterDesign");

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

    public void RetrieveConnectedPersons(System.Action<List<Person>> callback)
    {
        List<Person> persons = new List<Person>();
        print("IN RETRIEVE");
        retrieveConnections(Game.Get().User.Email, data =>
        {
            print(data);
            if (data != null)
            {
                int l = data.Count;
                int i = 0;
                foreach (String s in data)
                {
                    retrieveByEmail(s, data2 =>
                    {
                        print(i+" == " + l);

                        i++;
                        persons.Add(data2);
                        if (i == l)
                        {
                            print("CALLING BACK" + persons.Count);
                            callback(persons);
                        }
                        
                    });
                }
            }
            else
            {
                callback(persons);
            }
        });
    }
//     insert into users (FirstName,LastName,CompanyName,Email,JobFunction,email_openbaar,telefoonnummer_openbaar) values("Guido", "Kerkhove","Kerkhove IT","guido.kerkhove@student.hu.nl","CEO",0,0);
//     insert into users (FirstName,LastName,CompanyName,Email,JobFunction,email_openbaar,telefoonnummer_openbaar) values("Lars", "Botma","Whiskey for Breakfast","lars.botma@student.hu.nl","Gitarist",0,0);
//     insert into users (FirstName,LastName,CompanyName,Email,JobFunction,email_openbaar,telefoonnummer_openbaar) values("Theo", "Florea","MaijLogic","theo.florea@student.hu.nl","Developer",0,0);
//     insert into users (FirstName,LastName,CompanyName,Email,JobFunction,email_openbaar,telefoonnummer_openbaar) values("Peter", "Noordijk","Lame","peter.noordijk@student.hu.nl","Zoeken",0,0);
//     insert into users (FirstName,LastName,CompanyName,Email,JobFunction,email_openbaar,telefoonnummer_openbaar) values("Mark", "van der Horst","DEVENTit","mark.vanderhorst@student.hu.nl","Support 2e lijns",0,0);
//     insert into users (FirstName,LastName,CompanyName,Email,JobFunction,email_openbaar,telefoonnummer_openbaar) values("Jimi", "Verhoeven","Een bedrijf","jimi.verhoeven@student.hu.nl","Werekuh",0,0);
//     insert into users (FirstName,LastName,CompanyName,Email,JobFunction,email_openbaar,telefoonnummer_openbaar) values("Nick", "Rutgers","Zebs","nick.rutgers@student.hu.nl","Keuken Hulp",0,0);
//     insert into users (FirstName,LastName,CompanyName,Email,JobFunction,email_openbaar,telefoonnummer_openbaar) values("Thea", "Floreo","Developer","Floreo.theo@hu.student.nl","MaijLogic",0,0);
//     insert into users (FirstName,LastName,CompanyName,Email,JobFunction,email_openbaar,telefoonnummer_openbaar) values("Gerald", "Ovink","Kaer Morhen","gerald.ovink@hu.nl","Witcher",0,0);
//     insert into users (FirstName,LastName,CompanyName,Email,JobFunction,email_openbaar,telefoonnummer_openbaar) values("Sir Geralt", "of Rivia","Hogeschool Utrecht","Thewitcher@CDprojectRed.red","Leraar",0,0);


    public void RetrieveImage(string email, System.Action<Texture2D> callback)
    {
        print("WOLLA WILLEM");
        WWW www = new WWW("http://jimiverhoeven.nl:8080/getImage/" + email + "?user=DocterDesign");
        Texture2D txt = new Texture2D(1,1);
        StartCoroutine(WaitForRequest(www, data =>
        {
print(data.texture );
            if (data != null && data.error == null)
            {

                data.LoadImageIntoTexture(txt);
                
                callback(txt);
            }
            else
            {
                print(data.error);
                callback(null);
            }
        }));
    }

	public void uploadImage(Texture2D snap, string email)
    {
        WWWForm form = new WWWForm();
        form.AddBinaryData("avatar", snap.EncodeToJPG());
		form.AddField ("email", email);
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

        WWW w = new WWW("http://jimiverhoeven.nl:8080/uploadImage?user=DocterDesign", form);
        yield return w;
        callback(w);
    }

    public void CreateConnection(string email1, string email2, string notes)
    {
        WWWForm form = new WWWForm();
        form.AddField("from", email1);
        form.AddField("to", email2);
        form.AddField("time", DateTime.Now.ToString("hh:mm:ss"));
        form.AddField("notities", notes);
        StartCoroutine(MakeConnection(form, data =>
        {
            if (data.error == null)
            {
                Debug.Log("Blablabla1123");
                Debug.Log(data.text);
            }
            else
            {
                Debug.Log("WOLLLLLLLAAAA&%&%&%&");
                Debug.Log(data.error);
            }
        }));
    }
    IEnumerator MakeConnection(WWWForm form, System.Action<WWW> callback)
    {
        Debug.Log("Started the IE");

        WWW w = new WWW("http://jimiverhoeven.nl:8080/addConnection?user=DocterDesign", form);
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
            Debug.Log(www.texture);
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
