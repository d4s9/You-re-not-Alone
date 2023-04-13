using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI contentField;
    [SerializeField] private LayoutElement layoutElement;

    [SerializeField] int maxCharacter;





    public void SetText(string header , string content )
    {

            headerField.gameObject.SetActive(true);
            headerField.text = header;

        contentField.text = content;


        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > maxCharacter || contentLength > maxCharacter) ? true : false;
    }
    

}
