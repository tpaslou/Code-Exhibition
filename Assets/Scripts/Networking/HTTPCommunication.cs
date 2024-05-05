using System;
using System.IO;
using System.Net;


namespace HTTPMocking
{
	/// <summary>
	/// This class is responsible for HTTP communication,
    /// with no business logic inside, using .Net instead
    /// of unity web requests to avoid Monobehaviours and
    /// re use it in different environments if needed.
	/// </summary>
    /// 
	public class HTTPCommunication
	{

        public  Action<string> OnHTTPSuccess;
        public  Action<string> OnHTTPError;

        string uri;
        

        public HTTPCommunication(string uri)
		{
			this.uri = uri;            
        }

        public void RequestData(int object_id)
        {
            string request_uri = uri + object_id.ToString();
            GetRequest(OnHTTPSuccess, OnHTTPError ,request_uri);
        }

        void GetRequest(Action<string> onSuccess, Action<string> onError,string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream dataStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    string responseText = reader.ReadToEnd();
                    onSuccess?.Invoke(responseText);
                }
            }
            catch (Exception e)
            {
                onError?.Invoke(e.Message);
            }
        }
    }

}