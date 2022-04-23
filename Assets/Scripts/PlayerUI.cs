using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public Color positiveColor;
    public Color negativeColor;
    public TextMeshProUGUI textField;
    public float entryStaysFor = 2f;

    private List<(DateTime timestamp, string value)> uiTextEntries;

    void Start()
    {
        uiTextEntries = new List<(DateTime, string)>();
        InvokeRepeating("Cleanup", entryStaysFor, entryStaysFor);
    }

    public void PushMessage(string message, string type, bool isPositive)
    {
        string colorHex = ColorUtility.ToHtmlStringRGBA(isPositive ? positiveColor : negativeColor);
        uiTextEntries.Add((DateTime.Now,
            "<sprite name=\"" + type + "_icon\"> " + "<color=#" + colorHex + ">" + message + "</color>\n"
        ));
        DrawEntries();
    }

    private void Cleanup()
    {
        List<(DateTime, string)> newTextEntries = new List<(DateTime, string)>();

        foreach ((DateTime timestamp, string value) entry in uiTextEntries)
        {
            if (DateTime.Now.Subtract(entry.timestamp).TotalSeconds < entryStaysFor)
            {
                newTextEntries.Add((entry.timestamp, entry.value));
            }
        }

        uiTextEntries = newTextEntries;
        DrawEntries();
    }

    private void DrawEntries()
    {
        string result = "";
        foreach ((DateTime timestamp, string value) entry in uiTextEntries)
        {
            if (DateTime.Now.Subtract(entry.timestamp).TotalSeconds < entryStaysFor)
            {
                result += entry.value;
            }
        }

        textField.text = result;
    }
}
