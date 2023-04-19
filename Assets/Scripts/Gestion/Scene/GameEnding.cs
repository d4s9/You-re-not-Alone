using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{

    [SerializeField] private GameObject _topText;
    [SerializeField] private GameObject _GridContent;
    [SerializeField] private int _leaderBoardLenght;
    [SerializeField] private Button _timeBt;
    [SerializeField] private Button _scoreBt;
    [SerializeField] private TextMeshProUGUI _PlayerName;
    [SerializeField] private TextMeshProUGUI _nameError;

    Vector2 _startContentSize;
    private string _playerNameStr = "YOU";
    private List<string> _names;
    private List<string> _valueStr;
    private List<int> _values = new List<int>();
    private int _currentScore = 4;
    private int _currentTime = 1;
    private string[] _namesArr;
    private string[] _valueArr;
    private bool _isTime = true;
    private string[] a = {"a", "b","c","d","e", "a", "b", "c", "d", "e", "a", "b", "c", "d", "e", "a", "b", "c", "d", "e"};
    private int[] b = {10,5,4,3,2, 10, 5, 4, 3, 2, 10, 5, 4, 3, 2, 10, 5, 4, 3, 2};
    private int[] c = { 11, 6, 3, 2, 1, 11, 6, 3, 2, 1, 11, 6, 3, 2, 1, 11, 6, 3, 2, 1};
    GameObject[] _top;
    private int _currentPlace;
    void Start()
    {
        _startContentSize = _GridContent.GetComponent<RectTransform>().sizeDelta;
        _nameError.SetText("");
        /*
        PlayerPrefs.SetString("Top5Num", string.Join("###", a));
        string[] b = PlayerPrefs.GetString("Top5Num").Split(new[] { "###" }, System.StringSplitOptions.None);
        foreach(string f in b){
            Debug.Log(f);
        }
        */
        PlayerPrefs.SetString("Top5Names", string.Join("###", a));
        PlayerPrefs.SetString("Top5Times", string.Join("###", b));
        _timeBt.interactable = false;
        _scoreBt.interactable = true;

        CreateTop();
        CheckTop();
        ShowTop();
    }

    private void CreateTop()
    {
        _namesArr = PlayerPrefs.GetString("Top5Names").Split(new[] { "###" }, System.StringSplitOptions.None);
        _valueArr = _isTime ? (PlayerPrefs.GetString("Top5Times").Split(new[] { "###" }, System.StringSplitOptions.None)) :
            PlayerPrefs.GetString("Top5Scores").Split(new[] { "###" }, System.StringSplitOptions.None);
        _names = new List<string>(_namesArr);
        _valueStr = new List<string>(_valueArr);
        int count = 0;
        foreach (string v in _valueStr)
        {
            Debug.Log(v);
            _values.Add(int.Parse(v));
            count++;
        }
    }
    private void CheckTop()
    {
        float currentValue = _isTime ? _currentTime : _currentScore;
        _currentPlace = -1;
        for(int i = _values.Count-1; i >= 0; i--)
        {
            if(currentValue > _values[i])
            {
                if(i > 0)
                {
                    if(currentValue <= _values[i - 1])
                    {
                        _currentPlace = i;
                    }
                }
                else
                {
                    _currentPlace = i;
                }
            }
        }
        if (_currentPlace > -1 && _currentPlace != _values.Count)
        {
            for (int i = _values.Count - 1; i >= _currentPlace; i--)
            {
                if((i+1) == _values.Count && _values.Count < _leaderBoardLenght)
                {
                    _names.Add(_names[i]);
                    _values.Add(_values[i]);
                }
                else if((i + 1) != _values.Count)
                {
                    _names[i + 1] = _names[i];
                    _values[i + 1] = _values[i];
                }
            }
            _names[_currentPlace] = _playerNameStr;
            _values[_currentPlace] = Mathf.RoundToInt(currentValue);
        }

        if (_values.Count < _leaderBoardLenght && _currentPlace == -1)
        {
            _currentPlace = _values.Count;
            _names.Add(_playerNameStr);
            _values.Add(Mathf.RoundToInt(currentValue));
            
        }
   
        _top = new GameObject[_values.Count];
    }
    private void ShowTop()
    {
        int count = 0;
        foreach(string name in _names)
        {
            Vector2 currentContentSize = _GridContent.GetComponent<RectTransform>().sizeDelta;
            if (count > 10 && currentContentSize.y <= _startContentSize.y + (_names.Count-12)*20)
            {               
                _GridContent.GetComponent<RectTransform>().sizeDelta = currentContentSize + new Vector2(0, 20);
            }           
            _top[count] = Instantiate(_topText, _GridContent.transform);
            RectTransform m_RectTransform = _top[count].GetComponent<RectTransform>();
            m_RectTransform.anchoredPosition = new Vector2(1.3658f, 95 - (count * 20));
            _top[count].transform.Find("Name1").GetComponent<TextMeshProUGUI>().text = _names[count];
            _top[count].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = _values[count].ToString();
            _top[count].transform.Find("Place").GetComponent<TextMeshProUGUI>().text = (count+1).ToString();
            count++;
        }
        _GridContent.GetComponent<RectTransform>().anchoredPosition = Vector2.one;
    }

    private void DeleteTop()
    {
        int count = _names.Count-1;
        foreach(string name in _names)
        {
            Destroy(_top[count]);
            count--;
        }
    }

    //Ajouter scrollbar
    //Importer des bouttons
    public void TimeOnOff()
    {

        if(!_isTime)
        {
            _isTime = !_isTime;
            _timeBt.interactable = false;
            _scoreBt.interactable = true;
            PlayerPrefs.SetString("Top5Names", string.Join("###", a));
            PlayerPrefs.SetString("Top5Scores", string.Join("###", c));
            PlayerPrefs.Save();
            DeleteTop();
            _names.Clear();
            _valueStr.Clear();
            _values.Clear();
            CreateTop();
            CheckTop();
            ShowTop();
        }
    }
    public void ScoreOnOff()
    {
        if (_isTime)
        {
            _timeBt.interactable = true;
            _scoreBt.interactable = false;
            _isTime = !_isTime;
            PlayerPrefs.SetString("Top5Names", string.Join("###", a));
            PlayerPrefs.SetString("Top5Times", string.Join("###", b));
            PlayerPrefs.Save();
            DeleteTop();
            _names.Clear();
            _valueStr.Clear();
            _values.Clear();
            CreateTop();
            CheckTop();
            ShowTop();
        }
    }

    public void ConfirmName()
    {
        if((_PlayerName.text.Length-1) <= 11 && _PlayerName.text.Length > 0)
        {
            if (!_nameError.text.Length.Equals("")) { _nameError.SetText(""); }
            _playerNameStr = _PlayerName.text;
            if (_currentPlace > -1){
                _names[_currentPlace] = _playerNameStr;
                DeleteTop();
                ShowTop();
            }
        } 
        else
        {
            _nameError.SetText("Your username has to contain a maximum of 11 characters !");
        }
    }
}
