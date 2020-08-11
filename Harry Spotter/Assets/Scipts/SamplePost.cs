/*
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class PlayerInfoTest
{
    public string userId;
    
    public static PlayerInfoTest CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<PlayerInfoTest>(jsonString);
    }

    // Given JSON input:
    // {"name":"Dr Charles","lives":3,"health":0.8}
    // this example will return a PlayerInfo object with
    // name == "Dr Charles", lives == 3, and health == 0.8f.
}

public class Account
{
    public string email;
    public string password;

    public Account(string email, string password)
    {
        this.email = email;
        this.password = password;
    }

    public string ToJsonString()
    {
        //string data = "{\"email\":" + "\"" + this.email + "\"" + "," + "\"password\":" + "\"" + this.password + "\"" + "}";
        string data = JsonUtility.ToJson(this);
        return data;
    }

}

public class SamplePost : MonoBehaviour
{
    Account newUser = new Account("nima@garbage.com", "secret123");
    void Start()
    {
        
        //"{\"email\":\"nima@garbage.com\",\"password\":\"secret123\"}"
        StartCoroutine(Post("http://sjoglekar-25233.portmap.io:25233/signin", newUser.ToJsonString()));
    }


    IEnumerator Post(string url, string bodyJsonString)
    {
        print(bodyJsonString);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.Send();


        Debug.Log("Status Code: " + request.responseCode);

        //getting the body from call
        Debug.Log(request.downloadHandler.text);
        var data = request.downloadHandler.text;
        PlayerInfoTest playerInfo = PlayerInfoTest.CreateFromJSON(data); ;
        Debug.Log(playerInfo.userId);

    }
}
*/

