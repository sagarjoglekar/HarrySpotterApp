using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using SimpleJson1;
//test
using UnityEngine.UI;
public class GameNetwork : MonoBehaviour
{
    #region Add Object
    [System.Serializable]
    public class GameNetworkImage
    {
        public string userId;
        public string image;
        public string lon;
        public string lat;
        public string event_id;
        public GameNetworkImage(string userId, string image, string lon, string lat, string event_id = "")
        {
            this.userId = userId;
            this.image = image;
            this.lon = lon;
            this.lat = lat;
            this.event_id = event_id;
        }

        public JSONNode Serialize()
        {
            var n = new JSONObject();
            n["userId"] = userId;
            n["image"] = image;
            n["lon"] = lon;
            n["lat"] = lat;
            n["event_id"] = event_id;
            return n;
        }

    }

    [System.Serializable]
    public class ImageInfo
    {
        public List<string> objectId = new List<string>();

        public static ImageInfo CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ImageInfo>(jsonString);
        }
    }
    List<string> emptyObject = new List<string>();


    public IEnumerator PostAddObject(string url, string bodyJsonString, System.Action<List<string>> callback)
    {

        print(bodyJsonString);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        //yield return request.Send();
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        //getting the body from call
        Debug.Log("Add Object Download Handler: " + request.downloadHandler.text);
        //for cases that backend object detection does not detect any object
        if(request.downloadHandler.text == "400")
        {
            emptyObject.Clear();
            emptyObject.Add("NoObjectId");
            callback(emptyObject);
        } else
        {
            
            //for cases that backend can not find the object
            var data = request.downloadHandler.text;
            ImageInfo imageInfo = ImageInfo.CreateFromJSON(data);
            //Debug.Log("House: " + houseInfo.house);
            callback(imageInfo.objectId);
        }
        //Testing no oject senario
        //emptyObject.Clear();
        //emptyObject.Add("NoObjectId");
        //callback(emptyObject);
    }
    #endregion

    #region Annotation
    [System.Serializable]
    public class AnnotationDetail
    {
        public string userId;
        public List<string> objectId;
        public string label_user;
        public string need_label_user;
        public string event_id;
        public AnnotationDetail(string userId, List<string> objectId, string user_label, string need_label_user, string event_id = "")
        {
            this.userId = userId;
            this.objectId = objectId;
            this.label_user = user_label;
            this.need_label_user = need_label_user;
            this.event_id = event_id;
        }
        public string ToJsonString()
        {
            string data = JsonUtility.ToJson(this);
            return data;
        }
    }

    [System.Serializable]
    public class AnotationCallBackData
    {
        public float score_user;
        public string userId;

        public static AnotationCallBackData CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<AnotationCallBackData>(jsonString);
        }
    }

    public IEnumerator PostAnnotateObject(string url, string bodyJsonString, System.Action<float, string> callback)
    {

        print(bodyJsonString);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        //yield return request.Send();
        yield return request.SendWebRequest();
        Debug.Log("Post Annotate Object Status Code: " + request.responseCode);
        //getting the body from call
        Debug.Log(request.downloadHandler.text);
        var data = request.downloadHandler.text;
        AnotationCallBackData anotationCallBackData = AnotationCallBackData.CreateFromJSON(data);
        callback(anotationCallBackData.score_user, anotationCallBackData.userId);
    }
    #endregion

    #region GetEents
    [System.Serializable]
    public class GameNetworkEvent
    {
        public string userId;
        public string lon;
        public string lat;

        public GameNetworkEvent(string userId, string lon, string lat)
        {
            this.userId = userId;
            this.lon = lon;
            this.lat = lat;
        }

        public JSONNode Serialize()
        {
            var n = new JSONObject();
            n["userId"] = userId;
            n["lon"] = lon;
            n["lat"] = lat;
            return n;
        }
    }

    //Process the data
    [System.Serializable]
    public class Event
    {
        public string event_id;
        public float poi_lat;
        public float poi_lon;
        public string poi_type;
        public string defenderId;
        public int mayor_house;
        public string name;
        public float prob_poi;

        public Event(string event_id, float poi_lat, float poi_lon, string poi_type, string defenderId, int mayor_house, string name, float prob_poi)
        {
            this.event_id = event_id;
            this.poi_lat = poi_lat;
            this.poi_lon = poi_lon;
            this.poi_type = poi_type;
            this.defenderId = defenderId;
            this.mayor_house = mayor_house;
            this.name = name;
            this.prob_poi = prob_poi;
        }

        public static Event CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<Event>(jsonString);
        }
    }

    [System.Serializable]
    public class EventCallBack
    {
        public List<Event> events;

        public EventCallBack() { }

        public EventCallBack(List<Event> events)
        {
            this.events = events;
        }


        public static EventCallBack CreateFromJSON(string jsonString)
        {
            //callbackEvents = (List<Event>)JsonUtility.FromJson(jsonString,typeof(Event));
            return JsonUtility.FromJson<EventCallBack>(jsonString);
            //return JsonUtility.FromJson<EventCallBack>(jsonString);

        }
    }

    public IEnumerator GetEvent(string url, string bodyJsonString, System.Action<EventCallBack> callback)
    {

        print(bodyJsonString);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.timeout = 10; //Setting the timeout request;
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log("Network Error: " + request.responseCode + request.error);
        }
        else
        {
            Debug.Log("Status Code: " + request.responseCode);
            //getting the body from call
            Debug.Log("Download Handler: "+ request.downloadHandler.text);
            var data = request.downloadHandler.text;
            EventCallBack eventCallBack = EventCallBack.CreateFromJSON(data);
            callback(eventCallBack);
        }
    }
    #endregion

    #region GetEventMetaData
    [System.Serializable]
    public class EventID
    {
        public string eventId;

        public EventID(string eventId)
        {
            this.eventId = eventId;
        }

        public JSONNode Serialize()
        {
            var n = new JSONObject();
            n["eventId"] = eventId;
            return n;
        }
    }

    [System.Serializable]
    public class EventMetaDataCallBack
    {
        public string defenderId;
        public string eventId;
        public string eventType;
        public float lat;
        public float lon;
        public int mayorHouse;
        public string eventName;

        public static EventMetaDataCallBack CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<EventMetaDataCallBack>(jsonString);
        }
    }

    public IEnumerator GetEventMetaData(string url, string bodyJsonString, System.Action<EventMetaDataCallBack> callback)
    {
        print(bodyJsonString);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        //yield return request.Send();
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        //getting the body from call
        Debug.Log(request.downloadHandler.text);
        var data = request.downloadHandler.text;
        EventMetaDataCallBack eventMetaDataCallBack = EventMetaDataCallBack.CreateFromJSON(data);
        callback(eventMetaDataCallBack);
        //ImageInfo imageInfo = ImageInfo.CreateFromJSON(data);
        //Debug.Log("House: " + houseInfo.house);
        //callback(imageInfo.objectId);
    }
    #endregion

    #region getFightStatus
    [System.Serializable]
    public class FightStatusCallBack
    {
        public int defenderScore;
        public string defenderId;
        public static FightStatusCallBack CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<FightStatusCallBack>(jsonString);
        }
    }

    public IEnumerator GetFightStatus(string url, string bodyJsonString, System.Action<FightStatusCallBack> callback)
    {
        print(bodyJsonString);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        //yield return request.Send();
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        //getting the body from call
        Debug.Log(request.downloadHandler.text);
        var data = request.downloadHandler.text;
        FightStatusCallBack fightStatusCallBack = FightStatusCallBack.CreateFromJSON(data);
        callback(fightStatusCallBack);
        //ImageInfo imageInfo = ImageInfo.CreateFromJSON(data);
        //Debug.Log("House: " + houseInfo.house);
        //callback(imageInfo.objectId);
    }
    #endregion

    #region StartFight
    [System.Serializable]
    public class EventSet
    {
        public string eventId;
        public string userId;

        public EventSet(string eventId, string userId)
        {
            this.eventId = eventId;
            this.userId = userId;
        }

        public JSONNode Serialize()
        {
            var n = new JSONObject();
            n["eventId"] = eventId;
            n["userId"] = userId;
            return n;
        }
    }

    [System.Serializable]
    public class StartFightCallBack
    {
        public string lockMutex;

        public static StartFightCallBack CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<StartFightCallBack>(jsonString);
        }
    }

    public IEnumerator StartFight(string url, string bodyJsonString, System.Action<StartFightCallBack> callBack)
    {
        print(bodyJsonString);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        //yield return request.Send();
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        //getting the body from call
        Debug.Log(request.downloadHandler.text);
        var data = request.downloadHandler.text;
        StartFightCallBack eventSetCallBack = StartFightCallBack.CreateFromJSON(data);
        callBack(eventSetCallBack);
    }

    #region setFightStatus
    [System.Serializable]
    public class FightSet
    {
        public string eventId;
        public string userId;
        public bool isWinner;
        public string lockMutex;

        public FightSet(string eventId, string userId, bool isWinner, string lockMutex)
        {
            this.eventId = eventId;
            this.userId = userId;
            this.isWinner = isWinner;
            this.lockMutex = lockMutex;
        }

        public JSONNode Serialize()
        {
            var n = new JSONObject();
            n["eventId"] = eventId;
            n["userId"] = userId;
            n["isWinner"] = isWinner;
            n["lockMutex"] = lockMutex;
            return n;
        }

    }

    public IEnumerator SetFightSatus(string url, string bodyJsonString)
    {
        print(bodyJsonString);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        //yield return request.Send();
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        //getting the body from call
        Debug.Log(request.downloadHandler.text);
    }
    #endregion


    #endregion

    #region Leaderboard
    [System.Serializable]
    public class LeaderboardCallBackData
    {
        // 1: Hufflepuff, 2: Ravenclaw, 3: Griffindor , 4: Slytherin
        public float house1Score;
        public float house2Score;
        public float house3Score;
        public float house4Score;

        public static LeaderboardCallBackData CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<LeaderboardCallBackData>(jsonString);
        }
    }

    public IEnumerator GetLeaderboard(string url, System.Action<float, float, float, float> callback)
    {

        var request = new UnityWebRequest(url, "Get");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        //yield return request.Send();
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        //getting the body from call
        Debug.Log(request.downloadHandler.text);
        var data = request.downloadHandler.text;
        LeaderboardCallBackData leaderboardCallBackData = LeaderboardCallBackData.CreateFromJSON(data);
        callback(leaderboardCallBackData.house1Score, leaderboardCallBackData.house2Score, leaderboardCallBackData.house3Score, leaderboardCallBackData.house4Score);
        ////Debug.Log("House: " + houseInfo.house);
        //callback(imageInfo.objectId);
    }
    #endregion

    #region DeleteUser
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

    public IEnumerator DeleteUser(string url, string bodyJsonString, System.Action<string> callback)
    {
        print(bodyJsonString);
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log("Status Code: " + request.responseCode);

        //getting the body from call
        //Debug.Log(request.downloadHandler.text);
        var data = request.downloadHandler.text;
        callback(data);

        #endregion
    }

    #region GetUserAccountStats
    [System.Serializable]
    public class UserAccountDetailCallBack
    {
        public int defenseScore;
        public int house;
        public List<string> mayorships;
        public int score;
        public string userId;

        public UserAccountDetailCallBack(int defenseScore = 0, int house = 0, List<string> mayorships = null, int score = 0, string userId = "")
        {
            this.defenseScore = defenseScore;
            this.house = house;
            this.mayorships = mayorships;
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

        yield return request.SendWebRequest();

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
    #endregion
}
