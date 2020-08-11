using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerSurvey : MonoBehaviour
{
    [Tooltip("Set to false by defalut, allows you activate your custom object")]
    [SerializeField] private bool _debugMode = false;
    [SerializeField] private float _fadeTime = 1f;
    [SerializeField] private CanvasGroup _questionsPanel;
    [SerializeField] private CanvasGroup _questionsPanel1;
    [SerializeField] private CanvasGroup _questionsPanel2;

    [SerializeField] private GameObject _panelHolder;
    private PageSwiper _pageSwiper;

    [SerializeField] private GameObject _startTheQuizePanel;
    [SerializeField] private GameObject _pageSelectorUI;
    [SerializeField] private GameObject _submitButton;
    [SerializeField] private GameObject _resultPanel;

    private bool _isRunningPage1 = false;
    private bool _isRunningPage2 = false;
    private bool _isRunningPage3 = false;


    [SerializeField] private GameObject _AnswereAllofTheQuestions;
    private bool _isAnswereAllQuestionsRunning = false;

    [SerializeField] private GameObject _grryffindorHouse;
    [SerializeField] private GameObject _hufflepuffHouse;
    [SerializeField] private GameObject _ravenclawHouse;
    [SerializeField] private GameObject _slytherinHouse;

    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        if(_debugMode == false)
        {
            _panelHolder.SetActive(false);
            _pageSelectorUI.SetActive(false);
            //_AnswereAllofTheQuestions.SetActive(false);
            _submitButton.SetActive(false);
            _resultPanel.SetActive(false);
            _startTheQuizePanel.SetActive(true);
        }
        if (_panelHolder == null)
        {
            Debug.LogError("Panel Holder Is Null.");
        }
        else
        {
            _pageSwiper = _panelHolder.GetComponent<PageSwiper>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        PanelFadeInAndOutEffect();
        _AnswereAllofTheQuestionsAnimation();
        //print(_AnswereAllofTheQuestions.activeSelf);
    }

    public void OpenUrl(string url)
    {
        Application.OpenURL(url);
    }

    private void PanelFadeInAndOutEffect()
    {
        if (_pageSwiper.currentPage == 1 && _isRunningPage1 == false)
        {
            _isRunningPage2 = false;
            print("got called page 1");
            StartCoroutine(FadeCanvasGroup((false), _questionsPanel));
            StartCoroutine(FadeCanvasGroup((true), _questionsPanel1));
            _isRunningPage1 = true;
        }
        else if (_pageSwiper.currentPage == 2 && _isRunningPage2 == false)
        {
            _isRunningPage1 = false;
            _isRunningPage3 = false;
            print("got called page 2");
            StartCoroutine(FadeCanvasGroup((true), _questionsPanel));
            StartCoroutine(FadeCanvasGroup((true), _questionsPanel2));
            StartCoroutine(FadeCanvasGroup((false), _questionsPanel1));
            _isRunningPage2 = true;
        }
        else if (_pageSwiper.currentPage == 3 && _isRunningPage3 == false)
        {
            _isRunningPage2 = false;
            print("got called page 3");
            StartCoroutine(FadeCanvasGroup((true), _questionsPanel1));
            StartCoroutine(FadeCanvasGroup((false), _questionsPanel2));
            if (_submitButton.activeSelf == false)
            {
                _submitButton.SetActive(true);
                StartCoroutine(FadeCanvasGroup(false, _submitButton.GetComponent<CanvasGroup>()));
            }
            _isRunningPage3 = true;
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

    private void _AnswereAllofTheQuestionsAnimation()
    {
        if (_AnswereAllofTheQuestions.activeSelf == true && _isAnswereAllQuestionsRunning == false)
        {
            _isAnswereAllQuestionsRunning = true;
            StartCoroutine(FadeCanvasGroup(false,_AnswereAllofTheQuestions.GetComponent<CanvasGroup>()));
        }
    }
    /*
    private IEnumerator AnswereAllQuestionEffect(CanvasGroup canvasGroup)
    {
        int count = 0;
        while (true)
        {
            //frade opauqe to transparent
            for (float i = _fadeTime; i >= 0; i -= Time.deltaTime)
            {
                canvasGroup.alpha = i;
                yield return null;
            }
            //fade from transparent to opaque
            //loop over 1 second
            for (float i = 0; i <= _fadeTime; i += Time.deltaTime)
            {
                canvasGroup.alpha = i;
                yield return null;
            }
            count++;
            if (count >= 1)
            {
                break;
            }
        }

    }
    */

    public void OnStartTheQuizeButtonClick()
    {
        StartCoroutine(FadeCanvasGroup(true, _startTheQuizePanel.GetComponent<CanvasGroup>()));
        StartCoroutine(SetPanelHolderActive());
    }

    IEnumerator SetPanelHolderActive()
    {
        yield return new WaitForSeconds(_fadeTime);
        _panelHolder.SetActive(true);
        _pageSelectorUI.SetActive(true);
        _startTheQuizePanel.SetActive(false);
    }

    public void DisplaySortingHatResult(string houseName)
    {
        StartCoroutine(FadeCanvasGroup(true, _AnswereAllofTheQuestions.GetComponent<CanvasGroup>()));
        StartCoroutine(FadeCanvasGroup(true, _panelHolder.GetComponent<CanvasGroup>()));
        StartCoroutine(FadeCanvasGroup(true, _pageSelectorUI.GetComponent<CanvasGroup>()));
        StartCoroutine(FadeCanvasGroup(true, _submitButton.GetComponent<CanvasGroup>()));
        _AnswereAllofTheQuestions.SetActive(false);
        _panelHolder.SetActive(false);
        _pageSelectorUI.SetActive(false);
        _submitButton.SetActive(false);
        _resultPanel.SetActive(true);
        StartCoroutine(FadeCanvasGroup(false, _resultPanel.GetComponent<CanvasGroup>()));
        //"Gryffindor", "Slytherin", "Ravenclaw", "Hufflepuff"
        if(houseName == "Gryffindor")
        {
            _grryffindorHouse.SetActive(true);
            StartCoroutine(FadeCanvasGroup(false, _grryffindorHouse.GetComponent<CanvasGroup>()));
        } else if (houseName == "Slytherin")
        {
            _slytherinHouse.SetActive(true);
            StartCoroutine(FadeCanvasGroup(false, _slytherinHouse.GetComponent<CanvasGroup>()));
        } else if (houseName == "Ravenclaw")
        {
            _ravenclawHouse.SetActive(true);
            StartCoroutine(FadeCanvasGroup(false, _ravenclawHouse.GetComponent<CanvasGroup>()));
        } else if (houseName == "Hufflepuff")
        {
            _hufflepuffHouse.SetActive(true);
            StartCoroutine(FadeCanvasGroup(false, _hufflepuffHouse.GetComponent<CanvasGroup>()));
        }
    }


    public void OnNextButtonClick()
    {
        StartCoroutine(LoadLevel("Map"));
    }


    IEnumerator LoadLevel(string levelName)
    {
        //Play Animation
        _transition.SetTrigger("Start");
        //Wait
        yield return new WaitForSeconds(_transitionTime);
        //Load Scene
        SceneManager.LoadScene(levelName);
    }



}

/*
if (fadeAway)
{
    for (float i = _fadeTime; i >= 0; i -= Time.deltaTime)
    {
        foreach (var text in texts)
        {
            //set colour with i as aplha
            text.color = new Color(text.color.r, text.color.g, text.color.b, i);
        }
        yield return null;
    }
}
//fade from transparent to opaque
else
{
    //loop over 1 second
    for (float i = 0; i <= _fadeTime; i += Time.deltaTime)
    {
        foreach (var text in texts)
        {
            //set color with i as alpha
            text.color = new Color(text.color.r, text.color.g, text.color.b, i);
        }
        yield return null;
    }
}
}
*/

