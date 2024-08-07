using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
    [SerializeField] private string sceneName;
    [SerializeField] private float duration;

    private IEnumerator Start() {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(sceneName);
    }
}