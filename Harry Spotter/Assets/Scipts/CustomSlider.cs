using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomSlider : MonoBehaviour
{

    private Slider _mySlider;
    [SerializeField] private Image _myHandle;
    [SerializeField] private Text _sliderText;
    // Start is called before the first frame update
    void Start()
    {
        _mySlider = GetComponent<Slider>();
        if(_mySlider == null)
        {
            Debug.LogError("Slider is null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //_myHandle.color = new Color(74, 142, 144);
        if (_mySlider.value == 0f)
        {
            //_sliderText.color = Color.black;
            _sliderText.text = "X";
            //_mySlider.handleRect.GetComponent<Image>().color = Color.white;

        } else if (_mySlider.value == 1f)
        {
            //_sliderText.color = Color.white;
            _sliderText.text = "1";
            //_mySlider.handleRect.GetComponent<Image>().color = new Color(74, 142, 144);
        } else if (_mySlider.value == 2f)
        {
            _sliderText.text = "2";
        } else if ( _mySlider.value == 3f)
        {
            _sliderText.text = "3";
        } else if (_mySlider.value == 4f)
        {
            _sliderText.text = "4";
        } else if (_mySlider.value == 5f)
        {
            _sliderText.text = "5";
        } else if (_mySlider.value == 6f)
        {
            _sliderText.text = "6";
        } else if (_mySlider.value == 7f)
        {
            _sliderText.text = "7";
        }
    }
}
