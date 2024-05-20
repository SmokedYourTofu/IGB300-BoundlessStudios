using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameSpawner : MonoBehaviour
{
    public GameObject[] miniGames; // Array of mini-game prefabs
    public List<GameObject> activeMiniGames; // List of active mini-games
    public List<GameObject> inactiveMiniGames; // List of inactive mini-games
    public int maxActiveMiniGames = 3; // Maximum number of active mini-games
    public bool spawnOnStart = true; // Whether to spawn mini-games on start
    public GameObject lastInteracted;
    public GameObject indicatorPrefab; // Reference to the indicator prefab

    private void Start()
    {
        // Initialize the inactive mini-games list with all mini-games
        inactiveMiniGames = new List<GameObject>(miniGames);

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
        while (true)
        {
            if (activeMiniGames.Count < maxActiveMiniGames && inactiveMiniGames.Count > 0)
            {
                float spawnDelay = Random.Range(2f, 6f);
                yield return new WaitForSeconds(spawnDelay);
                SpawnMiniGame();
                Debug.Log("Minigame Spawned");
            }

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

            // Remove the inactive mini-games from the active pool and add them back to the inactive pool
            foreach (GameObject miniGame in toRemove)
            {
                activeMiniGames.Remove(miniGame);
                inactiveMiniGames.Add(miniGame);
            }

            yield return null;
        }
    }

    public void SpawnMiniGame()
    {
        if (activeMiniGames.Count >= maxActiveMiniGames || inactiveMiniGames.Count == 0)
        {
            return;
        }

        // Randomly select a mini-game prefab from the inactive list
        GameObject miniGamePrefab = inactiveMiniGames[Random.Range(0, inactiveMiniGames.Count)];
        MiniGameTypes.MiniGameType type = GetMiniGameType(miniGamePrefab);

        // Check if the selected mini-game type violates any rules
        if (type == MiniGameTypes.MiniGameType.Urgent && IsUrgentMiniGameActive())
        {
            return;
        }

        // Activate the mini-game
        miniGamePrefab.GetComponent<NewInteract>().enabled = true;
        activeMiniGames.Add(miniGamePrefab);
        inactiveMiniGames.Remove(miniGamePrefab);

        // Instantiate and position the indicator above the mini-game
        GameObject indicator = Instantiate(indicatorPrefab, miniGamePrefab.transform);
        indicator.transform.localPosition = new Vector3(0, 2, 0); // Adjust the position as needed
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

        // Remove the indicator
        Transform indicator = miniGame.transform.Find(indicatorPrefab.name);
        if (indicator != null)
        {
            Destroy(indicator.gameObject);
        }

        activeMiniGames.Remove(miniGame);
        inactiveMiniGames.Add(miniGame);
    }
}
