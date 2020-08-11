using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalUser : MonoBehaviour
{
    public string userID;
    // 1: Hufflepuff, 2: Ravenclaw, 3: Griffindor , 4: Slytherin
    public int userHouse;
    public float score;

    public string currentEventId;
    public int currentDefenderScore;
    public int currentUserDefenderScore;
    public int currentEventHouse;
    public string currentMutex;
    [SerializeField] InputField inputField;
    [SerializeField] Text _testText;

    public void SaveData()
    {
        SaveSystem.SaveUserData(this);
    }

    public void LoadData()
    {
        if (SaveSystem.LoadUserData() != null)
        {
            LocalUserData data = SaveSystem.LoadUserData();

            userID = data.userId;
            userHouse = data.userHouse;
            score = data.score;
            currentEventId = data.currentEventId;
            currentEventHouse = data.currentEventHouse;
            currentMutex = data.currentMutex;
            currentDefenderScore = data.currentDefenderScore;
            currentUserDefenderScore = data.currentUserDendenderScore;
        }
        else
        {
            userID = "";
            userHouse = 0;
            score = 0;
            currentEventId = "";
            currentDefenderScore = 0;
            currentUserDefenderScore = 0;
            currentEventHouse = 0;
            currentMutex = "";

        }
    }


    public void onLoadButton()
    {
        LoadData();
        _testText.text = "UserID: " + userID + " HouseName: " + userHouse + "Score: " + score;
    }

    public void onSaveButton()
    {
        userID = inputField.text;
        SaveData();
    }

    public void onLoadDataButtonClick()
    {
        LoadData();
    }
    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
