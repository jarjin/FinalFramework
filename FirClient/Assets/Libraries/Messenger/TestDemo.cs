using UnityEngine;

public class TestDemo {
    void Start() {
        Messenger.AddListener("start game", StartGame);
    }

    void StartGame() {
        Debug.Log("StartGame called in");  //This is the line that would throw an exception
    }

    void StartGameButtonPressed() {
        Messenger.RemoveListener("start game", StartGame);
        Messenger.Broadcast("start game");
    }

    /// <summary>
    /// ---------------------------------------------------------------------
    /// </summary>
    void OnEnable() {
        Messenger.AddListener<GameObject>("prop collected", OnPropCollected);
    }

    void OnDisable() {
        Messenger.RemoveListener<GameObject>("prop collected", OnPropCollected);
    }

    public void OnTriggerEnter(Collider _collider) {
        Messenger.Broadcast<GameObject>("prop collected", _collider.gameObject);
    }

    void OnPropCollected(GameObject gameObj) {
        Debug.LogError(gameObj.name);
    }
}
