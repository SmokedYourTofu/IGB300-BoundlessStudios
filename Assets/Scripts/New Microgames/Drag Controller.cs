using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DragController : MonoBehaviour
{
    // Start is called before the first frame update

    private AudioSource mySource;
    private string SceneName;
    void Start()
    {
        SceneName = SceneManager.GetActiveScene().name;
        mySource = this.GetComponent<AudioSource>();
    }

    public void Endgame()
    {
        mySource.Play();
        //do score and such
        Debug.Log("Game Over");
        SceneManager.UnloadSceneAsync(SceneName);
    }
}
