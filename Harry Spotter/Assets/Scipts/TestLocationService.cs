using UnityEngine;
using System.Collections;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.iOS;
#endif
public class TestLocationService : MonoBehaviour
{
    //Responsible for accessing the user location (Attach to TestLocationServicet GameObject)
    //note this sctipt is using preprocessor it is set to it will on run on mobile device and not on Unity editor

#if UNITY_ANDROID && !UNITY_EDITOR
    IEnumerator Start()
    {

        UniAndroidPermission.RequestPermission(AndroidPermission.ACCESS_FINE_LOCATION);
        yield return null;
    }
#endif

#if UNITY_IOS && !UNITY_EDITOR

    IEnumerator Start()
    {

        //Application.RequestUserAuthorization(UserAuthorization.WebCam);
        yield return null;
    }
#endif


}