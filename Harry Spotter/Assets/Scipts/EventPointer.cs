using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameNetwork;
public class EventPointer : MonoBehaviour
{
    private UIManagerMap _uIManagerMap;
    private GameNetwork _gameNetwork;
    private GameObject _rawImage;
    [SerializeField] private Animator _leaderboardTransition;
    // Start is called before the first frame update
    void Start()
    {
        _uIManagerMap = GameObject.Find("Canvas").GetComponent<UIManagerMap>();
        if(_uIManagerMap == null)
        {
            Debug.LogError("UI Manager is Null");
        }
        _gameNetwork = GameObject.Find("Network").GetComponent<GameNetwork>();
        if(_gameNetwork == null)
        {
            Debug.LogError("Game Network is Null");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        _rawImage = GameObject.Find("RawImage");
        _leaderboardTransition = GameObject.Find("Leaderboard Panel").GetComponent<Animator>();
        if(_rawImage == null && _leaderboardTransition.GetBool("isLeaderboardOn") == false)
        {
            EventID eventID = new EventID(this.gameObject.name);
            StartCoroutine(_gameNetwork.GetEventMetaData("https://sjoglekar-45523.portmap.io:45523/getEventMetadata", eventID.Serialize().ToString(), GetEventMetaData));
            Debug.Log(this.gameObject.name);
        }
 
    }

    private void GetEventMetaData(EventMetaDataCallBack eventMetaDataCallBack)
    {
        Debug.Log("Defender ID: " + eventMetaDataCallBack.defenderId + " Event ID: " + eventMetaDataCallBack.eventId + " Event Name: " + eventMetaDataCallBack.eventName + " Event Type: " + eventMetaDataCallBack.eventType + " lat: " + eventMetaDataCallBack.lat + " lon: " + eventMetaDataCallBack.lon + " Mayor House: " + eventMetaDataCallBack.mayorHouse);
        _uIManagerMap.DisplayEventPanel(eventMetaDataCallBack.eventId,eventMetaDataCallBack.mayorHouse,eventMetaDataCallBack.lat,eventMetaDataCallBack.lon,eventMetaDataCallBack.eventName,eventMetaDataCallBack.eventType,eventMetaDataCallBack.defenderId);

    }


}
