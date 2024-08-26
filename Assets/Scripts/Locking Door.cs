using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockingDoor : MonoBehaviour
{
    [SerializeField] private Animator mydoor;

    [SerializeField] private float timer = 0f;
    [SerializeField] private int closeInterval;

    public List<string> animationNames = new List<string>();
    private Outline outline;

    private bool open;
    public NewInteract interact;

    // Start is called before the first frame update
    void Start()
    {
        mydoor = this.GetComponent<Animator>();
        open = true;
        outline = GetComponent<Outline>();

        closeInterval = Random.Range(15, 25);
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            timer += Time.deltaTime;
        }

        if (timer >= closeInterval && open)
        {
            mydoor.Play(animationNames[1], 0, 0.0f);
            timer = 0f;
            open = false;
            interact.enabled = true;
            outline.enabled = false;
            closeInterval = Random.Range(15, 25);
        }
    }

    public void MiniGameCompleted()
    {
        // Deactivate the completed mini-game
        interact.enabled = false;
        outline.enabled = false;
        mydoor.Play(animationNames[0], 0, 0.0f);
        open = true;
    }
}
