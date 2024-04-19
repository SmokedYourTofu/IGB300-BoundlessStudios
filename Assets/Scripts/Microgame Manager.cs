using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MicrogameManager : MonoBehaviour
{
    [SerializeField] private MicrogameSO[] microGameSOList;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private float spawnTimer;
    [SerializeField] private GameObject timerGameObject;
    [SerializeField] private bool timerOn;
    private float microChoice;

    private void Start() {
        timerOn = timerGameObject.GetComponent<Timer>().TimerOn;
    }

    private void Update() {
        if (timerOn) {
            if (spawnTimer > 0) {
                spawnTimer -= Time.deltaTime;
            } else {
                SpawnMicroGames();
                spawnTimer = 5;
            }
        }
    }

    void SpawnMicroGames() {
        // random which micro game to spawn
        microChoice = Random.Range(0, 5);
        int selection = ((int)microChoice);

        Debug.Log("Spawn Microgame" + microChoice);

        // if the chosen microgame already exists in scene

        // Spawn microgame at selected spawnpoints (might have to do some more code if there are multiple spawn points)
        GameObject microGame = Instantiate(microGameSOList[selection].microGameObject, spawnPoints[0].position, spawnPoints[0].rotation);
        
    }
}
