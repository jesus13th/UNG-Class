using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] private string gameScene = "Game";
    public void StartGame() => SceneManager.LoadScene(gameScene);
    public void QuitGame() => Application.Quit();
}