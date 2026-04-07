using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    [Header("Références")]
    public VideoPlayer videoPlayer;
    public string nextSceneName = "TempleScene";

    [Header("Options")]
    public bool allowSkip = true;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void Update()
    {
        if (allowSkip && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space)))
        {
            LoadNextScene();
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        videoPlayer.Stop();
        SceneManager.LoadScene(nextSceneName);
    }
}