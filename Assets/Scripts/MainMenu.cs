using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Cinematic");
    }

    public void Quit()
    {
        Application.Quit();
    }   
}
