using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class GameEnding : MonoBehaviour
{

    //Use a list

    [SerializeField] private GameObject _topText;
    [SerializeField] private GameObject _Grid;
    [SerializeField] private int _leaderBoardLenght;

    private int _currentScore;
    private int _currentTime = 1;
    private string[] _names;
    private string[] _valueStr;
    private int[] _value;
    private bool _isTime = true;
    private string[] a = {"a", "b"};
    private int[] b = {5, 3};
    GameObject[] _top;
    private int _currentPlace;
    void Start()
    {
        /*
        PlayerPrefs.SetString("Top5Num", string.Join("###", a));
        string[] b = PlayerPrefs.GetString("Top5Num").Split(new[] { "###" }, System.StringSplitOptions.None);
        foreach(string f in b){
            Debug.Log(f);
        }
        */
        PlayerPrefs.SetString("Top5Names", string.Join("###", a));
        PlayerPrefs.SetString("Top5Times", string.Join("###", b));

        CreateTop();
        CheckTop();
        ShowTop();
    }

    private void CreateTop()
    {
        _names = PlayerPrefs.GetString("Top5Names").Split(new[] { "###" }, System.StringSplitOptions.None);
        _valueStr = _isTime ? (PlayerPrefs.GetString("Top5Times").Split(new[] { "###" }, System.StringSplitOptions.None)) :
            PlayerPrefs.GetString("Top5Scores").Split(new[] { "###" }, System.StringSplitOptions.None);
        _value = new int[_valueStr.Length];
        int count = 0;
        foreach(string v in _valueStr)
        {
            _value[count] = int.Parse(v);
            count++;
        }
    }
    private void CheckTop()
    {
        float currentValue = _isTime ? _currentTime : _currentScore;
        _currentPlace = -1;
        for(int i = (_value.Length-1); i >= 0; i--)
        {
            if(currentValue > _value[i])
            {
                if(i > 0)
                {
                    if(currentValue <= _value[i - 1])
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
        //Add if value is added at the end
        if(_value.Length < _leaderBoardLenght)
        {
            _currentPlace = _value.Length;
            
        }
        if(_currentPlace > -1 && _currentPlace != _value.Length)
        {
            for (int i = _leaderBoardLenght - 1; i >= _currentPlace; i--)
            {
                _names[i + 1] = _names[i];
                _value[i + 1] = _value[i];
            }
            _names[_currentPlace] = "YOU";
            _value[_currentPlace] = Mathf.RoundToInt(currentValue);
        }
        _top = new GameObject[_value.Length];
    }
    private void ShowTop()
    {
        int count = 0;
        foreach(string name in _names)
        {
            _top[count] = Instantiate(_topText, _Grid.transform);
            RectTransform m_RectTransform = _top[count].GetComponent<RectTransform>();
            m_RectTransform.anchoredPosition = new Vector2(1.3658f, 95 - (count * 20));
            _top[count].transform.Find("Name1").GetComponent<TextMeshProUGUI>().text = _names[count];
            _top[count].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = _value[count].ToString();
            _top[count].transform.Find("Place").GetComponent<TextMeshProUGUI>().text = (count+1).ToString();
            count++;
        }
    }

    public void TimeOnOff()
    {

    }
}
