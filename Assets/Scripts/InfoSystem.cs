using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InfoSystem : MonoBehaviour
{
    public int popularity = 50;
    public int mediaControl = 30;
    public int policeForce = 50;
    public List<string> staticEffects;
    public GameObject playerRenderer;

    [Header("UI")]
    public Text populatiryIndicator;
    public Text mediaControlIndicator;
    public Text policeIndicator;
    public Text staticEffectsText;
    public GameObject eventsBoard;
    public Text eventTitle;
    public Button SeparatistButton;
    public Button WarButton;
    public GameObject WarWrapper;
    public Button PoliceButton;
    public Button LawButton;
    public Button CaptureButton;
    public Button PoisonButton;

    public GameObject newsScreen;
    private bool mediaControlWasHigh = false;

    public GameObject endgameScreen;
    public Text endgameText;
    public GameObject additionalEndgameText;

    [Header("Music")]
    public AudioSource theme1;
    public AudioSource theme2;
    public AudioSource theme3;
    public AudioSource theme4;

    private bool negativeEffects = true;
    private bool lawChanged = false;
    private bool policeFunded = false;
    private bool territoryCaptured = false;
    private bool oppositionPoisoned = false;
    private bool separatistSupported = false;
    public bool warStarted = false;

    public UnityEvent stepEvent;

    private GameObject player;
    private PlayerUI playerUI;

    private int eventsCount = 0;

    private void Start()
    {
        if (stepEvent == null)
            stepEvent = new UnityEvent();

        CheckLimits();
        Invoke("NextEvent", 30f);
        InvokeRepeating("Step", 0.5f, 0.5f);

        player = GameObject.FindWithTag("Player");
        playerUI = player.GetComponent<PlayerUI>();
    }

    private void Step()
    {
        stepEvent.Invoke();
    }

    public void PressBusted()
    {
        if (negativeEffects) { 
            popularity -= 1;
            playerUI.PushMessage("-1", "pop", false);

        }
        mediaControl += 3;
        playerUI.PushMessage("+3", "media", true);


        CheckLimits();
    }

    public void CopBusted()
    {
        popularity += 3;
        playerUI.PushMessage("+3", "pop", true);

        if (negativeEffects)
        {
            policeForce -= 3;
            playerUI.PushMessage("-3", "cop", false);
        }

        CheckLimits();
    }

    public void PressMissed()
    {
        mediaControl -= 2;
        playerUI.PushMessage("-2", "media", false);
        popularity -= 3;
        playerUI.PushMessage("-3", "pop", false);

        CheckLimits();
    }

    public void CopMissed()
    {
        policeForce += 1;
        playerUI.PushMessage("+1", "cop", true);

        CheckLimits();
    }

    public void NextEvent()
    {
        eventsCount += 1;

        if (eventsCount > 3 && !lawChanged)
        {
            EndGame("Dura lex, sed lex. You cannot participate in the upcoming election.");
            return;
        }

        if (warStarted)
        {
            EndGame("Your war was a failure. The corruption consumed everything. You hide in a bunker afraid of your own shadow. There's only one way out of this...", true);
            return;
        }

        if (eventsCount > 7)
        {
            EndGame("Your sickness consumed you. Soon enough your crimes will be revealed and your legacy destroyed.\nNice try though", true);
            return;
        }

        eventTitle.text = eventsCount % 2 == 0 ? "UPCOMING ELECTION. CHOOSE WISELY" : "MIDTERM ACTION. CHOOSE WISELY";

        SeparatistButton.interactable = CanSupportSeparatists();
        WarButton.interactable = CanStartWar();
        PoliceButton.interactable = !policeFunded;
        LawButton.interactable = CanChangeLaw();
        CaptureButton.interactable = CanCapture();
        PoisonButton.interactable = CanPoison();

        eventsBoard.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void AfterEvent()
    {
        Unpause();
        Invoke("NextEvent", 30f);
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        eventsBoard.SetActive(false);
        newsScreen.SetActive(false);
    }

    #region STRATEGY
    public void SupportSeparatists()
    {
        if (policeForce > 50 && mediaControl > 60)
        {
            SeparatistButton.interactable = false;
            separatistSupported = true;
            WarWrapper.SetActive(true);

            popularity += 10;
        }

        AfterEvent();
    }

    public bool CanSupportSeparatists() { return !separatistSupported && policeForce > 50 && mediaControl > 60; }


    public void PoisonOpposition()
    {
        if (policeForce > 50)
        {
            PoisonButton.interactable = false;
            oppositionPoisoned = true;

            popularity -= 10;
            mediaControl += 10;
        }

        AfterEvent();
    }

    public bool CanPoison() { return !oppositionPoisoned && policeForce > 50; }


    public void FundPolice()
    {
        PoliceButton.interactable = false;
        policeFunded = true;

        policeForce += 15;
        popularity -= 10;
        mediaControl -= 5;

        AfterEvent();
    }

    public void CaptureTerritory()  
    {
        if (CanCapture())
        {
            CaptureButton.interactable = false;
            territoryCaptured = true;
            WarWrapper.SetActive(true);

            popularity += 20;
        }
        AfterEvent();
    }

    public bool CanCapture() { return !territoryCaptured && mediaControl > 60 && policeForce > 60; }


    public void StartWar()
    {
        if (CanStartWar())
        {
            WarButton.interactable = false;
            warStarted = true;

            mediaControl = 100;
            popularity += 20;
            playerRenderer.transform.localScale = new Vector3(5, 1, 1);
            player.GetComponent<BoxCollider2D>().size = new Vector2(5, 1);
            playerRenderer.GetComponent<Animator>().SetBool("Bloody", true);

            theme2.Stop();
            theme3.Play();
        }

        AfterEvent();
    }

    public bool CanStartWar() { return (separatistSupported || territoryCaptured) && !warStarted && policeForce > 70 && mediaControl > 70; }


    public void ChangeLaw()
    {
        if (CanChangeLaw())
        {
            LawButton.interactable = false;
            popularity -= 5;
            mediaControl -= 10;
            lawChanged = true;
            player.GetComponent<BoxCollider2D>().size = new Vector2(3, 1);
            playerRenderer.transform.localScale = new Vector3(3, 1, 1);

            theme1.Stop();
            theme2.Play();
        }

        AfterEvent();
    }

    public bool CanChangeLaw() { return !lawChanged && popularity > 50 && mediaControl > 70; }
    #endregion

    public void CheckLimits()
    {
        ApplyStaticEffects();

        if (popularity > 100) { popularity = 100; }
        if (mediaControl > 100) { mediaControl = 100; }
        if (policeForce > 100) { policeForce = 100; }

        if (popularity < 0) { popularity = 0; }
        if (mediaControl < 0) { mediaControl = 0; }
        if (policeForce < 0) { policeForce = 0; }

        CheckEndConditions();

        populatiryIndicator.text = "Popularity: " + popularity + "/100";
        mediaControlIndicator.text = "Media control: " + mediaControl + "/100";
        policeIndicator.text = "Police force: " + policeForce + "/100";
    }

    private void ApplyStaticEffects()
    {
        staticEffects = new List<string>();

        if (mediaControl > 70)
        {
            negativeEffects = false;
            staticEffects.Add("Media control > 70: stomp em all without consequences!");

            if (!mediaControlWasHigh)
            {
                mediaControlWasHigh = true;
                ShowNewsBoard();
            }
        } else
        {
            mediaControlWasHigh = false;
            if (mediaControl < 50)
            {
                popularity -= 1;
                staticEffects.Add("Media control < 50: -1 additional popularity on every event");
            }

            if (mediaControl < 30)
            {
                popularity -= 2;
                staticEffects.Add("Media control < 30: -2 additional popularity on every event");
            }

            if (policeForce < 50)
            {
                mediaControl -= 1;
                staticEffects.Add("Police force < 50: -1 additional media control on every event");
            }

            if (policeForce < 30)
            {
                mediaControl -= 2;
                staticEffects.Add("Police force < 30: -2 additional media control on every event");
            }
        }

        if (!lawChanged)
        {
            staticEffects.Add("Laws don't allow you to stay more than 2 terms. Change it!");
        }


        if (popularity > 70)
        {
            mediaControl += 1;
            staticEffects.Add("Popularity > 70: +1 additional media control on every event");
        }

        staticEffectsText.text = "";
        staticEffects.ForEach((item) => { staticEffectsText.text += item + "\n"; });
    }

    private void CheckEndConditions()
    {
        if (popularity == 0)
        {
            if (policeForce > 50)
            {
                EndGame("One of your close assistants ran a coup and put you in jail!");
            }
            else
            {
                EndGame("You were impeached and couldn't do anything about it!");
            }
        }
    }

    private void ShowNewsBoard()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        newsScreen.SetActive(true);
    }

    private void EndGame(string message, bool intendedEnding = false)
    {
        Time.timeScale = 0f;
        theme1.Stop();
        theme2.Stop();
        theme3.Stop();
        theme4.Play();

        endgameScreen.SetActive(true);
        if (intendedEnding) { additionalEndgameText.SetActive(true); }

        endgameText.text = message;
    }
}
