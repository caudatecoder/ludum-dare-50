using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Button))]
public class InteractableColor : MonoBehaviour
{   
    private Button button;
    private CanvasGroup canvasGroup;

    void Start()
    {
        button = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();


        canvasGroup.alpha = button.interactable ? 1f : 0.2f;
    }

    void Update()
    {
        canvasGroup.alpha = button.interactable ? 1f : 0.2f;
    }
}
