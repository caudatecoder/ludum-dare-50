using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoSystem : MonoBehaviour
{
    public int popularity = 50;
    public int mediaControl = 30;
    public int policeForce = 50;
    public List<string> staticEffects;

    private bool negativeEffects = true;

    public void PressBusted()
    {
        if (negativeEffects) { popularity -= 1; }
        mediaControl += 3;

        CheckLimits();
    }

    public void CopBusted()
    {
        popularity += 3;
        if (negativeEffects)
        {
            policeForce -= 3;
        }

        CheckLimits();
    }

    public void PressMissed()
    {
        if (negativeEffects)
        {
            popularity -= 3;
            mediaControl -= 2;
        }

        CheckLimits();
    }

    public void CopMissed()
    {
        policeForce += 1;

        CheckLimits();
    }

    public void CheckLimits()
    {
        ApplyStaticEffects();

        if (popularity > 100) { popularity = 100; }
        if (mediaControl > 100) { mediaControl = 100; }
        if (policeForce > 100) { policeForce = 100; }

        if (popularity < 0) { popularity = 0; }
        if (mediaControl < 0) { mediaControl = 0; }
        if (policeForce < 0) { policeForce = 0; }
    }

    private void ApplyStaticEffects()
    {
        staticEffects = new List<string>();

        if (mediaControl > 70)
        {
            negativeEffects = false;
            staticEffects.Add("Media control > 70: no negative effects!");
        } else
        {
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

            if (policeForce < 60)
            {
                mediaControl -= 1;
                staticEffects.Add("Police force < 60: -1 additional media control on every event");
            }

            if (policeForce < 30)
            {
                mediaControl -= 2;
                staticEffects.Add("Police force < 30: -2 additional media control on every event");
            }
        }


        if (popularity > 70)
        {
            mediaControl += 1;
            staticEffects.Add("Popularity > 70: +1 additional media control on every event");
        }

        staticEffects.ForEach((item) => { Debug.Log(item); });
       
    }
}
