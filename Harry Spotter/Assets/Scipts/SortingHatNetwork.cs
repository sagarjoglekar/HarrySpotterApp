using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using SimpleJson1;

public class SortingHatNetwork : MonoBehaviour
{
    [System.Serializable]
    public class HouseInfo
    {
        public int house;

        public static HouseInfo CreateFromJSON(string jsonString)
        {
            // 1: Hufflepuff, 2: Ravenclaw, 3: Griffindor , 4: Slytherin
            return JsonUtility.FromJson<HouseInfo>(jsonString);
        }
    }

    [System.Serializable]
    public class HatQuiz
    {
        public string userId;
        public Dictionary<string, int> questions = new Dictionary<string, int>();

        public HatQuiz(string userId, int[] answares)
        {
            this.userId = userId;
            //questions.Add("q1", 1);
            //this.questions = answares;
            for (int i = 0; i < answares.Length; i++)
            {
                string key = "q" + (i + 1).ToString();
                int value = answares[i];
                //questions[key] = value;
                questions.Add(key, value);
            }
        }

        public JSONNode Serialize()
        {
            var n = new JSONObject();
            n["userId"] = userId;
            var h = n["questions"].AsObject;
            foreach (var v in questions)
                h[v.Key] = v.Value;
            return n;
        }
    }

    public IEnumerator PostRegistery(string url, string bodyJsonString, System.Action<int> callback)
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
        HouseInfo houseInfo = HouseInfo.CreateFromJSON(data);
        //Debug.Log("House: " + houseInfo.house);
        callback(houseInfo.house);
    }
}
