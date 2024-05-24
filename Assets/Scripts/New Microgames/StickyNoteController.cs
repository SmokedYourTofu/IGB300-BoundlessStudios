using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class StickyNoteController : MonoBehaviour
{
    public Camera specificCamera;
    public List<GameObject> stickyNotePool;
    public List<GameObject> activeStickyNotes = new List<GameObject>();
    public AudioSource sound;
    public GameObject completeText;

    private MiniGameSpawner mySpawner;

    public void Start()
    {
        mySpawner = FindObjectOfType<MiniGameSpawner>();
        specificCamera = Camera.main;

        // Get a random number of sticky notes to activate (2-4)
        int stickyNoteCount = Random.Range(2, 5);

        // Shuffle the sticky note pool to randomize the selection
        Shuffle(stickyNotePool);

        // Activate the first 'stickyNoteCount' number of sticky notes
        for (int i = 0; i < stickyNoteCount; i++)
        {
            if (i < stickyNotePool.Count)
            {
                GameObject stickyNotePrefab = stickyNotePool[i];
                stickyNotePrefab.SetActive(true); // Activate the GameObject directly
                activeStickyNotes.Add(stickyNotePrefab);
            }
        }

        // Deactivate the remaining sticky notes
        for (int i = stickyNoteCount; i < stickyNotePool.Count; i++)
        {
            stickyNotePool[i].SetActive(false);
        }
    }

    public void Update()
    {
        // Check if there are no more active sticky notes
        Debug.Log(activeStickyNotes.Count);
        if (activeStickyNotes.Count == 0)
        {
            sound.Play();
            StartCoroutine(FinishWait());
        }
    }

    private IEnumerator FinishWait()
    {
        completeText.SetActive(true);
        yield return new WaitForSeconds(1f);
        EndMinigame();
    }

    public void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public void StickyNoteDestroyed(GameObject destroyedStickyNote)
    {
        // Remove the destroyed sticky note from the active list
        activeStickyNotes.Remove(destroyedStickyNote);

        // Check if there are no more active sticky notes
        if (activeStickyNotes.Count == 0)
        {
            EndMinigame();
        }
    }

    public void EndMinigame()
    {
        // End the minigame logic here
        Debug.Log("Minigame ended");
        mySpawner.MiniGameCompleted(mySpawner.lastInteracted);

        GameManager.instance.player.SetActive(true);
        GameManager.instance.camera.SetActive(true);
        GameManager.instance.environment.SetActive(true);
        GameManager.instance.controls.SetActive(true);
        SceneManager.UnloadSceneAsync("Sticky Note");
    }
}