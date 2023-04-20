using System;
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
    [SerializeField] private TextMeshProUGUI[] _ResultsTxt;

    Vector2 _startContentSize;
    private string _playerNameStr = "YOU";
    private List<string> _names;
    private List<string> _valueStr;
    private List<int> _values = new List<int>();
    private int _currentScore = 10;
    private int _currentTime = 3;
    private string[] _namesArr;
    private string[] _valueArr;
    private bool _isTime = true;
  //  private string[] a = {"a", "b","c","d","e", "a", "b", "c", "d", "e", "a", "b", "c", "d", "e", "a", "b", "c", "d", "e"};
   // private int[] b = {10,5,4,3,2, 10, 5, 4, 3, 2, 10, 5, 4, 3, 2, 10, 5, 4, 3, 2};
    //private int[] c = { 11, 6, 3, 2, 1, 11, 6, 3, 2, 1, 11, 6, 3, 2, 1, 11, 6, 3, 2, 1};
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
        /*
        PlayerPrefs.SetString("Top5Names", string.Join("###", null));
        PlayerPrefs.SetString("Top5Times", string.Join("###", null));
        PlayerPrefs.SetString("Top5Scores", string.Join("###", null));
        */
        //PlayerPrefs.DeleteAll();
        
        _timeBt.interactable = false;
        _scoreBt.interactable = true;

        _ResultsTxt[0].SetText("Your Score : " + _currentScore.ToString());
        _ResultsTxt[1].SetText("Your Time : " + _currentTime.ToString("00:00.00"));

        CreateTop();
        CheckTop();
        ShowTop();
        
        
    }

    private void CreateTop()
    {
        _namesArr = _isTime ? PlayerPrefs.GetString("Top5NamesT").Split(new[] { "###" }, System.StringSplitOptions.None) :
            PlayerPrefs.GetString("Top5NamesS").Split(new[] { "###" }, System.StringSplitOptions.None);
        _valueArr = _isTime ? (PlayerPrefs.GetString("Top5Times").Split(new[] { "###" }, System.StringSplitOptions.None)) :
            PlayerPrefs.GetString("Top5Scores").Split(new[] { "###" }, System.StringSplitOptions.None);
        _names = new List<string>(_namesArr);
        _valueStr = new List<string>(_valueArr);
        if (!_namesArr[0].Equals(""))
        {
            int count = 0;
            foreach (string v in _valueStr)
            {                
                _values.Add(int.Parse(v));
                count++;
            }
        }
        else
        {
            Debug.Log(1);
            _names.Clear();
            _values.Clear();
            _valueStr.Clear();
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
                    _valueStr.Add(_valueStr[i]);
                }
                else if((i + 1) != _values.Count)
                {
                    _names[i + 1] = _names[i];
                    _values[i + 1] = _values[i];
                    _valueStr.Add(_valueStr[i]);
                }
            }
            _names[_currentPlace] = _playerNameStr;
            _values[_currentPlace] = Mathf.RoundToInt(currentValue);
            _valueStr[_currentPlace] = Mathf.RoundToInt(currentValue).ToString();
        }
  
        if (_values.Count < _leaderBoardLenght && _currentPlace == -1)
        {
            _currentPlace = _values.Count;
            _names.Add(_playerNameStr);
            _values.Add(Mathf.RoundToInt(currentValue));
            _valueStr.Add(Mathf.RoundToInt(currentValue).ToString());
            
        }
        

        _top = new GameObject[_values.Count];
    }
    private void ShowTop()
    {
        int count = 0;
        foreach (string name in _names)
        {
            Vector2 currentContentSize = _GridContent.GetComponent<RectTransform>().sizeDelta;
            if (count > 10 && currentContentSize.y <= _startContentSize.y + (_names.Count - 12) * 30)
            {
                _GridContent.GetComponent<RectTransform>().sizeDelta = currentContentSize + new Vector2(0, 30);
            }
            _top[count] = Instantiate(_topText, _GridContent.transform);
            RectTransform m_RectTransform = _top[count].GetComponent<RectTransform>();
            m_RectTransform.anchoredPosition = new Vector2(1.3658f, 95 - (count * 20));
            _top[count].transform.Find("Name1").GetComponent<TextMeshProUGUI>().text = _names[count];
            _top[count].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = _values[count].ToString();
            _top[count].transform.Find("Place").GetComponent<TextMeshProUGUI>().text = (count + 1).ToString();
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

    public void TimeOnOff()
    {

        if(!_isTime)
        {
            Debug.Log(_names[0]);
            Debug.Log(_valueStr[0]);
            if (_names.Contains("YOU"))
            {
                _names[_names.IndexOf("YOU")] = "Player";
            }
            _isTime = !_isTime;
            _timeBt.interactable = false;
            _scoreBt.interactable = true;
            Debug.Log(string.Join("###", _names));
            PlayerPrefs.SetString("Top5NamesS", string.Join("###", _names));
            PlayerPrefs.SetString("Top5Scores", string.Join("###", _valueStr));
            PlayerPrefs.Save();
        }
        else
        {
            if (_names.Contains("YOU"))
            {
                _names[_names.IndexOf("YOU")] = "Player";
            }
            _timeBt.interactable = true;
            _scoreBt.interactable = false;
            _isTime = !_isTime;
            Debug.Log(string.Join("###", _names));
            PlayerPrefs.SetString("Top5NamesT", string.Join("###", _names));
            PlayerPrefs.SetString("Top5Times", string.Join("###", _valueStr));
            PlayerPrefs.Save();
        }
        DeleteTop();
        _names.Clear();
        _valueStr.Clear();
        _values.Clear();
        CreateTop();
        CheckTop();
        ShowTop();
    }
    public void ScoreOnOff()
    {
        if (_isTime)
        {
            _timeBt.interactable = true;
            _scoreBt.interactable = false;
            _isTime = !_isTime;
            if (_names.Contains("YOU"))
            {
                _names[_names.IndexOf("YOU")] = "Player";
            }
            PlayerPrefs.SetString("Top5NamesT", string.Join("###", _names));
            PlayerPrefs.SetString("Top5Times", string.Join("###", _valueStr));
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
        if((_PlayerName.text.Length-1) <= 11 && _PlayerName.text.Length > 1)
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
