using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mapbox.Examples;
using UnityEngine.Networking;
using static GameNetwork;
public class UIManagerMap : MonoBehaviour
{
    [Tooltip("Set to false by defalut, allows you activate your custom object")]
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private Material[] _housesMaterials;// Local user house: 1: Hufflepuff, 2: Ravenclaw, 3: Griffindor , 4: Slytherin
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private float _fadeTime = 1f;
    [SerializeField] private GameObject _mapPanel;
    [SerializeField] private GameObject _startImage;
    [SerializeField] private GameObject _locationBasedGame;
    [SerializeField] private GameObject _scanAreaButton;
    [SerializeField] private GameObject _infoText;

    //Spell Energy UI indicator
    [SerializeField] private Color[] _myColors;
    [SerializeField] [Range(0f, 1f)] float _learpTime;
    private float _tempScore;
    //Map Panel
    [SerializeField] private Image[] _spellEnergyImage;//0 map panel, 1 Congratulation Panel
    private int spellEnergyImageIndex;

    //Capture Panel
    [SerializeField] private GameObject _capturePanel;
    [SerializeField] private InputField _inputFieldRef;
    [SerializeField] private GameObject _rawImage;
    [SerializeField] private GameObject _blackBackground;
    [SerializeField] private GameObject _nextButton;
    [SerializeField] private Animator _inputFiledLabelTransition;

    //Question Panel
    [SerializeField] private GameObject _questionPanel;
    [SerializeField] private Toggle[] _answares;

    //Congratulation Panel
    [SerializeField] private GameObject _congratulationPanel;
    [SerializeField] private Text _congratulationText;

    //Leaderboard Panel
    [SerializeField] private GameObject _leaderboardPanel;
    [SerializeField] private Image _playerHouseImage;
    [SerializeField] private GameObject[] _housesObjects;
    [SerializeField] private Sprite[] _houseLogoSprite;
    [SerializeField] private Text _spellEnergyText;
    [SerializeField] private Text _mayourshipsText;
    [SerializeField] private Text _defendingPowerText;
    [SerializeField] private Animator _leaderboardTransition;
    [SerializeField] private GameObject _deleteAccount;
    private int tempHouse;
    private bool _isDeleteAccountSelected = false;

    

    //EventPanel
    [SerializeField] private GameObject _evetPanel;
    [SerializeField] private Image _mayorHouseLogoImage;
    [SerializeField] private Text _mayorText;
    [SerializeField] private Text _distanceText;
    [SerializeField] private GameObject _walkCloserText;
    [SerializeField] private GameObject _cliamMayorshipButton;
    [SerializeField] private GameObject _busyMayorText;

    //Local User
    [SerializeField] private LocalUser _localUser;

    //CurrentInfo
    private string tempEventID;
    private int tempEventHouse;
    private int tempDefenderScore;

    //test
    [SerializeField] private Text _buttonText;

    //User Location
    [SerializeField] private LocationStatus _locationStatus;

    //Game Network
    [SerializeField] private GameNetwork _gameNetwork;

    //Tesing
    [SerializeField] private Text _networkStatus;

    private void Awake()
    {
        _localUser.LoadData();
        //_networkStatus.text = "NetWork Status: " + Application.internetReachability.ToString();
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //Change the Text
            //_networkStatus.text = "NetWork Status: " + "Not Reachable.";
            Debug.Log("NetWork Status: " + "Not Reachable.");
        }
        //Check if the device can reach the internet via a carrier data network
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            //_networkStatus.text = "NetWork Status: " + "Reachable via carrier data network.";
            Debug.Log("NetWork Status: " + "Reachable via carrier data network.");
        }
        //Check if the device can reach the internet via a LAN
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Debug.Log("NetWork Status: " + "Reachable via Local Area Network.");
            //_networkStatus.text = "NetWork Status: " + "Reachable via Local Area Network.";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        if (_debugMode == false)
        {
            foreach(var item in _playerGameObject.GetComponentsInChildren<MeshRenderer>())
            {
                item.material = _housesMaterials[_localUser.userHouse - 1];
            }
            //_playerGameObject.GetComponentInChildren<MeshRenderer>().material = _housesMaterials[_localUser.userHouse - 1];
            _scanAreaButton.SetActive(true);
            _leaderboardPanel.SetActive(true);
            _capturePanel.SetActive(false);
            _startImage.SetActive(true);
            _nextButton.SetActive(false);
            _rawImage.SetActive(false);
            _blackBackground.SetActive(false);
            StartCoroutine(FadeCanvasGroup(true, _startImage.GetComponent<CanvasGroup>()));
            _startImage.SetActive(false);
            //DisplaySpelEnergy(0);

        }
    }
    // Update is called once per frame
    void Update()
    {
        DisplaySpelEnergy();

    }

    private void DisplaySpelEnergy()
    {
        if(_localUser.score != _tempScore)
        {
            if(_mapPanel.activeSelf == true)
            {
                spellEnergyImageIndex = 0;
            }
            if(_congratulationPanel.activeSelf == true)
            {
                spellEnergyImageIndex = 1;
            }
            float time = 0f;
            time += Time.time;
            if (_localUser.score < 30)
            {
                _spellEnergyImage[spellEnergyImageIndex].color = Color.Lerp(_spellEnergyImage[spellEnergyImageIndex].color, _myColors[0], Mathf.PingPong(Time.time, 1));
            }
            else if (_localUser.score < 90)
            {
                _spellEnergyImage[spellEnergyImageIndex].color = Color.Lerp(_spellEnergyImage[spellEnergyImageIndex].color, _myColors[1], Mathf.PingPong(Time.time, 1));
            }
            else if (_localUser.score < 270)
            {
                _spellEnergyImage[spellEnergyImageIndex].color = Color.Lerp(_spellEnergyImage[spellEnergyImageIndex].color, _myColors[2], Mathf.PingPong(Time.time, 1));
            }
            else if (_localUser.score < 500)
            {
                _spellEnergyImage[spellEnergyImageIndex].color = Color.Lerp(_spellEnergyImage[spellEnergyImageIndex].color, _myColors[3], Mathf.PingPong(Time.time, 1));
            }
            else if (_localUser.score >= 1000)
            {
                _spellEnergyImage[spellEnergyImageIndex].color = Color.Lerp(_spellEnergyImage[spellEnergyImageIndex].color, _myColors[4], Mathf.PingPong(Time.time, 1));
            }
            if(time >= 1f)
            {
                _tempScore = _localUser.score;
            }
        }


    }

    private IEnumerator FadeCanvasGroup(bool fadeAway, CanvasGroup canvasGroup)
    {
        if (fadeAway)
        {
            //frade opauqe to transparent
            for (float i = _fadeTime; i >= 0; i -= Time.deltaTime)
            {
                canvasGroup.alpha = i;
                yield return null;
            }
        }
        //fade from transparent to opaque
        else
        {
            //loop over 1 second
            for (float i = 0; i <= _fadeTime; i += Time.deltaTime)
            {
                canvasGroup.alpha = i;
                yield return null;
            }
        }
    }

    public void OnLeaderboardButtonClick()
    {
        if(_leaderboardTransition.GetBool("isLeaderboardOn") == false){
            _leaderboardPanel.SetActive(true);
            _leaderboardTransition.SetBool("isLeaderboardOn", true);
            UserId userId = new UserId(_localUser.userID);
            StartCoroutine(_gameNetwork.PostUserInfo("https://sjoglekar-45523.portmap.io:45523/getUser", userId.Serialize().ToString(),GetUserAccountStats));
            StartCoroutine(_gameNetwork.GetLeaderboard("https://sjoglekar-45523.portmap.io:45523/getGlobalLeaderboard", GetLeaderboardDetail));
            DisplayUserHouseLogo();
        } else
        {
            _leaderboardTransition.SetBool("isLeaderboardOn", false);
        }

        //StartCoroutine(FadeCanvasGroup(false, _leaderboardPanel.GetComponent<CanvasGroup>()));
        //GetLeaderboardDetail(8.0f, 10.0f, 6.0f, 5.0f);
    }

    private void DisplayUserHouseLogo()
    {
        if (_localUser.userHouse == 1)
        {
            _playerHouseImage.sprite = _houseLogoSprite[0];
        }
        else if (_localUser.userHouse == 2)
        {
            _playerHouseImage.sprite = _houseLogoSprite[1];
        }
        else if (_localUser.userHouse == 3)
        {
            _playerHouseImage.sprite = _houseLogoSprite[2];
        }
        else if (_localUser.userHouse == 4)
        {
            _playerHouseImage.sprite = _houseLogoSprite[3];
        }
    }

    private void GetUserAccountStats(UserAccountDetailCallBack userInfo)
    {
        Debug.Log("User Score: "+ userInfo.score);
        _spellEnergyText.text = "Spell Energy " + "<b>" + userInfo.score + "</b>";

        Debug.Log("Number of mayorships: " + userInfo.mayorships.Count);
        _mayourshipsText.text = "Mayorships " + "<b>" + userInfo.mayorships.Count.ToString() + "</b>";

        Debug.Log("Defender Score: " + userInfo.defenseScore);
        _defendingPowerText.text = "Defending Power " + "<b>" + userInfo.defenseScore + "</b>";

    }

    private class HousesData
    {
        public string houseName;
        public float houseScore;
        public HousesData(string houseName, float houseScore)
        {
            this.houseName = houseName;
            this.houseScore = houseScore;
        }
    }

    private void GetLeaderboardDetail(float house1Score, float house2Score, float house3Score, float house4Score)
    {
        // 1: Hufflepuff, 2: Ravenclaw, 3: Griffindor , 4: Slytherin
        print("House 1 (Hufflepuff) Score is: " + house1Score);
        print("House 2 (Ravenclaw) Score is: " + house2Score);
        print("House 3 (Gryffindor) Score is: " + house3Score);
        print("House 4 (Slytherin) score is: " + house4Score);

        //Process the data
        HousesData hufflepuff = new HousesData("Hufflepuff", house1Score);
        HousesData ravenclaw = new HousesData("Ravenclaw", house2Score);
        HousesData gryffindor = new HousesData("Gryffindor", house3Score);
        HousesData slytherin = new HousesData("Slytherin", house4Score);
        HousesData[] housesDatas = { hufflepuff, ravenclaw, gryffindor, slytherin };

        //Sorting House Data array from highest to lowest score
        for (int i = 0; i < housesDatas.Length; i++)
        {
            for (int j=i+1; j <housesDatas.Length; j++)
            {
                if(housesDatas[j].houseScore > housesDatas[i].houseScore)
                {
                    //Sawp
                    HousesData temp = housesDatas[i];
                    housesDatas[i] = housesDatas[j];
                    housesDatas[j] = temp;
                }
            }
        }

        //Displaying the house name and logo in order from highest to lowest score
        int count = 0;
        foreach (var i in housesDatas)
        {
            _housesObjects[count].GetComponentInChildren<Text>().text = i.houseName + ": " + "<b>" + Mathf.RoundToInt(i.houseScore*100) + "</b>";
            if(i.houseName == "Hufflepuff")
            {
                _housesObjects[count].GetComponentInChildren<Image>().sprite = _houseLogoSprite[0];
            } else if (i.houseName == "Ravenclaw")
            {
                _housesObjects[count].GetComponentInChildren<Image>().sprite = _houseLogoSprite[1];
            } else if(i.houseName == "Gryffindor")
            {
                _housesObjects[count].GetComponentInChildren<Image>().sprite = _houseLogoSprite[2];
            } else if (i.houseName == "Slytherin")
            {
                _housesObjects[count].GetComponentInChildren<Image>().sprite = _houseLogoSprite[3];
            }
            count++;
            print(i.houseName + ": " + i.houseScore);
        }

    }

    public void DisplayMapPanel()
    {
        _rawImage.SetActive(false);
        _blackBackground.SetActive(false);
        _capturePanel.SetActive(false);
        _scanAreaButton.SetActive(true);
        _leaderboardPanel.SetActive(true);
        StartCoroutine(FadeCanvasGroup(false, _scanAreaButton.GetComponent<CanvasGroup>()));
        _mapPanel.SetActive(true);
    }

    public void DisplayCapturePanel()
    {
        StartCoroutine(FadeCanvasGroup(true, _scanAreaButton.GetComponent<CanvasGroup>()));
        _leaderboardPanel.SetActive(false);
        _capturePanel.SetActive(true);
        _rawImage.SetActive(true);
        _blackBackground.SetActive(true);
        _nextButton.SetActive(true);
        //_inputFieldRef.inputType = InputField.InputType.AutoCorrect;//keyboard auto correct
        _inputFieldRef.transform.GetChild(1).GetComponent<Text>().text = "Give a label for this object";
        _inputFieldRef.text = "";
        _inputFieldRef.placeholder.color = Color.gray;
        if (_congratulationPanel.activeSelf == true)
        {
            _congratulationPanel.SetActive(false);
            StartCoroutine(FadeCanvasGroup(false, _capturePanel.GetComponent<CanvasGroup>()));
            StartCoroutine(FadeCanvasGroup(false, _nextButton.GetComponent<CanvasGroup>()));

        } else
        {
            StartCoroutine(FadeCanvasGroup(false, _capturePanel.GetComponent<CanvasGroup>()));
            StartCoroutine(FadeCanvasGroup(false, _nextButton.GetComponent<CanvasGroup>()));
            _scanAreaButton.SetActive(false);
            _mapPanel.SetActive(false);
        }
    }

    public void DisplayQuestionPanel()
    {
        foreach(var answer in _answares)
        {
            answer.isOn = false;
        }
        _rawImage.GetComponent<RawImage>().color = Color.gray;
        _capturePanel.SetActive(false);
        StartCoroutine(FadeCanvasGroup(true, _nextButton.GetComponent<CanvasGroup>()));
        _nextButton.SetActive(false);
        _questionPanel.SetActive(true);
        StartCoroutine(FadeCanvasGroup(false, _questionPanel.GetComponent<CanvasGroup>()));
        print("Display Question");
    }

    public void DisplayCongratulationPanel(float score)
    {
        _congratulationText.text = "<b> Congratulation!</b> You increased your spell to " + score.ToString();
        StartCoroutine(FadeCanvasGroup(true, _questionPanel.GetComponent <CanvasGroup>()));
        _questionPanel.SetActive(false);
        _congratulationPanel.SetActive(true);
        StartCoroutine(FadeCanvasGroup(false, _congratulationPanel.GetComponent<CanvasGroup>()));
    }

    public void ButtonTest()
    {
        /*
        if(_buttonText.text == "Off")
        {
            _buttonText.text = "On";
        } else
        {
            _buttonText.text = "Off";
        }
        */
        //DisplayCongratulationPanel();
        DisplayQuestionPanel();
    }

    public void OnBackToMapButtonClick()
    {
        _rawImage.SetActive(false);
        _blackBackground.SetActive(false);
        StartCoroutine(FadeCanvasGroup(true, _congratulationPanel.GetComponent<CanvasGroup>()));
        _congratulationPanel.SetActive(false);
        _mapPanel.SetActive(true);
        _leaderboardPanel.SetActive(true);
        _scanAreaButton.SetActive(true);
        StartCoroutine(FadeCanvasGroup(false, _scanAreaButton.GetComponent<CanvasGroup>()));
    }

    public void DisplayEventPanel(string eventID,int houseNumber, float eventLat, float eventLon, string eventName, string eventType,string defenderId)
    {
        
        EventID eventID1 = new EventID(eventID);
        StartCoroutine(_gameNetwork.GetFightStatus("https://sjoglekar-45523.portmap.io:45523/getFightStatus", eventID1.Serialize().ToString(), GetFightStatusCallBack));
        var userLocation = new GeoCoordinatePortable.GeoCoordinate(double.Parse(_locationStatus.GetLocationLon()), double.Parse(_locationStatus.GetLocationLat()));
        var eventLocation = new GeoCoordinatePortable.GeoCoordinate(double.Parse(eventLat.ToString()), double.Parse(eventLon.ToString()));
        //Debug.Log(userLocation);
        //Debug.Log(eventLocation);
        var distance = userLocation.GetDistanceTo(eventLocation);
        _distanceText.text = "Distance: " + Mathf.RoundToInt((float)distance) + "m";
        if (distance < 800)//turn back to 80
        {
            tempEventID = eventID;
            tempEventHouse = houseNumber;
            _walkCloserText.SetActive(false);
            _cliamMayorshipButton.SetActive(true);
        }
        else
        {
            _walkCloserText.SetActive(true);
            _cliamMayorshipButton.SetActive(false);

        }

        tempHouse = houseNumber;
        string[] houseNames = new string[] { "Hufflepuff", "Ravenclaw", "Griffindor", "Slytherin"};
        _mayorText.text = "";
        _busyMayorText.SetActive(false);
        if(houseNumber == 0)
        {
            _mayorText.text = "No mayor to defend " + "<color=#C8BEBE>"  + eventName +  "</color>";
            _mayorHouseLogoImage.gameObject.SetActive(false);
        } else
        {
            _mayorHouseLogoImage.gameObject.SetActive(true);
            // 1: Hufflepuff, 2: Ravenclaw, 3: Griffindor , 4: Slytherin
            _mayorHouseLogoImage.sprite = _houseLogoSprite[houseNumber-1];
            if(defenderId == _localUser.userID)
            {
                _mayorText.text = "You are the Mayor of " + eventName;
                _cliamMayorshipButton.SetActive(false);
            }
            else
            {
                _mayorText.text = "The Mayor of " + "<color=#C8BEBE>" + eventName + "</color>" + " from " + houseNames[houseNumber - 1] + " is defending it";
            }
        }
        _evetPanel.SetActive(true);
    }

    public void GetFightStatusCallBack(FightStatusCallBack fightStatusCallBack)
    {
        print(fightStatusCallBack.defenderScore);
        tempDefenderScore = fightStatusCallBack.defenderScore;
        if(tempHouse != 0)
        {
            _mayorText.text = _mayorText.text + " with power " + fightStatusCallBack.defenderScore;
        }
    }

    public void OnCliamMayorshipButtonClick()
    {
        UserId userId = new UserId(_localUser.userID);
        StartCoroutine(_gameNetwork.PostUserInfo("https://sjoglekar-45523.portmap.io:45523/getUser", userId.Serialize().ToString(), GetUserDefenderScore));
        EventSet eventSet = new EventSet(tempEventID, _localUser.userID);
        StartCoroutine(_gameNetwork.StartFight("https://sjoglekar-45523.portmap.io:45523/startFight", eventSet.Serialize().ToString(), StartFightCallBack));
    }

    public void StartFightCallBack(StartFightCallBack startFightCallBack)
    {
        Debug.Log("Mutex: " + startFightCallBack.lockMutex);
        if(startFightCallBack.lockMutex == "")
        {
            _busyMayorText.SetActive(true);
        } else
        {
            _localUser.currentEventId = tempEventID;
            _localUser.currentDefenderScore = tempDefenderScore;
            _localUser.currentEventHouse = tempEventHouse;
            _localUser.currentMutex = startFightCallBack.lockMutex;
            _localUser.SaveData();
            SceneManager.LoadScene("Fight");
        }
    }

    public void GetUserDefenderScore(UserAccountDetailCallBack userInfo)
    {
        _localUser.currentUserDefenderScore = userInfo.defenseScore;
        _localUser.SaveData();
  
    }

    public void OnCloseEventPanelButtonClick()
    {
        _evetPanel.SetActive(false);
        //StartCoroutine(FadeCanvasGroup(true, _evetPanel.GetComponent<CanvasGroup>()));
        //StartCoroutine(DelayActive(_evetPanel, false, _fadeTime));
    }

    //playing give a lable animation fo inputfild lift it up
    public void GiveLabelPointerDown()
    {
        _inputFieldRef.transform.GetChild(1).GetComponent<Text>().text = "";
        _inputFiledLabelTransition.SetBool("ClickedOnGiveLabel", true);
    }

    public void OnGiveLabelDeselect()
    {
        if (_inputFieldRef.transform.GetChild(2).GetComponent<Text>().text == "")
        {
            _inputFieldRef.transform.GetChild(1).GetComponent<Text>().text = "Give a label for this object";
        }
        _inputFiledLabelTransition.SetBool("ClickedOnGiveLabel", false);
    }

    public void OnSignOutButtonClick()
    {
        LocalUser emptyUser = new LocalUser();
        _localUser = emptyUser;
        _localUser.SaveData();
        SceneManager.LoadScene("LogIn");
    }

    public void OnDeleteAccountSelected()
    {
        _isDeleteAccountSelected = true;
        _deleteAccount.GetComponent<Image>().color = new Color32(255,0,0,200);
        _deleteAccount.GetComponentInChildren<Text>().text = "Hold For 5 Seconds";
        _deleteAccount.GetComponentInChildren<Text>().fontSize = 54;
        StartCoroutine(DelteUserAaccount());
    }

    public void OnDeleteAccountDeselected()
    {
        _isDeleteAccountSelected = false;
        _deleteAccount.GetComponent<Image>().color = new Color32(214, 214, 214, 81);
        _deleteAccount.GetComponentInChildren<Text>().text = "Delete Account";
        _deleteAccount.GetComponentInChildren<Text>().fontSize = 65;
        Debug.Log("Not Holding Dlete Button");
    }

    public IEnumerator DelteUserAaccount()
    {
        float timeLeft = 10f;
        while(_isDeleteAccountSelected != false)
        {
            timeLeft -= 1;
            if(timeLeft <= 0)
            {
                _deleteAccount.GetComponentInChildren<Text>().text = "Sending the request";
                UserId userId = new UserId(_localUser.userID);
                StartCoroutine(_gameNetwork.DeleteUser("https://sjoglekar-45523.portmap.io:45523/deleteUser", userId.Serialize().ToString(), DeleteAccountCallBack));
                break;
            }
            _deleteAccount.GetComponentInChildren<Text>().text = "Hold For " + Mathf.RoundToInt(timeLeft) +" Seconds";
            yield return new WaitForSeconds(1f);//Refresh rate
        }
        yield return new WaitUntil(() => (_isDeleteAccountSelected == false));
        Debug.Log("Exit IEnumerator");
        yield break;
    }

    private void DeleteAccountCallBack(string callbackInfo)
    {
        Debug.Log(callbackInfo);
        if(callbackInfo == "200")
        {
            _deleteAccount.GetComponentInChildren<Text>().text = "Account has been deleted";
            Debug.Log("Account has been deleted");
            LocalUser emptyUser = new LocalUser();
            _localUser = emptyUser;
            _localUser.SaveData();
            SceneManager.LoadScene("LogIn");
        }
        else
        {
            _deleteAccount.GetComponentInChildren<Text>().text = "Failed to delete account";
            Debug.Log("Failed to delete account");
        }
    }
}
