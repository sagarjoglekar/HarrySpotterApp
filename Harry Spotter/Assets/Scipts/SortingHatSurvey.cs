using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SortingHatNetwork;

public class SortingHatSurvey : MonoBehaviour
{
    public GameObject[] questionGroupArr; //Array of gameobjects "group of questions" we are going to populate from the inspector 
    public QAClass[] qaArr; //array of type class QAClass

    [SerializeField] private GameObject _AnswerAllQuestionsPanel;

    //for test
    public GameObject AnswerPanel;
    [SerializeField] private GameObject _activePanel;

    private bool _allQuestionsHaveBeenAnswered = false;

    private UIManagerSurvey _uIManagerSurvey;
    private int unAnswredQcount = 0;

    [SerializeField] private bool _checkForSlider = true;
    [SerializeField] private LocalUser _localUser;
    [SerializeField] private SortingHatNetwork _sortingHatNetwork;
    private void Start()
    {
        _uIManagerSurvey = GameObject.Find("Canvas").GetComponent<UIManagerSurvey>();
        if (_uIManagerSurvey == null)
        {
            Debug.LogError("UIManagerSurvey is null");
        }

        qaArr = new QAClass[questionGroupArr.Length]; //initializing new array requires length
        //initializing qarr
    }
    private void Update()
    {
        //print("number of unanswered questions: " + unAnswredQcount);
    }

    public void OnSubmitButtonClick() //used by the okay button
    {
        unAnswredQcount = 0;
        for (int i = 0; i < qaArr.Length; i++) //for loop running for the length of qArr
        {
            qaArr[i] = ReadQuestionAndAnswer(questionGroupArr[i]);
            //print(qaArr[i].Answer);
            if (qaArr[i].Answer == "")
            {
                unAnswredQcount++;
                //Debug.Log("Please Answare all question " + i);
                _AnswerAllQuestionsPanel.SetActive(true);
                questionGroupArr[i].transform.Find("Question").gameObject.GetComponent<Text>().color = Color.red;
                _allQuestionsHaveBeenAnswered = false;
            } else
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

            int[] saveAnswares = new int [qaArr.Length];
            for (int i = 0; i < qaArr.Length; i++)
            {
                //print(qaArr[i].Answer);
                saveAnswares[i] = int.Parse(qaArr[i].Answer);
            }
            //foreach(int i in saveAnswares)
            //{
            //    print(i);
            //}

            //ec2d3ab4-c1c6-11ea-88d8-47f884e0816a

            //dea45272-bd40-11ea-b807-7bc7be4e90df
            _localUser.LoadData();
            HatQuiz myResults = new HatQuiz(_localUser.userID, saveAnswares);
            StartCoroutine(_sortingHatNetwork.PostRegistery("https://harryspotter-backend.portmap.io:26214/registerUser", myResults.Serialize().ToString(),GetHouseID));
           
            //harryPotterHouses[Random.Range(0, 4)];
            //print("You have answared all of the questions");
            
        }
        //DisplayResult();
    }

    public void GetHouseID(int houseId)
    {
        print("this the house number:" + houseId);
        _localUser.userHouse = houseId;
        _localUser.SaveData();
        // 1: Hufflepuff, 2: Ravenclaw, 3: Griffindor , 4: Slytherin
        string[] harryPotterHouses = { "Hufflepuff", "Ravenclaw", "Gryffindor", "Slytherin" };
        _uIManagerSurvey.DisplaySortingHatResult(harryPotterHouses[houseId-1]);
    }

    QAClass ReadQuestionAndAnswer(GameObject questionGroup) //whatever value is read by the QuestionAndAnswer is returned and used to populate the length of qArr. It takes in a parameter GameObject questionGroup
    {
        QAClass result = new QAClass();

        GameObject q = questionGroup.transform.Find("Question").gameObject; //We are looking for a gameobject called question within the question group object. If found set the reference in GameObject q.
        GameObject a = questionGroup.transform.Find("Answer").gameObject; //We are looking for a gameobject called answer within the question group object.If found set the reference in GameObject a.

        result.Question = q.GetComponent<Text>().text; //reading the question. All are of type text
        if (_checkForSlider == false)
        {
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
        } else
        {
            if (a.GetComponentInChildren<Slider>() !=null)
            {
                if(a.GetComponentInChildren<Slider>().value.ToString() == "0")
                {
                    result.Answer = "";
                }
                else
                {
                   result.Answer = a.GetComponentInChildren<Slider>().value.ToString();
                }
            }
        }
        return result;
    }

    [System.Serializable] //if array is created of this class it will show up in the inspector
    public class QAClass
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
