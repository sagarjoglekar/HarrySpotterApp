
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RunTimeChecker : MonoBehaviour
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod()
    {
        
        GameObject gameObject = new GameObject();
        LocalUser localUser = gameObject.AddComponent<LocalUser>();
        localUser.LoadData();
        if (localUser.userID == null || localUser.userID == "")
        {
            //SceneManager.LoadScene("LogIn");
            SceneManager.LoadSceneAsync("LogIn");
        } else
        {
            if (localUser.userID != "")
            {
                if (localUser.userHouse == 0)
                {
                    //SceneManager.LoadScene("Survey");
                    SceneManager.LoadSceneAsync("Survey");
                }
                else
                {
                    //SceneManager.LoadScene("Map");
                    SceneManager.LoadSceneAsync("Map");
                }
            }
        }
        print(localUser.userID);
       
        Debug.Log("Before first Scene loaded");
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnAfterSceneLoadRuntimeMethod()
    {
        Debug.Log("After first Scene loaded");
    }

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Debug.Log("RuntimeMethodLoad: After first Scene loaded");
    }
}


