using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PageTextFadeIn : MonoBehaviour
{
    [SerializeField] private float _fadeTime = 1f;

    private void Start()
    {
        //foreach(var text in _WelcomPanelTexts)
        //{
        //    text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        //}
        //StartCoroutine(Fade(false, _WelcomPanelTexts));
    }

    public void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    StartCoroutine(Fade(true, _WelcomPanelTexts));
        //}
    }

    public IEnumerator Fade(bool fadeAway, Text[] texts)
    {
        if (fadeAway)
        {
            for(float i = _fadeTime; i >= 0; i -= Time.deltaTime)
            {
                foreach(var text in texts)
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
            for (float i = 0; i<= _fadeTime; i += Time.deltaTime)
            {
                foreach(var text in texts)
                {
                    //set color with i as alpha
                    text.color = new Color(text.color.r, text.color.g, text.color.b, i);
                }
                yield return null;
            }
        }
    }
}
