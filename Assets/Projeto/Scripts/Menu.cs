using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    public void LoadScene2()
    {
        SceneManager.LoadScene("1Jogador");
    }
    public void LoadScene3()
    {
        SceneManager.LoadScene("2Jogadores");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}