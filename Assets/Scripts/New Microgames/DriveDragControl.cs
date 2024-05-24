using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DriveDragControl : MonoBehaviour
{
    // Start is called before the first frame update

    private AudioSource mySource;
    private string SceneName;

    private MiniGameSpawner mySpawner;
    void Start()
    {
        mySpawner = FindObjectOfType<MiniGameSpawner>();
        mySource = this.GetComponent<AudioSource>();
    }

    private IEnumerator FinishWait()
    {
        yield return new WaitForSeconds(1f);
        Endgame();
    }

    public void Endgame()
    {
        mySource.Play();
        //do score and such
        Debug.Log("Game Over");
        SceneManager.LoadScene("Drive Smash", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("Drive Unplug");
    }

    public void OnTriggerEnter(Collider other)
    {
        // Check if the sticky note has collided with the 'bin' game object
        if (other.gameObject.CompareTag("Bin"))
        {
            Destroy(gameObject);
            // Remove the destroyed sticky note from the active list using the instance of StickyNoteController
            StartCoroutine(FinishWait());
            SceneManager.UnloadSceneAsync("Drive Smash");
        }
    }
}
