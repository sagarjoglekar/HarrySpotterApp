using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameNetwork;
public class MaslowQuestion : MonoBehaviour
{
    public GameObject[] questionGroupArr; //Array of gameobjects "group of questions" we are going to populate from the inspector 
    public MaslowQAClass[] qaArr; //array of type class MaslowQAClass

    [SerializeField] private GameObject _AnswerAllQuestionsPanel;

    //for test
    public GameObject AnswerPanel;
    [SerializeField] private GameObject _activePanel;

    private bool _allQuestionsHaveBeenAnswered = false;

    private UIManagerMap _uIManagerMap;
    private int unAnswredQcount = 0;

    private string tempAnsware;
    [SerializeField] TakePhoto _takePhoto;
    [SerializeField] InputField _labelInputField;
    [SerializeField] LocalUser _localUser;
    [SerializeField] GameNetwork _gameNetwork;
    private void Start()
    {
        _uIManagerMap = GameObject.Find("Canvas").GetComponent<UIManagerMap>();
        if (_uIManagerMap == null)
        {
            Debug.LogError("_uIManagerMap is null");
        }

        qaArr = new MaslowQAClass[questionGroupArr.Length]; //initializing new array requires length
        //initializing qarr
    }
    private void Update()
    {
        //print("number of unanswered questions: " + unAnswredQcount);
    }

    public void OnNextButtonClick() //used by the okay button
    {
        unAnswredQcount = 0;
        for (int i = 0; i < qaArr.Length; i++) //for loop running for the length of qArr
        {
            qaArr[i] = ReadQuestionAndAnswer(questionGroupArr[i]);
            if (qaArr[i].Answer == "")
            {
                unAnswredQcount++;
                Debug.Log("Please Answare all question " + i);
                //_AnswerAllQuestionsPanel.SetActive(true);
                questionGroupArr[i].transform.Find("Question").gameObject.GetComponent<Text>().color = Color.red;
                //_allQuestionsHaveBeenAnswered = false;
            }
            else
            {
                questionGroupArr[i].transform.Find("Question").gameObject.GetComponent<Text>().color = Color.white;
            }
        }
        if (unAnswredQcount == 0)
        {
            _allQuestionsHaveBeenAnswered = true;
        }

        if (_allQuestionsHaveBeenAnswered == true)
        {
            tempAnsware = "";

            for (int i = 0; i < qaArr.Length; i++)
            {
                tempAnsware = tempAnsware + qaArr[i].Answer;
                if (tempAnsware == "Self-actualization")
                {
                    tempAnsware = "L5";
                }
                else if (tempAnsware == "Esteem")
                {
                    tempAnsware = "L4";
                }
                else if (tempAnsware == "Love/belonging")
                {
                    tempAnsware = "L3";
                }
                else if (tempAnsware == "Safety")
                {
                    tempAnsware = "L2";
                }
                else if (tempAnsware == "Physiological")
                {
                    tempAnsware = "L1";
                }
            }
            print(tempAnsware);
            AnnotationDetail annotation = new AnnotationDetail(_localUser.userID, _takePhoto.tempObjectIds, _labelInputField.text, tempAnsware);
            StartCoroutine(_gameNetwork.PostAnnotateObject("https://harryspotter.eu.ngrok.io/annotate", annotation.ToJsonString(),GettingUserScoreBack));
        }
    }

    public void GettingUserScoreBack(float score, string userId)
    {
        print("User Score is: " + score);
        print("User Id is: " + userId);
        if(userId == _localUser.userID)
        {
            _localUser.score = score;
            _localUser.SaveData();
            _uIManagerMap.DisplayCongratulationPanel(_localUser.score);
        }
    }
    

    

    MaslowQAClass ReadQuestionAndAnswer(GameObject questionGroup){ //whatever value is read by the QuestionAndAnswer is returned and used to populate the length of qArr. It takes in a parameter GameObject questionGroup

    MaslowQAClass result = new MaslowQAClass();

    GameObject q = questionGroup.transform.Find("Question").gameObject; //We are looking for a gameobject called question within the question group object. If found set the reference in GameObject q.
    GameObject a = questionGroup.transform.Find("Answer").gameObject; //We are looking for a gameobject called answer within the question group object.If found set the reference in GameObject a.

    result.Question = q.GetComponent<Text>().text; //reading the question. All are of type text

    if (a.GetComponent<ToggleGroup>() != null) //Check Box, Toggle Group (Only one answared is allowd)
    {
        for (int i = 0; i < a.transform.childCount; i++)
        {
            if (a.transform.GetChild(i).GetComponent<Toggle>().isOn)
            {
                result.Answer = a.transform.GetChild(i).Find("Label").GetComponent<Text>().text;
                break;
            }
        }
    }
    else if (a.GetComponent<InputField>() != null) //For Input Feild Survey
    {


        result.Answer = a.transform.Find("Text").GetComponent<Text>().text;

    }
    else if (a.GetComponent<ToggleGroup>() == null && a.GetComponent<InputField>() == null) //check box (allows for multiple toggle)
    {
        string s = "";
        int counter = 0;
        for (int i = 0; i < a.transform.childCount; i++)
        {
            if (a.transform.GetChild(i).GetComponent<Toggle>().isOn)
            {
                if (counter != 0)
                {
                    s = s + " , ";
                }
                s = s + a.transform.GetChild(i).Find("Label").GetComponent<Text>().text; //extending the string not overwriting it
                counter++;
            }
            if (i == a.transform.childCount - 1)
            {
                s = s + " . ";
            }
        }
        result.Answer = s;
    }
    return result;
}

[System.Serializable] //if array is created of this class it will show up in the inspector
public class MaslowQAClass
{
    public string Question = "";
    public string Answer = "";
}


void DisplayResult()
{
    _activePanel.SetActive(false);
    AnswerPanel.SetActive(true);

    string s = "";

    for (int i = 0; i < qaArr.Length; i++)
    {
        s = s + qaArr[i].Question + "\n";
        s = s + qaArr[i].Answer + "\n\n";
    }
    string[] harryPotterHouses = { "Gryffindor", "Slytherin", "Ravenclaw", "Hufflepuff" };
    AnswerPanel.transform.Find("Answer").GetComponent<Text>().text = s + "\n" + "You are in " + harryPotterHouses[Random.Range(0, 4)] + " House";

}
}
