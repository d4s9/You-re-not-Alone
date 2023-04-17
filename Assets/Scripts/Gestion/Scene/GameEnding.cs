using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameEnding : MonoBehaviour
{

    [SerializeField] private GameObject _topText;
    [SerializeField] private GameObject _Grid;
    [SerializeField] private int _leaderBoardLenght;

    private List<string> _names;
    private List<string> _valueStr;
    private List<int> _values = new List<int>();
    private int _currentScore = 4;
    private int _currentTime = 1;
    private string[] _namesArr;
    private string[] _valueArr;
    private bool _isTime = true;
    private string[] a = {"a", "b","c","d","e"};
    private int[] b = {10,5,4,3,2};
    private int[] c = { 11, 6, 3, 2, 1 };
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
                    Debug.Log(i + 1);
                    _names[i + 1] = _names[i];
                    _values[i + 1] = _values[i];
                }
            }
            _names[_currentPlace] = "YOU";
            _values[_currentPlace] = Mathf.RoundToInt(currentValue);
        }

        if (_values.Count < _leaderBoardLenght && _currentPlace == -1)
        {
            _currentPlace = _values.Count;
            _names.Add("YOU");
            _values.Add(Mathf.RoundToInt(currentValue));
            
        }
   
        _top = new GameObject[_values.Count];
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
            _top[count].transform.Find("Value").GetComponent<TextMeshProUGUI>().text = _values[count].ToString();
            _top[count].transform.Find("Place").GetComponent<TextMeshProUGUI>().text = (count+1).ToString();
            count++;
        }
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
    //régler bouttons on/off score et time laisser enfoncé quand activé
    //Ajouter le player dans le player prefs et peut custom nom avec champ texte
    //Ajouter scrollbar
    //Importer des bouttons
    public void TimeOnOff()
    {
        _isTime = !_isTime;
        if(_isTime)
        {
            PlayerPrefs.SetString("Top5Names", string.Join("###", a));
            PlayerPrefs.SetString("Top5Times", string.Join("###", b));
        }
        else
        {
            PlayerPrefs.SetString("Top5Names", string.Join("###", a));
            PlayerPrefs.SetString("Top5Scores", string.Join("###", c));
        }
        DeleteTop();
        _names.Clear();
        _valueStr.Clear();
        _values.Clear();
        CreateTop();
        CheckTop();
        ShowTop();
    }
}
