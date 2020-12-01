using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameNetwork;
public class UIManagerFight : MonoBehaviour
{
    [Tooltip("Set to false by defalut, allows you activate your custom object")]
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private GameObject _readyButton;
    [SerializeField] private float _fadeTime = 1f;
    [SerializeField] private GameObject _tapUIFeedbackImage;
    [SerializeField] private GameObject _planeFinder;
    //Intro Text
    [SerializeField] private GameObject _intro;
    [SerializeField] private GameObject _exitButton;
    //End Panel
    [SerializeField] private GameObject _endPanel;
    [SerializeField] private GameObject _endWonPanel;
    [SerializeField] private GameObject _endLostPanel;
    [SerializeField] private GameObject _goBackToMapButton;

    //Local User
    [SerializeField] private LocalUser _localUser;

    //Game Network
    [SerializeField] private GameNetwork _gameNetwork;
    // Start is called before the first frame update
    void Start()
    {
        if (_debugMode == false)
        {
            _intro.SetActive(true);
            _readyButton.SetActive(false);
            _exitButton.SetActive(true);
            _endLostPanel.SetActive(false);
            _endWonPanel.SetActive(false);
            _endPanel.SetActive(false);
            _tapUIFeedbackImage.SetActive(false);
            //_goBackToMapButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        MouseClickUIFeedback();
    }

    private void MouseClickUIFeedback()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(Input.mousePosition);
            _tapUIFeedbackImage.SetActive(true);
            StartCoroutine(FadeCanvasGroup(false,0.6f,_tapUIFeedbackImage.GetComponent<CanvasGroup>()));
            _tapUIFeedbackImage.transform.position = Input.mousePosition;
            StartCoroutine(FadeCanvasGroup(true, 0.6f, _tapUIFeedbackImage.GetComponent<CanvasGroup>()));
        }
    }

    private IEnumerator FadeCanvasGroup(bool fadeAway,float fadeTime , CanvasGroup canvasGroup)
    {
        if (fadeAway)
        {
            //frade opauqe to transparent
            for (float i = fadeTime; i >= 0; i -= Time.deltaTime)
            {
                canvasGroup.alpha = i;
                yield return null;
            }
        }
        //fade from transparent to opaque
        else
        {
            //loop over 1 second
            for (float i = 0; i <= fadeTime; i += Time.deltaTime)
            {
                canvasGroup.alpha = i;
                yield return null;
            }
        }
    }

    public void OnMageAugmented()
    {
        _readyButton.SetActive(true);

    }

    public void DisplayEndPanel(bool playerWon)
    {
        _endPanel.SetActive(true);
        if(playerWon == true)
        {
            _endWonPanel.SetActive(true);
        }
        else
        {
            _endPanel.SetActive(true);
            _endLostPanel.SetActive(true);
        }
    }


    public void DisplayGoBackTOMapButton()
    {
        _goBackToMapButton.SetActive(true);
    }

    public void OnGoBackToMapButtonClick()
    {
        SceneManager.LoadScene("Map");
    }


    public void OnReadyButtonClick()
    {
        _intro.SetActive(false);
        _planeFinder.SetActive(false);
        _exitButton.SetActive(false);
        FightManager.startFgiht = true;
        _readyButton.SetActive(false);
    }

    public void onResetPositionButtonClick()
    {
        _planeFinder.SetActive(true);
    }

    public void OnExitButtonClick()
    {
        FightSet fightSet = new FightSet(_localUser.currentEventId, _localUser.userID, false, _localUser.currentMutex);
        StartCoroutine(_gameNetwork.SetFightSatus("https://harryspotter-backend.portmap.io:26214/setFightStatus", fightSet.Serialize().ToString()));
        SceneManager.LoadScene("Map");
    }
}
