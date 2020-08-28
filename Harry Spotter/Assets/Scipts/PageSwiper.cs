using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PageSwiper : MonoBehaviour, IDragHandler,IEndDragHandler
{
    //Responsible for page swipe animation (Attach to Panel Holder GameObject)
    private Vector3 _panelLocation;
    [SerializeField] private float _percentThreshold = 0.2f;
    [SerializeField] private float _easing = 0.5f;
    [SerializeField] private int _totalPages = 1;

    [Tooltip("Turn On if you want to show multiple page UI indicator. Make Sure to assign pageSelectorUI in the Inspector ")]
    [SerializeField] private bool _showPageSelectorUI;
    [SerializeField] private Image[] _pageSelectorUI;
    [SerializeField] private Sprite _selectedUISprit;
    [SerializeField] private Sprite _unSelectedUISprite;

    public int currentPage = 1;
  
    // Start is called before the first frame update
    void Start()
    {
        _panelLocation = transform.position;
        if(_showPageSelectorUI == true)
        {
            _pageSelectorUI[currentPage - 1].GetComponent<Image>().color = Color.white;
        }
    }

    void Update()
    {

        //print("We are in page; " + currentPage);
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        transform.position = _panelLocation - new Vector3(difference, 0, 0);
    }

    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        if (Mathf.Abs(percentage) >= _percentThreshold)
        {
            Vector3 newLocation = _panelLocation;
            if (percentage > 0 && currentPage < _totalPages)
            {
                Debug.Log("page up");
                currentPage++;
                newLocation += new Vector3(-Screen.width, 0, 0);
                if(_showPageSelectorUI == true)
                {
                    //Hilighting the Currant Page UI
                    //_pageSelectorUI[currentPage - 1].GetComponent<Image>().color = Color.white;
                    //_pageSelectorUI[currentPage - 2].GetComponent<Image>().color = Color.gray;
                    _pageSelectorUI[currentPage - 1].GetComponent<Image>().sprite = _selectedUISprit;
                    _pageSelectorUI[currentPage - 2].GetComponent<Image>().sprite = _unSelectedUISprite;
                }
            }
            else if (percentage < 0 && currentPage > 1)
            {
                Debug.Log("page down");
                currentPage--;
                newLocation += new Vector3(Screen.width, 0, 0);
                if(_showPageSelectorUI == true)
                {
                    //Hilighting the Currant Page UI
                    //_pageSelectorUI[currentPage - 1].GetComponent<Image>().color = Color.white;
                    //_pageSelectorUI[currentPage].GetComponent<Image>().color = Color.gray;
                    _pageSelectorUI[currentPage - 1].GetComponent<Image>().sprite = _selectedUISprit;
                    _pageSelectorUI[currentPage].GetComponent<Image>().sprite = _unSelectedUISprite;
                }
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, _easing));
            _panelLocation = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, _panelLocation, _easing));
        }
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}
