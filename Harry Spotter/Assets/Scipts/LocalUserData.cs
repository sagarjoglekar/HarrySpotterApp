using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocalUserData
{
    public string userId;
    // 1: Hufflepuff, 2: Ravenclaw, 3: Griffindor , 4: Slytherin
    public int userHouse;
    public float score;

    public string currentEventId;
    public int currentDefenderScore;
    public int currentUserDendenderScore;
    public int currentEventHouse;
    public string currentMutex;
    public LocalUserData(LocalUser localUser)
    {
        userId = localUser.userID;
        userHouse = localUser.userHouse;
        score = localUser.score;
        currentEventId = localUser.currentEventId;
        currentDefenderScore = localUser.currentDefenderScore;
        currentUserDendenderScore = localUser.currentUserDefenderScore;
        currentEventHouse = localUser.currentEventHouse;
        currentMutex = localUser.currentMutex;
    }
}
