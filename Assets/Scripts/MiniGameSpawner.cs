using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameSpawner : MonoBehaviour
{
    public GameObject[] miniGames; // Array of mini-game prefabs
    public List<GameObject> activeMiniGames; // List of active mini-games
    public int maxActiveMiniGames = 3; // Maximum number of active mini-games
    public bool spawnOnStart = true; // Whether to spawn mini-games on start
    public GameObject lastInteracted;

    private void Start()
    {
        if (spawnOnStart)
        {
            StartCoroutine(SpawnInitialMiniGame());
        }
    }

    private IEnumerator SpawnInitialMiniGame()
    {
        float initialDelay = Random.Range(2f, 4f);
        yield return new WaitForSeconds(initialDelay);
        SpawnMiniGame();
        Debug.Log("Minigame Spawned");

        StartCoroutine(SpawnAdditionalMiniGames());
    }

    private IEnumerator SpawnAdditionalMiniGames()
    {
        while (activeMiniGames.Count < maxActiveMiniGames)
        {
            float spawnDelay = Random.Range(2f, 6f);
            yield return new WaitForSeconds(spawnDelay);
            SpawnMiniGame();
            Debug.Log("Minigame Spawned");

            // Check if any active mini-game has its NewInteract component disabled
            List<GameObject> toRemove = new List<GameObject>();
            foreach (GameObject miniGame in activeMiniGames)
            {
                NewInteract interact = miniGame.GetComponent<NewInteract>();
                if (interact != null && !interact.enabled)
                {
                    toRemove.Add(miniGame);
                }
            }

            // Remove the inactive mini-games from the active pool
            foreach (GameObject miniGame in toRemove)
            {
                activeMiniGames.Remove(miniGame);
            }
        }
    }

    public void SpawnMiniGame()
    {
        if (activeMiniGames.Count >= maxActiveMiniGames)
        {
            return;
        }

        // Randomly select a mini-game prefab
        GameObject miniGamePrefab = miniGames[Random.Range(0, miniGames.Length)];
        MiniGameTypes.MiniGameType type = GetMiniGameType(miniGamePrefab);

        // Check if the selected mini-game type violates any rules
        if (type == MiniGameTypes.MiniGameType.Urgent && IsUrgentMiniGameActive())
        {
            return;
        }

        // Check if the selected mini-game is already active
        if (activeMiniGames.Contains(miniGamePrefab))
        {
            return;
        }

        // Instantiate and activate the mini-game
        //GameObject miniGame = Instantiate(miniGamePrefab, transform.position, transform.rotation);
        miniGamePrefab.GetComponent<NewInteract>().enabled = true;
        activeMiniGames.Add(miniGamePrefab);
    }

    private MiniGameTypes.MiniGameType GetMiniGameType(GameObject miniGamePrefab)
    {
        NewInteract interactScript = miniGamePrefab.GetComponent<NewInteract>();
        if (interactScript != null)
        {
            return interactScript.miniGameType;
        }
        else
        {
            Debug.LogError("NewInteract script not found on mini-game prefab.");
            return MiniGameTypes.MiniGameType.NonUrgent; // Return a default value or handle the error as needed
        }
    }

    private bool IsUrgentMiniGameActive()
    {
        foreach (GameObject miniGame in activeMiniGames)
        {
            if (GetMiniGameType(miniGame) == MiniGameTypes.MiniGameType.Urgent)
            {
                return true;
            }
        }
        return false;
    }

    public void MiniGameCompleted(GameObject miniGame)
    {
        // Deactivate the completed mini-game
        miniGame.GetComponent<NewInteract>().enabled = false;
        miniGame.GetComponent<Outline>().enabled = false;
        activeMiniGames.Remove(miniGame);
        // Optionally, reintroduce the completed mini-game back into the pool
    }

}
