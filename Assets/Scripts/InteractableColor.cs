using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableColor : MonoBehaviour
{
    public Button parentButton;
    public Color activeColor;
    public Color inactiveColor;

    void Start()
    {
        GetComponent<Text>().color = parentButton.interactable ? activeColor : inactiveColor;
    }

    void Update()
    {
        GetComponent<Text>().color = parentButton.interactable ? activeColor : inactiveColor;
    }
}
