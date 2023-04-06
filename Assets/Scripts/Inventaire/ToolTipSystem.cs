using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToolTipSystem : MonoBehaviour
{
    public static ToolTipSystem instance;

    [SerializeField] private Tooltip tooltip;

    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        tooltip.gameObject.SetActive(false);
    }

    public void Show(string content, string header)
    {
        tooltip.SetText(content, header);
        tooltip.gameObject.SetActive(true);
    }
    public void Hide()
    {
        tooltip.gameObject.SetActive(false);
    }
}
