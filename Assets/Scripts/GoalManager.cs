using System.Collections;

using UnityEngine;

public class GoalManager : MonoBehaviour {
    [SerializeField] private GameObject gWin;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            StartCoroutine(WinCoroutine());
        }
    }
    private IEnumerator WinCoroutine() {
        gWin.SetActive(true);
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(4f);
        Time.timeScale = 1;
        GameManager.Instance.GameOver();
    }
}
