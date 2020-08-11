using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameNetwork;
using System;
using Mapbox.Examples;
public class TakePhoto : MonoBehaviour
{
    [SerializeField] private RawImage photoTaken;
    [SerializeField] private Text labelInputText;
    [SerializeField] private Text _placeHolder;
    [SerializeField] private UIManagerMap _uIManagerMap;
    [SerializeField] private GameNetwork _gameNetwork;
    [SerializeField] private Texture2D _testTexture;
    [SerializeField] private LocalUser _localUser;
    [SerializeField] private LocationStatus _locationStatus;
    [SerializeField] bool _PcdebugMode;
    // Create a Texture2D from the captured image
    Texture2D texture;
    public List<string> tempObjectIds;
    private byte[] bytes;
    private string image64;
    private string test1 = "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBxISEhUQEBIVFhUVFRUVFhUVFxUVFRUVFRUWFhUVFhYYHSggGBolGxUVITEhJSkrLi4uFx81ODMtNygtLi0BCgoKDQ0NDw0NDysZFRkrNys3LSsrNysrKysrKysrKystKzctLSsrKysrKystKysrKy0rKysrKysrKy0rKysrK//AABEIANQA7QMBIgACEQEDEQH/xAAcAAABBQEBAQAAAAAAAAAAAAAAAgQFBgcDAQj/xABNEAABAwEEBAsCCggEBQUAAAABAAIDEQQFEiEGEzFxByIyQVFhcoGRobFSwSNCQ2JzgpKys9EUJCUzNGOiwjXS4fAVU4Oj8RdGhJPD/8QAFgEBAQEAAAAAAAAAAAAAAAAAAAEC/8QAFhEBAQEAAAAAAAAAAAAAAAAAABEB/9oADAMBAAIRAxEAPwDcUIQgEKuX7phZ7OSwHWSey3YD1u/33Km3hpjaZssWrb7LMvE7UGl2u8YouW8A9G13gM1GS6Rt+TYT1uOEe8+QWcRXiBmTn0n3pnemm0UBLNr/AGRmRXYT/qQdh51RphveV3OG9ke91Ua952vf4keQyWFXjp5an1EZDB1GpHuPeCq9ZdKra1+sbOa1qQWswnqIDRluog+ljXpUNelttcZrBZdZ1tkYw+JcD5Kg3LpxLK3l4XgZsJqN7SCKhTEemUw5QB8vcVUcb302vmHkWOYdeCSYfaaCFH3Twz2mN5ZbmU6CWgFvaYGg03Z9Sslm009pnh+ZPuUgNJ4HikjajoIDh34hRRTqxcIAe0Paxr2nYWHLxqVJQ6bRHlxvbu435KDs3/DiSWRQsLtuBgaT1nV7U6/4fZXbDTc/3OqgsMGlNld8fDvH+WqfR3pA7ZKzvIafAqnnR+P4rz3gO9KJvJo6drXNr3hINCY8HMEEdWaUszN02hhq2p3OB9Slstlsj+PKN+Kn5JBpKFn0WlFqbtIdvaPcE8g01kH7yFp7JLfWqQXVCrEOmkJ5THg9VCO8mifw6TWV3ylD0FrvUCnmoJhCbQXhC/JkrHHoDgT4JygEIQgEIQgEIQgFT9L77fTUQHNxwVBoXuzqK/FYACSdpoe+yXraMEZINCcq9HOT4V76Km3fCJHOmOwfBs6ABQvI3mg/6fWrggG3NQZmp5zsru6B1Lm+7FcTZgkOsaqKW+7ym8tgrygDvFfVXd1h6lwku4dCCgTXDC7bCzPoGH7tFGTaG2Y8lr2dl5P36rSZLsCbSXWoM5bomG5xyuB5qgHzBC6m7bQ3YWO7y0+FCrzJdabSXcehBTWa5vKifvaWvHka+SWbyA5WJvba5nmQrQ+xkLi6zlFQcFva7Y4HcQU9itxGwkbiQlWm7mO5cbTvaCfGiZvutg5Ic3sucPKtPJBJxXtI3MPPl6lP4NJ5x8au+p9TRVd9hcOTK76wa70APmkGKYbCx32m/mgvcGlr+cA7/wAhRSEGlTTtb31I8gCs1ZaJRtiP1XNI8yD5KQs73uoMmDpcQabmtOZ6iRvVGjMvqCTaK7w0jzPuXdllglbiaxpB5xVvpRU6x2ezDOQa0/zaFlduUfJ2jIkEjpU/HfLaUGxEPX3HEfaG4j3gr2O5YRtaXdo/lRcGXqDsTploJGLY32nGg7uc9wIQOWQsaKNY0DqAC9BpyatPzSWnyzTIWku/dtL6fGPEYO+vv7k1lnGySb/pwj+7/RBNm3SMz1xaPnFp+8C5dIL8kOykg+ZG/wC9WnkoCOZw/c2U19p4L3eLl0kNtdta8Do2KC1wXk48qF43Fp8iU+ina7ZtG0EEEbwc1QGyTsPGxDfVTl1Xg4kBxrTYecbvySKs6EljqiqUoK5pnacELj1AfaNT90eKirCMEbG84aK7zm7zJTnTjNrWe3M0d1Gg+pTIvVQ+ZKurJFHNeuzZFQ/BCVhCZtkXUSoO+pCQbMENlSxIg4PsQXB9gCkMaViCCFku7qTWW7OpWPJIcwIKpLdfUmU11K6OhC4vswQUWW7Cmkl3kK+yWMJrJd46EFDksbuhNnwkK9S3Ymc119SCnGRwTmySSOcGtBJJoANpP+/CldimpLo6l0mgFliBA+FlGXS1hzDeonInuHxUHeyOEZDMpZef/lR9Z9ree4c5f/pFc3HWu6XZRjst2u71BwZCnefnHrTmOSpQTUTDKRrXEjmaOK0dwVgu2yRsHFYB3Z+KgbtCs1jCB8wLoE3tNoZFG6WR2FjGue5x2Na0FzidwBXK57yitMLLRCSWPFRUFpyJBBacwahA6tNlbI2hCrUUWCQt6CrUHAAk5AZknYFAXtTWB7SCDlUdIQTdhfl3en/nyTtR13O2f76vepFZ1VQ0sNXQj57z4Fx9yi8Sf6Vv40R6nO+1X81C61aQ9a9dGvTFsq6tkQPQ9dGvTESJYkQPmyJYlTISJQkQPRKliVMRKvdagf61BlTHWo1iB9rF5rEy1qNagdl6Q5wTYyrwyoO7qLm9gK5GVAkQdhY2uLWkZE1d2G5uB6jxWntqrX64zWsA50qVcbKahx6g0d+bv7PBQFnsVbVMfYY0eOaCEtRoaLtZAuN4j4QpzYhsQWG7W7FZLIMlWLutAx6vOuEOrzHMgivSMj3jrVhbamMHHeG7zn4bUCtILLrbJaIT8pBMz7Ubh71E8GVo1l3Wdx2mKOu8RsYT4sKReGn9gicI3yFxJwnAK0rka1IooPgPtZNjNndyoSWeD31HdUKDR3AkEDbQ05s+bPmUPf7KBhNK4iTTrNfyHcnN637ZrKK2iZjOfCTV53MbVx8FWL001sM4a2OfMH4zJGCm9zQPNUWe6X7FMquXBaWvFWOa4UObSHDZ1Kxqaqh6XPpqvox7lXRMpnTd9DH2APuqqiZVEo2ZdGzKKEy6NmQSwmS2zqJE6WJ0EsJ10EyiROlidBKCZK1yixOla9BJ65GuUbr0a9BJa5eGZR2vRrkD/XIMyjzMkmZBIa5DZlHa5etlQW65GYmjrcT4HD/am91sDpLW/wCeGj6oon+jI4jHfMDvtcb3pjoznBaH9Mjigpd5H4U706shTO2msh3ruJcDS/LIE55AnmFes0QV3SHSaaOVzIaNo+odz1a0M29BAI+uVVbxv20y8uZxBzwg4RnzGnvWlMumCd7TqmzBrWNDmhzsYbzmSrYwa153GnMnlt4N4pm1i1dncXHJoMjcJAyxGhrWp6ONTmqisVZbnDY1vnXxqpS67/tMDXfo8jozM95koSGuJNaNpm3ZzH0Wjf8Aow47ba0boCf/ANAqxcegs1qktdliljrY55IxjDmYw17gHYm4sINK0oaVUFekvV+ZkZU7SQcyekg5+a9hvaKueId35KfvPg9vOHPUGQNFQY8Mo6wAw6w/YTB+i14HDO6wuDTxeNhhJIyzjlwv76UQObDeoBDo8QPMRxSO+oIWr6EaYl8Tm2ouLmkYXmhLga5GnOKbTmarNLi0Jt0rx8AIx0ukjp/S4nyWv6LaHR2eMiYiR7qE0qGtpXJvOdu3dkgrun7qavsj0Cp4kVs4RMtX2R6BUkPVQ8EqUJUyxpQegfCVLEqYCRKEiB+JUsTJgJF6JEEgJkoTKPEiUJED/XL3XphrUoSIHuuRrky1iMaB5rka1MtYjWIHmtQJkz1iS+TI7j6INTuLi2evswjyYmGi/wDAyHpc5PWHDZZj0Ru+7RMtHP8ADyeku9UFJtPLO9PLO0HIpnaOWU8syCwXeVYrGq3YCrHYygkWrOtADS974Z/Na77TiT6rRGlZ1oblf17DpER/pYfeoNHcVA6T8hvaU24qE0n/AHY7XuKo5XEeMN4VsbsVQuI5jeFb27FNVmPCTsZub6BULEr9wlDis+r6BZ8qjpiXocuVUVQdg5KDlxBSgUHYOSsa4VSgUHYOSg5cQUoFB1xJQcuIK9BQdsSMSRVCBeJGJIC9QKxIGeXUfQpKVCOMB1geOSDUrYaWKc/MP5Jro/8A4d3u9Su14H9Rm7PvCbXIf2eO/wB6CmWjlnenlmKZT8sp5Z0E/d5VjsZVbu/mVisaCSaVnOjJppDeY6WQn/tRH3rQws6uQU0jt/XDCf8AsQ/koNGUNpR+6HaHvUyoXSs/BDtD0KobXEcx3K4t2BUq4zmFdGbBuCmqzPhH5A+r7lny0ThEbxdxb7ln+FVHNC6YF7gQICUErCvQ1AkBegJeBehqBICVRKDUrCgTRKASg1ehqBNF7RLDV7hQIAXoSw1e4UHNLg5bO2z74XuFLszeOz6SP8RqDSLx/gZuwfUJrcn+HDefUp1b/wCCm7DkxuJ37OHad6lBT7RyyndmKZ2jllOrOgsN3nYrHY1WbuOxWSxnJBItWe2DLSS0j2rLGfCOMe5aCxZ7AaaTyDpsTT5N/JQaKoLS0/BDtD0Knaqv6XH4IdsehVDO5DmFeGbBuVEuU5hXtmwKarPdPo6sP1fcqFqlpOmsdWHcw+io2pVRH6pe6pP9QlCFBHiJKESf6he6hAwES91Sf6he6lAwESUIk91K91KBkI0oRp5qV6IUDQRo1ae6lGpQMxGjAnmqXuqQM8CVAzjs+kj/ABGpzql7HHxmfSRfiNQXm1fwc30b/RRlwn9n/Wd6lSUx/VJh/Kf90qL0dP7PPbd6oKjaDxynVmKZ2jllOrOUFgu5WSxlVq7jsVksSCTYs7/9z/8AwR6H8lobFFsuCMW194Ynax0LYcNeIA0uOKnOaOooJYlV7S4/BDtj0KnyVXNL3fBN7Y9CqGlynMbwr9HsCz243ZjeFoMXJG4Kaqn6WZgjqPkSPcqkIl04S79ns1rcwYXMc1rmtcNgLaEggg7Q7aqxZdNG/KwuHWwh3fR1KeKCyalKEKj7LpPZH/LBh/mAs/qcMJ7ipuEtcMTSHDpaQR4hUNhCvRCngYlCNAyEKj4pDrKc3RnUZV3ZDPIe8qwatR77uOOophqDsGQzr11zp15dCBkyY61zHbAQBxXCmIR0q7Y6pfTKlE6icx1Q1zSRSoBFRXMVHNVcrZk92JhIE1A5hAd/DscGjMEnG1hBNRkK0oE3fHHU8YswOZlI2rWlowuJOTcY41ak5NJzFCAkdVzeSVqU2sl2yNwOrrBWuIPDqtLoqOzApUBziBXeaqa1SCO1KDCpLVJJiREdqUalSGqXhiQR5iSCyhb9JH+I1SJjXGWPk/SRfiMQWGtbPKOmN/3SonRk/s94+eVLwj4N46WO9CobRg/qMg+efQIKrauWd6bT3xHEcJq51K4W5nqqTk3vIXt8z6tr3jaNm8mjfMhVPAAwucak1NTtJO070VYZdMZqhsQjjqaAu47s8qk8loG5yaQaQEzETWmUvbjGIOdk9oIAbho1nGA2UVPtEz6hwJByo4VBBGdQRsIPuTaMuBqNqlH0voXfDrTZw6TN7DgcaUxZAtd3g+IKniV85aLWySMiRrna2vFo6lAKV+rmK5HaMlvVw3ibRZ45iKOcCHAbMTSWmnVUV71USBKr+mP7lv0g+65T7SoDTg/As+kH3XII3R51XtHWFosPJG4eiy+5LW1kjATm5zWtHOSSBl1Z7VqTRQU6FNVm3DXchkgZa2DOLiyfRuPFd3O+/wBSw4vX1zJGHAtcAWkEEEVBByIIO0LG+GHQ6x2WBlqssIiJkDH4C4R0LTh4lcLMxTIDaFBlOsXSCcsOJji09LSWnxGaawNfI8RwtdI9xyZGC953NbmVaby4O71gGJ1kc9tK1hLZSMthY0467gUHKx6W2uP5XGOiQB3nk4+KnrFp/wD86Dvjd/Y7/Ms+lJY4seCx42seC1w3tdmF6JVaNesel1kk+UwHokBbT62bfNTcFoY8YmOa4dLSHDxCwoTLrBaS04mOLT7TSWnxGaUbo6NrhRwBHQQCMxQ7eokd6avumI7MTaZjCTTFxuOQagu4zsztrnXJZfYdL7XHsmxjokAeO88r+pT9i4RDsmg+tG7+x3+ZBcLHdurkMmIHE12LigOLnPx7eYCpyHmc1JAKuWLTKxybZcB6JAWU+tyfNTkFqa8YmODh0tIcPEKocURhSQ9ehyK8wrzAlVQiORYuEzeT9JF+IxO1wn5u3F+IxBLQHiu7J9FBaMn9UlHzz6BTUZyO4qA0Yd+qzdr3IKbpQ74N/aZ99qq75eLh2g7Ds281Tke5WfSb907tM/EaoGFgIJPP09HQioG1WpwaIyMg5zvjbSA0jbSnF6OcrnZrW0GrhzOHeQQPNTosjCHNe2ocBhPsmrSc+Y5HP5xCktHtAP0sjCXtj55TQjc3LjH051AvQ20QiZ0jxVjQCG0xVzeAKVHQDtGwLZNFLLq7LG0ilQ59DtAe4uaD14SFU9GuDKGyv1s9oMzW1cGljY2ADOryXOJGQqKgGmdRkpK/OECCKrbN8M/2gaRD621/1cj7QVRb5JGsaXvcGtaKlziA0DpJOQWb6baYRzUhswxBrqmU1DSaEUY05naczQbNoVUvq/rRajinkLgDUNHFjb2Wjn6zU9ajY43Pe1jAXvcaNa0EknmDWjnRVz4OrK6e2sc6pDPhHE/M5P8AUW5LalVOD7Rk2KAmWmuloXgZ4AOSyvOcySRznnoCrWsgXjmgihFQdoOxeoQcoLMxnIY1vZAb6LqhCBpeN2QWhuC0Qxyt9mRjXjwcCqbe/BFdc1THHJA488LyB9h+JoG4BX1CDD724D7Q2pslrjeOZszXRn7bMQJ+qFTL20DvSzZyWORzR8aGkzd9I6uA3gL6jQg+ODNQlpqHDa05EbwcwltnX1petyWa0jDabPFKP5jGvI3Eio7lSb34GrtlqYdbZ3fy3l7a9mXFl1AhBgrZ13s9rcw4mOc13tNJa7xGavl78CVtjzs08M46HB0L9wHGae9wVKvbRa8LLnaLJMwDa4N1jBvkjxNHiglrFpja4/lcY6JAHeYo7zU/YuEIbJoTvjcD/S6n3lmTLRXYV1bOlG0WLS6ySfLBh6JKs83cU9xU2yYOFWkEHnBqD3rAW2hOLLb3xmsb3MO3iOLa76HNWjeMa42h2Q7cX4jFlth03tTMnPbIPntFe4tp51U7ZNPI34RNG5nGYS5pxtADwSTkDsHMCg0MSZHcfRVzRyX9Vl63e5dGaUWMscf0iPYaAnCdnQ6ihrjv2zR2eVr54wSRQYhU5dARDS3wiQOY7Y4EVG0dBHWNqjLLcEhNHPbTpFTXrw0y3VS5tIrMCTVz+pjfe6gTS0aUyHKFjYx7R479+Ywg9xRVtsN0WaEa2YtoPjykBlegA5HdmnFq0/ijGGzRmQ7Mb6xxjc3lu2bCGjoKzWe1ue4Pkc57tgLjUjpA9kZbBkgz9/8AvoVE9fOkNotX7+Qltahg4sYpso0bSOk1PWo2ztfI7VxMdI87GsaXOP1W1KuuiHBlPaKTW4uhiNCI9kzx86v7sb+NtybtWt3Pc1nsrNXZomxt58Izd1ucc3HrJKlGKWDg7vKUjFCImn40j2ZDnOFhc6vUQFq2h+h0FgZlSSY8uYtAca/FZtwMy2V3qyIUAhCEAhCEAhCEAhCEAhCEAhCEAhCEEFfOh132uptFkhe47X4cEn/2Mo7zVIvjgPsb6myzzQnma6k0Y7jR/wDUtUQg+db34HLzhqYdVaG/Mfq307MlB4OKpV63TarKaWqzyw81ZGOa0nqfTC7uJX1+vHNBFCAQdoOYKD41ZMurZ19N3xwc3XaamSxxtcanHDWF1TtJ1ZGI76qkXvwFRmpsdsezobO1sg3Y2YSPAoMcdOkiZItVlexzmGhLXOaabKtJBp4J3cdx2q2SamyxGSShdhBY3iggE1eQOcc/Og4RvzThsy0C5OBS2yUNqmigb0NrNJ1igo0b8RWkaO8Ft3WWjnRG0SD49oo8A9UYAYOo0r1oMO0e0dtlueG2WFzgNsh4sTa85kOWzmFTnkCtu0F4OIbDSactmtO0PpxIvogefmxnPow1IV4ApkF6gEIQgEIQgEIQgEIQgEIQgEIQgEIQgEIQgEIQgEIQgEIQgEIQg+feFfRKz2KQyQGT4VxcWOILWlxJIbxcVK9JKu/A9orBDH+ntMhlkZq6OIwNacLjhAAzJAzJOzKlTUQg0tCEIBCEIBCEIBCEIBCEIP/Z";
    [SerializeField] private GameObject _sendingObjectText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    public Texture2D DeCompress(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }


    public void OnCaptureButtonClick()
    {
        tempObjectIds.Clear();//clearing object ids
        _sendingObjectText.SetActive(false);
        TakePicture(480);
        _uIManagerMap = GameObject.Find("Canvas").GetComponent<UIManagerMap>();
        if (_uIManagerMap == null)
        {
            Debug.LogError("UI Manager Map is Null");
        }
        _uIManagerMap.DisplayCapturePanel();

        #region Test Case For Taking Picture
        if (_PcdebugMode == true)
        {
            //base 64 convertor
            bytes = DeCompress(_testTexture).EncodeToJPG();
            image64 = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
            //print("this is local user save id: " + _localUser.userID);
            GameNetworkImage captured = new GameNetworkImage(_localUser.userID, image64, "0", "0");
            StartCoroutine(_gameNetwork.PostAddObject("https://sjoglekar-45523.portmap.io:45523/addObject", captured.Serialize().ToString(), GetObjectIds));
            #endregion
        }
    }

    //we get the id list here
    public void GetObjectIds(List<string> ids)
    {
        tempObjectIds = ids;

        foreach (var i in ids)
        {
            Debug.Log("Object ID is: " + i.ToString());
        }
    }


    public void OnNextButton()
    {
        //photoTaken.color = Color.red;
        //_uIManagerMap.DisplayQuestionPanel();
        if (labelInputText.text != "")
        {
            StartCoroutine(DisplayQuestionsPanel());
            // _uIManagerMap.DisplayQuestionPanel();
        }
        else
        {
            _placeHolder.color = Color.red;
        }
    }


    IEnumerator DisplayQuestionsPanel()
    {
        _sendingObjectText.SetActive(true);
        yield return new WaitUntil(() => tempObjectIds.Count != 0);
        _uIManagerMap.DisplayQuestionPanel();
    }

    private void TakePicture(int maxSize)
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {

            Debug.Log("Image path: " + path);
            if (path != null)
            {
                texture = NativeCamera.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                if (_PcdebugMode == false)
                {
                    photoTaken.gameObject.SetActive(true);
                    photoTaken.texture = texture;
                    //Sending the image which user has taken in 64bit format to the backend
                    byte[] currenBytes = DeCompress(texture).EncodeToJPG();
                    //bytes = _testTexture.EncodeToPNG();
                    string currentImage64 = "data:image/jpeg;base64," + Convert.ToBase64String(currenBytes);
                    GameNetworkImage captured = new GameNetworkImage(_localUser.userID, currentImage64, _locationStatus.GetLocationLat(), _locationStatus.GetLocationLon());
                    StartCoroutine(_gameNetwork.PostAddObject("https://sjoglekar-45523.portmap.io:45523/addObject", captured.Serialize().ToString(), GetObjectIds));
                }

                /*
				// Assign texture to a temporary quad and destroy it after 5 seconds
				GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
				quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
				quad.transform.forward = Camera.main.transform.forward;
				quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

				Material material = quad.GetComponent<Renderer>().material;
				if (!material.shader.isSupported) // happens when Standard shader is not included in the build
					material.shader = Shader.Find("Legacy Shaders/Diffuse");

				material.mainTexture = texture;

				Destroy(quad, 5f);

				// If a procedural texture is not destroyed manually, 
				// it will only be freed after a scene change
				Destroy(texture, 5f);
				*/
            }
            else
            {
                _uIManagerMap.DisplayMapPanel();
            }
        }, maxSize);
        Debug.Log("Permission result: " + permission);
    }
}


