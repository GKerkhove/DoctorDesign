WEB API KIT (WAK)
by Haptix Games Inc

The Web API Kit gives you the freedom to interconnect your application with the world wide web primarly using the HTTP protocol.
Lots of companies publish their API and make it available to HTTP clients.  With WAK you can take advantage of those public endpoints
and create your own client using Unity's WWW class or other HTTP enabling assets.  Then distribute your client with your application
and have the world's data at your fingertips.

Many times beginners struggle with the concept of storing and retrieving custom data.  Often they turn to solutions that store the 
application information on the device using flat files or SQLlite.  The video tutorials that accompany this asset demonstrate how to 
build your own API server using eXist-db and XML/JSON technologies.

Once developers become familiar with web technologies and Unity's WWW class, they begin experimenting with retrieving and posting
text data.  This results in something like this:

WWW httpOperation = new WWW("http://some.domain.com/textFile.txt");
yield return httpOperation;
string retrievedString = httpOperation.text;

That looks simple enough, but we are missing a couple features:
* error checking
* ability to timeout the operation
* ability to cancel the operation on user request
* simplified HTTP request construction, including HTTP headers, HTML Form fields, etc
* outbound/inbound data conversion and mapping

Don't worry, WAK comes to the rescue.  The world of WAK revolves around HTTP operations that you define.  An emphasis is placed
on using .NET attributes on classes and fields.  If you are not familiar with attributes then this might be a good time to look 
them up as well as attribute-oriented programming.

Let's try to rewrite the above flat file request in WAK.  Create a class and inherit from 'hg.ApiWebKit.core.http.HttpOperation'.
Let's add a few attributes to it from the 'hg.ApiWebKit.core.attributes' namespace.  We are retrieving a file which means it is
a HTTP GET operation.  And we need to define the URL, so we will use the 'HttpPath' attribute.

Inside the class we will want to store the retrieved text data and do something with it.  So let's define a public field that we
can access later.  To make is super easy, we will add an attribute to the field from the 'hg.ApiWebKit.core.attributes' namespace.
The 'HttpResponseTextBody' attribute will let WAK know to take the response text value and assign it to the marked up field.

Ok so here is our first HTTP operation:

[hg.ApiWebKit.core.attributes.HttpGET]
[hg.ApiWebKit.core.attributes.HttpPath(null,"http://some.domain.com/textFile.txt")]
public class MyFirstWakOperation: hg.ApiWebKit.core.http.HttpOperation
{
	[hg.ApiWebKit.core.attributes.HttpResponseTextBody]
	public string RetrievedString;
}

So let's try to use this operation from one of our application's scripts.
Inside the Start method we will create and instance of the operation and call Send.
The Send method accepts 3 callback functions, similar to JavaScript and jQuery in web programming.
If the execution is successful then OnSuccess will be called, otherwise OnFailure will be called.
OnComplete fires regardless of success or failure.
The callback method signature has 2 parameters.  The first one is of the HTTP operation type you are calling and
the second one is 'core.http.HttpResponse' which gives you access to HTTP protocol information.

public class MyAppScript: MonoBehaviour
{
	void Start()
	{
		new MyFirstWakOperation.Send(OnSuccess,OnFailure,OnComplete);
	}
	
	private void OnSuccess(MyFirstWakOperation operation, core.http.HttpResponse response)
	{
		Debug.Log("Success!");
		Debug.Log(operation.RetrievedString);
	}

	private void OnFailure(MyFirstWakOperation operation, core.http.HttpResponse response)
	{
		Debug.Log("Failed");

		Debug.Log("Faulted because: " + string.Join(" ; ", operation.FaultReasons.ToArray()));
	}

	private void OnComplete(MyFirstWakOperation operation, core.http.HttpResponse response)
	{
		Debug.Log("Completed with status code : " + response.StatusCode);
	}
}

You have just created your first HTTP operation.
Don't forget to watch our tutorial series for more advanced topics.

For a practical example please take a look here: http://www.reddit.com/r/Unity3D/comments/2at4pa/www_and_rest_apis/

-Haptix Games Inc




