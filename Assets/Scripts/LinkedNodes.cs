using UnityEngine;
using System.Collections;

public class LinkedNodes : MonoBehaviour {

	public int index;

	public GameObject[] linkedNodeObjects;

	public int[] linkedNodesIndex;

	//had to comment this out because it was breaking everything for no reason. If you want to set up a roomba you have to do it manually for now

	// Use this for initialization
	//void Start () {
	//	//Get the correct index for each linked Node
	//	linkedNodesIndex = new int[linkedNodeObjects.Length];

	//	for (int i = 0; i < linkedNodesIndex.Length; i++) {

	//		linkedNodesIndex[i] = linkedNodeObjects[i].GetComponent<LinkedNodes>().index;
	//	}
	//}
	
	// Update is called once per frame
	void Update () {
		//Draw lines between each connected waypoint
		foreach(GameObject linkedNode in linkedNodeObjects) {
			Debug.DrawLine (transform.position, linkedNode.transform.position,  Color.green);
		}
	}
}
