using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance => instance ?? (instance = FindObjectOfType<GameManager>());

    [Header("GameOver")]
    [SerializeField] private string mainMenuScene;
    [SerializeField] private CharacterMovement player;
    [SerializeField] private float bottomLimit = -5f;

    [Header("Coins")]
    public bool canGrabCoins = false;
    public string coinTag = "Coin";
    public int coins = 0;
    [SerializeField] private TMP_Text coinTexts;
    [SerializeField] private bool coinPlaySound = false;
    [SerializeField] private AudioClip coinClip;

    [Header("Timer")]
    [SerializeField] private bool isTimer = false;
    [SerializeField] private float time = 120;
    [SerializeField] private TMP_Text timeText;

    private void Awake() => instance = this;

    void Update() {
        if (isTimer) {
            time -= Time.deltaTime;
            timeText.text = $"Tiempo: {time:0}";
            if (time <= 0)
                GameOver();
        }

        if (player.transform.position.y < -5) {
            GameOver();
        }
    }
    public void GrabCoin(GameObject g) {
        coins++;
        coinTexts.text = $"Monedas: {coins}";
        if (coinPlaySound)
            AudioManager.Instance.PlaySound(coinClip);
        Destroy(g);
    }
    public void GameOver() {
        SceneManager.LoadScene(mainMenuScene);
    }
}