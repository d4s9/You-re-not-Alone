using System;
using System.Collections.Generic;
using TMPro;
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
    private List<string> _namesS, _namesT;
    private List<string> _valueSStr, _valuesTStr;
    private List<float> _valuesS = new List<float>();
    private List<float> _valuesT = new List<float>();
    private int _currentScore;
    private float _currentTime;
    private string[] _namesTArr, _namesSArr;
    private string[] _valueTArr, _valueSArr;
    private bool _isTime = true;
    TimeSpan time;

    GameObject[] _top;
    private int _currentPlace;
    void Start()
    {
        
        _currentScore = PlayerPrefs.GetInt("PlayerScore", 0);
        _currentTime = float.Parse(PlayerPrefs.GetString("PlayerTime", 61.46f.ToString()));
        _startContentSize = _GridContent.GetComponent<RectTransform>().sizeDelta;
        _nameError.SetText("");
      
        _timeBt.interactable = false;
        _scoreBt.interactable = true;

        time = TimeSpan.FromSeconds(_currentTime);

        _ResultsTxt[0].SetText("Your Score : " + _currentScore.ToString());
        _ResultsTxt[1].SetText("Your Time : " + time.ToString("h':'mm':'ss':'ff"));

        CreateTop();
        CheckTop("Time");
        CheckTop("Score");
        ShowTop("Time");
        
        
    }

    private void CreateTop()
    {

        _valueSArr = PlayerPrefs.GetString("Top5Scores").Split(new[] { "###" }, System.StringSplitOptions.None);
        _valueTArr = PlayerPrefs.GetString("Top5Times").Split(new[] { "###" }, System.StringSplitOptions.None);
        _namesSArr = PlayerPrefs.GetString("Top5NamesS").Split(new[] { "###" }, System.StringSplitOptions.None);
        _namesTArr = PlayerPrefs.GetString("Top5NamesT").Split(new[] { "###" }, System.StringSplitOptions.None);

        _namesS = new List<string>(_namesSArr);
        _valueSStr = new List<string>(_valueSArr);
        _namesT = new List<string>(_namesTArr);
        _valuesTStr = new List<string>(_valueTArr);
        if (!_namesSArr[0].Equals(""))
        {
            int count = 0;
            foreach (string v in _valueSStr)
            {                
                _valuesS.Add(int.Parse(v));
                count++;
            }

        }
        else
        {
            _namesS.Clear();
            _valuesS.Clear();
            _valueSStr.Clear();
        }
        if (!_namesTArr[0].Equals(""))
        {
            int count = 0;
            foreach (string v in _valuesTStr)
            {
                _valuesT.Add(float.Parse(v));
                count++;
            }

        }
        else
        {
            _namesT.Clear();
            _valuesT.Clear();
            _valuesTStr.Clear();
        }
    }
    private void CheckTop(string leaderboard)
    {
        List<float> _values = leaderboard.Equals("Time") ? _valuesT : _valuesS;
        List<string> _names = leaderboard.Equals("Time") ? _namesT : _namesS;
        List<string> _valueStr = leaderboard.Equals("Time") ? _valuesTStr : _valueSStr;
        float currentValue = leaderboard.Equals("Time") ? _currentTime : _currentScore;

        _currentPlace = -1;
        for (int i = _values.Count - 1; i >= 0; i--)
        {
            if (currentValue > _values[i])
            {
                if (i > 0)
                {
                    if (currentValue <= _values[i - 1])
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
                if ((i + 1) == _values.Count && _values.Count < _leaderBoardLenght)
                {
                    _names.Add(_names[i]);
                    _values.Add(_values[i]);
                    _valueStr.Add(_valueStr[i]);
                }
                else if ((i + 1) != _values.Count)
                {
                    _names[i + 1] = _names[i];
                    _values[i + 1] = _values[i];
                    _valueStr[i + 1] = _valueStr[i];
                }
            }
            _names[_currentPlace] = _playerNameStr;
            _values[_currentPlace] = currentValue;
            _valueStr[_currentPlace] = currentValue.ToString();
        }

        if (_values.Count < _leaderBoardLenght && _currentPlace == -1)
        {
            _currentPlace = _values.Count;
            _names.Add(_playerNameStr);
            _values.Add(currentValue);
            _valueStr.Add(currentValue.ToString());

        }


        _top = new GameObject[_values.Count];
    }
    private void ShowTop(string leaderboard)
    {
        List<float> _values = leaderboard.Equals("Time") ? _valuesT : _valuesS;
        List<string> _names = leaderboard.Equals("Time") ? _namesT : _namesS;
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
            if(leaderboard.Equals("Time"))
            {
                time = TimeSpan.FromSeconds(_values[count]);
                _top[count].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = time.ToString("h':'mm':'ss':'ff");
            }
            else
            {
                _top[count].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = _values[count].ToString();
            }
            _top[count].transform.Find("Place").GetComponent<TextMeshProUGUI>().text = (count + 1).ToString();
            count++;
        }
        _GridContent.GetComponent<RectTransform>().anchoredPosition = Vector2.one;     
    }


    private void DeleteTop(string leaderboard)
    {
        List<string> _names = leaderboard.Equals("Time") ? _namesT : _namesS;
        int count = _names.Count-1;
        foreach(string name in _names)
        {
            Destroy(_top[count]);
            count--;
        }
    }

    private void Save()
    {
        if (_namesS.Contains("YOU"))
        {
            _namesS[_namesS.IndexOf("YOU")] = "Player";
        }
        if (_namesT.Contains("YOU"))
        {
            _namesT[_namesT.IndexOf("YOU")] = "Player";
        }

        PlayerPrefs.SetString("Top5Times", string.Join("###", _valuesTStr));
        PlayerPrefs.SetString("Top5Scores", string.Join("###", _valueSStr));
        PlayerPrefs.SetString("Top5NamesS", string.Join("###", _namesS));
        PlayerPrefs.SetString("Top5NamesT", string.Join("###", _namesT));
        PlayerPrefs.Save();
    }

    public void TimeOnOff()
    {

        if(!_isTime)
        {

            _isTime = !_isTime;
            _timeBt.interactable = false;
            _scoreBt.interactable = true;
            DeleteTop("Score");
            ShowTop("Time");
        }
        else
        {
            _timeBt.interactable = true;
            _scoreBt.interactable = false;
            _isTime = !_isTime;
            DeleteTop("Time");
            ShowTop("Score");         
        }

    }
    public void MainMenu()
    {
        Save();
    }
    public void OnApplicationQuit()
    {
        Save();
    }

    public void ConfirmName()
    {
        if((_PlayerName.text.Length-1) <= 11 && _PlayerName.text.Length > 1)
        {
            if (!_nameError.text.Length.Equals("")) { _nameError.SetText(""); }
            _playerNameStr = _PlayerName.text;
            if (_currentPlace > -1){
                _namesS[_currentPlace] = _playerNameStr;
                _namesT[_currentPlace] = _playerNameStr;
                if (_isTime)
                {
                    DeleteTop("Time");
                    ShowTop("Time");
                } else
                {
                    DeleteTop("Score");
                    ShowTop("Score");
                }
                
            }
        } 
        else
        {
            _nameError.SetText("Your username has to contain a maximum of 11 characters !");
        }
    }
}
