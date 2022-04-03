using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public GameObject tutorial1;
    public GameObject tutorial2;
    public GameObject tutorial3;

    private int currentScreen = 0;

    public void Next()
    {
        currentScreen++;

        switch (currentScreen)
        {
            case 1:
                tutorial1.SetActive(true);
                break;
            case 2:
                tutorial1.SetActive(false);
                tutorial2.SetActive(true);
                break;
            case 3:
                tutorial2.SetActive(false);
                tutorial3.SetActive(true);
                break;
            case 4:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
        }
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
    }
}
