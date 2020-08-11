using UnityEngine;
using System.Collections;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.iOS;
#endif
public class TestLocationService : MonoBehaviour
{
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

        Application.RequestUserAuthorization(UserAuthorization.WebCam);
        yield return null;
    }
#endif


}