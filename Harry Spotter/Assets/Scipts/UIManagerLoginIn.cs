using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static LoginNetwork;

public class UIManagerLoginIn : MonoBehaviour
{
    /*
    Responsible for all the login UI functionalities (Attach to Canvas GameObject)
    */
    [SerializeField] private bool _debug = false;
    [SerializeField] private float _fadeTime = 1f;
    [SerializeField] private CanvasGroup _welcomPanle;
    [SerializeField] private CanvasGroup _sorttingHatPanel;
    [SerializeField] private GameObject _signUpPanel;
    [SerializeField] private Text _signUpWelcomInformationText;
    [SerializeField] private InputField _signUpEmailInputText;
    [SerializeField] private InputField _signUpPassInputText;

    //Login Panel
    [SerializeField] private GameObject _logInPanel;
    [SerializeField] private Text _loginWelcomInformationText;
    [SerializeField] private InputField _loginEmailInputText;
    [SerializeField] private InputField _loginPassInputText;

    //Page Selector UI
    [SerializeField] private GameObject _pageSelectorUI;
    [SerializeField] private GameObject _panelHolder;

    //Private UI animation;
    private bool _isAnimationOn = false;

    private PageSwiper _pageSwiper;

    private bool _isRunningPage1 = false;
    private bool _isRunningPage2 = false;


    [SerializeField] private Animator _transition;
    [SerializeField] private float _transitionTime = 1f;

    [SerializeField] private LocalUser _localUser;
    [SerializeField] private LoginNetwork _loginNetwork;


    private void Awake()
    {
        print("This is void awake (UIManagerLoginIN");
    }
    // Start is called before the first frame update
    void Start()
    {
        //for smooth animation
        Application.targetFrameRate = 60;
        //setting up the application
        if (_debug == false)
        {
            _localUser.LoadData();
            if (_localUser.userID == null || _localUser.userID == "")
            {
                _panelHolder.SetActive(true);
                _pageSwiper = _panelHolder.GetComponent<PageSwiper>();
                _signUpPanel.SetActive(false);
                _logInPanel.SetActive(false);
                _pageSelectorUI.SetActive(true);
            }
        }
        /*
        _panelHolder = GameObject.Find("Panel Holder");
        if (_panelHolder == null)
        {
            Debug.LogError("Panel Holder Is Null.");
        }
        else
        {
            _pageSwiper = _panelHolder.GetComponent<PageSwiper>();
        }
        */
        
    }

    // Update is called once per frame
    void Update()
    {
        PagesTextFadeInAndOutEffect();
        if(_signUpPanel.activeSelf == true)
        {
            _signUpEmailInputText.characterValidation = InputField.CharacterValidation.EmailAddress;
        }
        if(_logInPanel.activeSelf == true)
        {
            _loginEmailInputText.characterValidation = InputField.CharacterValidation.EmailAddress;
        }
    }
   

    private void PagesTextFadeInAndOutEffect()
    {
        if (_pageSwiper.currentPage == 1 && _isRunningPage1 == false)
        {
            _isRunningPage2 = false;
            print("got called page 1");
            StartCoroutine(FadeCanvasGroup((false), _welcomPanle));
            StartCoroutine(FadeCanvasGroup((true), _sorttingHatPanel));
            _isRunningPage1 = true;
        }
        else if (_pageSwiper.currentPage == 2 && _isRunningPage2 == false)
        {
            _isRunningPage1 = false;
            print("got called page 2");
            StartCoroutine(FadeCanvasGroup((true), _welcomPanle));
            StartCoroutine(FadeCanvasGroup((false), _sorttingHatPanel));
            _isRunningPage2 = true;
        }
    }

    //fade effect when swiping through pages
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


    #region SignUp Dynamic Animation
    //Sign UP responsive animation
    public void OnSignUpEmailPointerDown()
    {
        _signUpEmailInputText.transform.GetChild(1).GetComponent<Text>().text = "";
        if (_isAnimationOn == false)
        {
            _transition.SetBool("SignUpPassSelected", true);
        }
        _isAnimationOn = true;
    }

    public void OnSignUpEmailDeselect()
    {
        if (_signUpEmailInputText.transform.GetChild(2).GetComponent<Text>().text == "")
        {
           _signUpEmailInputText.transform.GetChild(1).GetComponent<Text>().text = "Email";
        }
        _transition.SetBool("SignUpPassSelected", false);
        _isAnimationOn = false;
    }


    public void OnSignUpPassPointerDown()
    {
        _signUpPassInputText.transform.GetChild(1).GetComponent<Text>().text = "";
        if (_isAnimationOn == false)
        {
            _transition.SetBool("SignUpPassSelected", true);
        }
        _isAnimationOn = true;
    }

    public void OnSignUpPassDeselected()
    {
        if (_signUpPassInputText.transform.GetChild(2).GetComponent<Text>().text == "")
        {
            _signUpPassInputText.transform.GetChild(1).GetComponent<Text>().text = "Password";
        }
        _transition.SetBool("SignUpPassSelected", false);
        _isAnimationOn = false;
    }
    #endregion

    //Gets called on -> Panel Holder->Sign Up (gameobject)
    public void OnSignUpButtonClick()
    {
        _panelHolder.SetActive(false);
        _signUpPanel.SetActive(true);
        _pageSelectorUI.SetActive(false);
        StartCoroutine(FadeCanvasGroup(false, _signUpPanel.GetComponent<CanvasGroup>()));
    }
    //Attach to Panel Holder -> Log In
    public void OnLogInButtonClick()
    {
        _panelHolder.SetActive(false);
        _logInPanel.SetActive(true);
        _pageSelectorUI.SetActive(false);
        StartCoroutine(FadeCanvasGroup(false, _logInPanel.GetComponent<CanvasGroup>()));
    }

    //Gets called on 
    // Sing Up Panel -> Back Button (gameobject)
    // Login Panel -> Back Button (gameobject)
    public void OnBackButtonClick()
    {
        if(_signUpPanel.activeSelf == true)
        {
            _signUpPanel.SetActive(false);
            _panelHolder.SetActive(true);
            StartCoroutine(FadeCanvasGroup(false, _panelHolder.GetComponent<CanvasGroup>()));
            _pageSelectorUI.SetActive(true);
        } else
        {
            _logInPanel.SetActive(false);
            _panelHolder.SetActive(true);
            StartCoroutine(FadeCanvasGroup(false, _panelHolder.GetComponent<CanvasGroup>()));
            _pageSelectorUI.SetActive(true);
        }
    }
    
    //Gets called on Sign Up Panel -> Submmit Button/Sign Up
    public void OnSignUpSubmmitButton()
    {
        if (_signUpEmailInputText.text == "" || _signUpPassInputText.text == "")
        {
            _signUpEmailInputText.GetComponentInChildren<Text>().color = Color.red;
            _signUpPassInputText.GetComponentInChildren<Text>().color = Color.red;
        }
        else
        {
            if(_signUpEmailInputText.text.Length > 4 && _signUpPassInputText.text.Length >= 6)
            {
                UserAccount newUser = new UserAccount(_signUpEmailInputText.text, _signUpPassInputText.text);
                //original stub:http://sjoglekar-25233.portmap.io:25233/
                //https stub:https://sjoglekar-45523.portmap.io:45523/
                StartCoroutine(_loginNetwork.PostUserAccount("https://sjoglekar-45523.portmap.io:45523/signup", newUser.ToJsonString(), SignUpGetUserID));
            }
            if(_signUpEmailInputText.text.Length <= 4)
            {
                _signUpWelcomInformationText.text = "Please enter a valid email address";
            }
            if(_signUpPassInputText.text.Length < 6)
            {
                _signUpWelcomInformationText.text = "Password needs to be at least 6 characters";
            }
            if(_signUpPassInputText.text.Length < 6 && _signUpEmailInputText.text.Length < 4)
            {
                _signUpWelcomInformationText.text = "Please enter a valid email address \n Password needs to be at least 6 characters";
            }
        }
    }

    #region Login Dynamic Animation
    //Log In responsive animation
    public void OnLoginEmailPointerDown()
    {
        Debug.Log("OnLoginEmailSelected()");
        _loginEmailInputText.transform.GetChild(1).GetComponent<Text>().text = "";
        if(_isAnimationOn == false)
        {
            _transition.SetBool("LoginPassSelected", true);
        }
        _isAnimationOn = true;
    }

    public void OnLoginEmailDeselect()
    {
        Debug.Log("OnLoginEmailDeselect()");
        if (_loginEmailInputText.transform.GetChild(2).GetComponent<Text>().text == "")
        {
            _loginEmailInputText.transform.GetChild(1).GetComponent<Text>().text = "Email";
        }
            _transition.SetBool("LoginPassSelected", false);
            _isAnimationOn = false;
    }


    public void OnLoginPassPointerDown()
    {
        print("OnLoginPassSelected()");
        _loginPassInputText.transform.GetChild(1).GetComponent<Text>().text = "";
        if (_isAnimationOn == false)
        {
            _transition.SetBool("LoginPassSelected", true);
        }
        _isAnimationOn = true;
    }

    public void OnLoginPassDeselected()
    {
        if(_loginPassInputText.transform.GetChild(1).GetComponent<Text>().text == "")
        {
            _loginPassInputText.transform.GetChild(1).GetComponent<Text>().text = "Password";
        }
           _transition.SetBool("LoginPassSelected", false);
           _isAnimationOn = false;
    }
    #endregion

    //Gets called on
    // Canvas -> LogIn Panel -> Submmit Button
    public void OnLogInSubmmitButton()
    {
       // UserAccount newUser = new UserAccount(_loginEmailInputText.text, _loginPassInputText.text);
        //StartCoroutine(_loginNetwork.Post("http://sjoglekar-25233.portmap.io:25233/signin", newUser.ToJsonString(), LoginGetUserID));
        if (_loginEmailInputText.text == "" || _loginPassInputText.text == "")
        {
            _loginEmailInputText.GetComponentInChildren<Text>().color = Color.red;
            _loginPassInputText.GetComponentInChildren<Text>().color = Color.red;
        }
        else
        {
            //print(_loginEmailInputText.text.ToString() + _loginPassInputText.text.ToString());
            //UserAccount newUser = new UserAccount("nima@garbage.com", "secret123");
            if (_loginEmailInputText.text.Length > 4 && _loginPassInputText.text.Length >= 6)
            {
                UserAccount newUser = new UserAccount(_loginEmailInputText.text, _loginPassInputText.text);
                StartCoroutine(_loginNetwork.PostUserAccount("https://sjoglekar-45523.portmap.io:45523/signin", newUser.ToJsonString(), LoginGetUserID));
            }
            if (_loginEmailInputText.text.Length <= 4)
            {
                _loginWelcomInformationText.text = "Please enter a valid email address";
            }
            if (_loginPassInputText.text.Length < 6)
            {
                _loginWelcomInformationText.text = "Password needs to be at least 6 characters";
            }
            if (_loginPassInputText.text.Length < 6 && _loginEmailInputText.text.Length < 4)
            {
                _loginWelcomInformationText.text = "Please enter a valid email address \n Password needs to be at least 6 characters";
            }
            print("exction order");
        }
    }


    #region CallBackMethodsToGetUserIDandAccrountInfo
    public void LoginGetUserID(string userID)
    {
        print(userID);
        if (userID == "")
        {
            _loginWelcomInformationText.text = "Email or password is incorrect";
            Debug.Log("User Not found");
        } else
        {
            //_localUser.LoadData();
            _localUser.userID = userID;
            _localUser.SaveData();
            UserId _userId = new UserId(_localUser.userID);
            StartCoroutine(_loginNetwork.PostUserInfo("https://sjoglekar-45523.portmap.io:45523/getUser", _userId.Serialize().ToString(),GetUserAccountDetail));
        }
    }

    public void SignUpGetUserID(string userID)
    {
        print("UserID is" + userID);
        _localUser.userID = userID;
        _localUser.SaveData();
        if (userID != "")
        {
            // StartCoroutine(LoadLevel("Survey"));
            SceneManager.LoadScene("Survey");
        } else
        {
            _signUpWelcomInformationText.text = "Account already exists use the login option";
           //UserId _userId = new UserId(_localUser.userID);
           //StartCoroutine(_loginNetwork.PostUserInfo("http://sjoglekar-25233.portmap.io:25233/getUser", _userId.Serialize().ToString(), GetUserAccountDetail));
        }
    }


    public void GetUserAccountDetail(UserAccountDetailCallBack userAccountDetailCallBack)
    {
        print("Defender Score is: " + userAccountDetailCallBack.defenderScore);
        print("User House Number is: " + userAccountDetailCallBack.house);
        if (userAccountDetailCallBack.mayourships != null)
        {
            foreach (var i in userAccountDetailCallBack.mayourships)
            {
                print("User Mayourships: " + i.ToString());
            }
        }
        print("User Score is: " + userAccountDetailCallBack.score);
        print("UserID is: " + userAccountDetailCallBack.userId);
        if (userAccountDetailCallBack.house == 0)
        {
            //StartCoroutine(LoadLevel("Survey"));
            SceneManager.LoadScene("Survey");
        }
        else
        {
            _localUser.userHouse = userAccountDetailCallBack.house;
            _localUser.score = userAccountDetailCallBack.score;
            _localUser.SaveData();
            //StartCoroutine(LoadLevel("Map"));
            SceneManager.LoadScene("Map");
        }
    }
    #endregion

    public void GoToSurveyButtonClick()
    {

        StartCoroutine(LoadLevel("Survey"));
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
