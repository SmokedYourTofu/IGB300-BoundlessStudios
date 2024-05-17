using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragController : MonoBehaviour
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

    public void Endgame()
    {
        mySource.Play();
        //do score and such
        Debug.Log("Game Over");
        mySpawner.MiniGameCompleted(mySpawner.lastInteracted);

        GameManager.instance.player.SetActive(true);
        GameManager.instance.camera.SetActive(true);
        GameManager.instance.environment.SetActive(true);
        GameManager.instance.controls.SetActive(true);
        Debug.Log(SceneName);
        SceneManager.UnloadSceneAsync("Printer");
    }
}
