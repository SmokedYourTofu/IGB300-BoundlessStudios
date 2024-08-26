using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class movingBumper : NavigationAgent
{

    //Movement Variables
    public float moveSpeed;
    public float minDistance = 1f;

    //FSM Variables
    public int newState = 0;
    private int currentState = 0;

    [SerializeField] private SpringJoint joint;

    private int[,] DFA = {
        { 0, 1, 2 },
        { 0, 1, 2 },
        { 0, 1, 2 }
    };


    // Use this for initialization
    void Start()
    {
        //Find waypoint graph
        graphNodes = GameObject.FindGameObjectWithTag("waypoint graph").GetComponent<WaypointGraph>();
        //Initial node index to move to
        currentPath.Add(currentNodeIndex);

        joint = GetComponent<SpringJoint>();
    }

    // Update is called once per frame
    void Update()
    {

        currentState = DFA[currentState, newState];

        switch (currentState)
        {
            //Roam
            case 0:
                Roam();
                break;
        }

        Move();
    }

    //Move Enemy
    private void Move()
    {

        if (currentPath.Count > 0)
        {

            //Move towards next node in path
            transform.position = Vector3.MoveTowards(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position, moveSpeed * Time.deltaTime);

            if (joint != null)
            {
                joint.connectedAnchor = transform.position;
            }

            //Increase path index
            if (Vector3.Distance(transform.position, graphNodes.graphNodes[currentPath[currentPathIndex]].transform.position) <= minDistance)
            {

                if (currentPathIndex < currentPath.Count - 1)
                    currentPathIndex++;
            }

            currentNodeIndex = graphNodes.graphNodes[currentPath[currentPathIndex]].GetComponent<LinkedNodes>().index;   //Store current node index
        }
    }

    //FSM Behaviour - Roam - Randomly select nodes to travel to using Greedy Search Algorithm
    private void Roam()
    {
        Debug.Log("Roam");
        if (Vector3.Distance(transform.position, graphNodes.graphNodes[currentPath[currentPath.Count - 1]].transform.position) <= minDistance)
        {
            int randomNode = UnityEngine.Random.Range(0, graphNodes.graphNodes.Length);

            currentPath.Clear();
            greedyPaintList.Clear();
            currentPathIndex = 0;
            currentPath.Add(currentNodeIndex);

            currentPath = GreedySearch(currentPath[currentPathIndex], randomNode, currentPath);

            currentPath.Reverse();
            currentPath.RemoveAt(currentPath.Count - 1);

        }
    }
}
