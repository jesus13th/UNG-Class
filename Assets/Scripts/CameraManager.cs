using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] private Transform tCharacter;

    void Update() => transform.position = new Vector3(tCharacter.position.x, transform.position.y, transform.position.z);
}