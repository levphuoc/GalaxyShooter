using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public string startScene;
    public void StartGame()
    {
        AudioManager.instance.PlaySFX(3);
        SceneManager.LoadScene(startScene);
    }
    public void OnApplicationQuit()
    {
        AudioManager.instance.PlaySFX(3);
        Application.Quit();
    }
}
