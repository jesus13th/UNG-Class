using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] private Transform tCharacter;
    [SerializeField] private bool FollowCharacterY;
    [SerializeField] private Vector2 offset;

    [SerializeField] private bool limitCamera;
    [ShowIf("limitCamera"), SerializeField] private Vector2 minLimits;
    [ShowIf("limitCamera"), SerializeField] private Vector2 maxLimits;

    void Update() {
        if (limitCamera) {
            transform.position = new Vector3(
                Mathf.Clamp(tCharacter.position.x + offset.x, minLimits.x, maxLimits.x),
                Mathf.Clamp(FollowCharacterY ? tCharacter.position.y + offset.y : transform.position.y, minLimits.y, maxLimits.y),
                transform.position.z
            );
        } else {
            transform.position = new Vector3(
                tCharacter.position.x + offset.x,
                FollowCharacterY ? tCharacter.position.y + offset.y : transform.position.y,
                transform.position.z
            );
        }
    }
}