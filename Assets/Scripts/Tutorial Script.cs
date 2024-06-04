using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class TutorialScript : MonoBehaviour
{
    public GameObject[] miniGames; // Array of mini-game prefabs
    public List<GameObject> activeMiniGames; // List of active mini-games
    public List<GameObject> inactiveMiniGames; // List of inactive mini-games
    public int maxActiveMiniGames = 3; // Maximum number of active mini-games
    public bool spawnOnStart = true; // Whether to spawn mini-games on start
    public GameObject lastInteracted;
    public GameObject indicatorPrefab; // Reference to the indicator prefab
    public TMP_Text tutorialText;
    public GameObject[] arrows;

    private Vector3 playerPos;
    private int minigameCounter = 0;
    private bool tutorialcomplete = false;
    private bool gameActive = false;
    public GameObject timer;

    private void Start()
    {
        // Initialize the inactive mini-games list with all mini-games
        inactiveMiniGames = new List<GameObject>(miniGames);
        playerPos = GameManager.instance.player.transform.position;

        if (spawnOnStart)
        {
            StartCoroutine(SpawnInitialMiniGame());
        }
    }

    private void Update()
    {
        if (tutorialcomplete == false)
        {
            TutorialProgress();
        }
    }

    private IEnumerator SpawnInitialMiniGame()
    {
        SpawnMiniGame(0);
        Debug.Log("Minigame Spawned");
        yield return null;
    }

    private IEnumerator SpawnAdditionalMiniGames()
    {
        while (true)
        {
            if (activeMiniGames.Count < maxActiveMiniGames && inactiveMiniGames.Count > 0)
            {
                float spawnDelay = Random.Range(2f, 6f);
                yield return new WaitForSeconds(spawnDelay);
                SpawnMiniGame(0);
                Debug.Log("Minigame Spawned");
            }

            // Check if any active mini-game has its NewInteract component disabled
            List<GameObject> toRemove = new List<GameObject>();
            foreach (GameObject miniGame in activeMiniGames)
            {
                TutorialInteract interact = miniGame.GetComponent<TutorialInteract>();
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

    public void SpawnMiniGame(int choice)
    {
        if (activeMiniGames.Count >= maxActiveMiniGames || inactiveMiniGames.Count == 0)
        {
            return;
        }

        // Randomly select a mini-game prefab from the inactive list
        GameObject miniGamePrefab = inactiveMiniGames[choice];
        MiniGameTypes.MiniGameType type = GetMiniGameType(miniGamePrefab);

        // Check if the selected mini-game type violates any rules
        if (type == MiniGameTypes.MiniGameType.Urgent && IsUrgentMiniGameActive())
        {
            return;
        }

        // Activate the mini-game
        miniGamePrefab.GetComponent<TutorialInteract>().enabled = true;
        activeMiniGames.Add(miniGamePrefab);
        inactiveMiniGames.Remove(miniGamePrefab);

        // Instantiate and position the indicator above the mini-game without stretching
        Renderer renderer = miniGamePrefab.GetComponent<Renderer>();
        if (renderer != null)
        {
            Vector3 topOfMesh = renderer.bounds.max;
            Vector3 indicatorPosition = new Vector3(topOfMesh.x, topOfMesh.y + 2, topOfMesh.z); // Adjust the height as needed
            GameObject indicator = Instantiate(indicatorPrefab, indicatorPosition, Quaternion.identity);
            indicator.transform.localScale = indicatorPrefab.transform.localScale; // Maintain the original size

            // Store the reference to the indicator in the mini-game's NewInteract component
            TutorialInteract interactScript = miniGamePrefab.GetComponent<TutorialInteract>();
            if (interactScript != null)
            {
                interactScript.indicator = indicator;
            }
        }
        else
        {
            Debug.LogError("Renderer not found on mini-game prefab.");
        }
    }

    private MiniGameTypes.MiniGameType GetMiniGameType(GameObject miniGamePrefab)
    {
        TutorialInteract interactScript = miniGamePrefab.GetComponent<TutorialInteract>();
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

    public void MiniGameCompleted(GameObject miniGame, bool isSuccessful)
    {
        // Deactivate the completed mini-game
        miniGame.GetComponent<TutorialInteract>().enabled = false;
        miniGame.GetComponent<Outline>().enabled = false;

        // Remove the indicator
        TutorialInteract interactScript = miniGame.GetComponent<TutorialInteract>();
        if (interactScript != null && interactScript.indicator != null)
        {
            Destroy(interactScript.indicator);
        }

        // Update the game multiplier based on success or failure
        ScoreManager.Instance.UpdateMultiplier(isSuccessful);

        activeMiniGames.Remove(miniGame);
        inactiveMiniGames.Add(miniGame);
        minigameCounter++;
        gameActive = false;
    }

    private void TutorialProgress()
    {
        if (minigameCounter == 0 && gameActive == false)
        {
            if (Vector3.Distance(playerPos, GameManager.instance.player.transform.position) > 5f)
            {
                arrows[0].SetActive(false);
                SpawnMiniGame(0);
                gameActive = true;
                tutorialText.text = "Oh No! A cyber security breach has appeared! Quickly, move over to it and tap on it to begin fixing it!";
            }
        }

        if (minigameCounter == 1 && gameActive == false)
        {
            StartCoroutine(ScoreExample());
            //SpawnMiniGame(0);
            //arrows[1].SetActive(false);
            //gameActive = true;
            //tutorialText.text = "Oh No! A different breach has appeared! They will appear quickly so you'd better be fast to fix it!";
        }

        if (minigameCounter == 2 && gameActive == false)
        {
            arrows[2].SetActive(true);
            tutorialText.text = "Looks like you finished it just in time. Every department requires you to work for a specific amount of time before your job is done";
            timer.GetComponent<Timer>().TimeLeft = 10f;
            tutorialcomplete = true;
        }
    }

    private IEnumerator ScoreExample()
    {
        arrows[1].SetActive(true);
        tutorialText.text = "Look! Your score went up! The faster you fix an issue, the faster it will grow!";
        yield return new WaitForSeconds(4f);

        SpawnMiniGame(0);
        arrows[1].SetActive(false);
        gameActive = true;
        tutorialText.text = "Oh No! A different breach has appeared! They will appear quickly so you'd better be fast to fix it!";
    }
}
