using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public GameObject player;
    public GameObject camera;
    public GameObject environment;
    public GameObject controls;

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this);
        } else {
            instance = this;
        }
    }

    private void Start() {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
}
