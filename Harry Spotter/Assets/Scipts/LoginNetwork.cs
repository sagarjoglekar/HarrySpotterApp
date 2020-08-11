using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using SimpleJson1;


public class LoginNetwork : MonoBehaviour
{
    [System.Serializable]
    public class PlayerInfo
    {
        public string userId;

        public static PlayerInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<PlayerInfo>(jsonString);
        }
    }

    public class UserAccount
    {
        public string email;
        public string password;

        public UserAccount(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        public string ToJsonString()
        {
            string data = JsonUtility.ToJson(this);
            return data;
        }

    }

    public IEnumerator PostUserAccount(string url, string bodyJsonString, System.Action<string> callback)
    {

        print(bodyJsonString);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 10; //Setting the timeout request
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Network Error: " + request.responseCode + request.error);
        } else { 

            Debug.Log("Status Code: " + request.responseCode);
            //getting the body from call
            //Debug.Log(request.downloadHandler.text);
            var data = request.downloadHandler.text;
            PlayerInfo playerInfo = PlayerInfo.CreateFromJSON(data); ;
            Debug.Log("UserID: " + playerInfo.userId);

            callback(playerInfo.userId);
        }
    }


    [System.Serializable]
    public class UserId
    {
        public string userId;

        public UserId(string userId)
        {
            this.userId = userId;
        }

        public JSONNode Serialize()
        {
            var n = new JSONObject();
            n["userId"] = userId;
            return n;
        }
    }

    [System.Serializable]
    public class UserAccountDetailCallBack
    {
        public int defenderScore;
        public int house;
        public List<string> mayourships;
        public int score;
        public string userId;

        public UserAccountDetailCallBack(int defenderScore = 0,int house=0, List<string> mayourships=null, int score =0, string userId = "")
        {
            this.defenderScore = defenderScore;
            this.house = house;
            this.mayourships = mayourships;
            this.score = score;
            this.userId = userId;
        }

        public static UserAccountDetailCallBack CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<UserAccountDetailCallBack>(jsonString);
        }
    }

    public IEnumerator PostUserInfo(string url, string bodyJsonString, System.Action<UserAccountDetailCallBack> callback)
    {
        print(bodyJsonString);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 10;//Setting the TimeOut Time
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Network Error");
        } else
        {
            Debug.Log("Status Code: " + request.responseCode);

            //getting the body from call
            Debug.Log(request.downloadHandler.text);
            if (request.downloadHandler.text == "401")
            {
                UserAccountDetailCallBack emptyCallBack = new UserAccountDetailCallBack();
                callback(emptyCallBack);
            }
            else
            {
                var data = request.downloadHandler.text;
                UserAccountDetailCallBack userAccountDetailCallBack = UserAccountDetailCallBack.CreateFromJSON(data);
                callback(userAccountDetailCallBack);
            }
        }
    }
}
