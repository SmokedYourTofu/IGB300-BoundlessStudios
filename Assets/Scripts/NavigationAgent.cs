using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class NavigationAgent : MonoBehaviour {

    //Navigation Variables
    public WaypointGraph graphNodes;
    public List<int> openList = new List<int>();
    public List<int> closedList = new List<int>();
    public List<int> currentPath = new List<int>();
    public List<int> greedyPaintList = new List<int>();
    public int currentPathIndex = 0;
    public int currentNodeIndex = 0;

    public Dictionary<int, int> cameFrom = new Dictionary<int, int>();

    // Use this for initialization
    void Start () {
        //Find waypoint graph
        graphNodes = GameObject.FindGameObjectWithTag("waypoint graph").GetComponent<WaypointGraph>();

        //Initial node index to move to
        currentPath.Add(currentNodeIndex);
    }

    //A-Star Search
    public List<int> AStarSearch(int start, int goal) {
        
        //Code here

        openList.Clear();
        closedList.Clear();
        cameFrom.Clear();

        openList.Add(start);

        float gScore = 0;
        float fScore = gScore + Heuristic(start, goal);

        while (openList.Count > 0)
        {
            int currentNode = bestOpenListFScore(start, goal);

            if (currentNode == goal)
            {
                return ReconstructPath(cameFrom, currentNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            for (int i = 0; i < graphNodes.graphNodes[currentNode].GetComponent<LinkedNodes>().linkedNodesIndex.Length; i++)
            {
                int thisNeighbourNode= graphNodes.graphNodes[currentNode].GetComponent<LinkedNodes>().linkedNodesIndex[i];

                if (!closedList.Contains(thisNeighbourNode))
                {
                    float tentativeGScore = Heuristic(start, currentNode)  + Heuristic(currentNode, thisNeighbourNode);

                    if (!openList.Contains(thisNeighbourNode) || tentativeGScore < gScore)
                    {
                        openList.Add(thisNeighbourNode);
                    }

                    if (!cameFrom.ContainsKey(thisNeighbourNode))
                    {
                        cameFrom.Add(thisNeighbourNode, currentNode);

                        gScore = tentativeGScore;
                        fScore = Heuristic(start, thisNeighbourNode) + Heuristic(thisNeighbourNode, goal);
                    }
                }
            }
        }

        return null;
    }

    public class GreedyChildren : IComparable<GreedyChildren>
    {
        public int childID { get; set; }
        public float childHScore { get; set; }

        public GreedyChildren(int childrenID, float childrenHScore)
        {
            this.childID = childrenID;
            this.childHScore = childrenHScore;
        }

        public int CompareTo(GreedyChildren other)
        {
            return this.childHScore.CompareTo(other.childHScore);
        }
    }


    //Greedy Search
    public List<int> GreedySearch(int currentNode, int goal, List<int> path) {

        //Code here
        if (!greedyPaintList.Contains(currentNode)) 
        {
            greedyPaintList.Add(currentNode);
        }

        List<GreedyChildren> thisNodesChildren = new List<GreedyChildren>();

        for (int i = 0; i < graphNodes.graphNodes[currentNode].GetComponent<LinkedNodes>().linkedNodesIndex.Length; i++)
        {
            thisNodesChildren.Add(new GreedyChildren(graphNodes.graphNodes[currentNode].GetComponent<LinkedNodes>().linkedNodesIndex[i], Heuristic(graphNodes.graphNodes[currentNode].GetComponent<LinkedNodes>().linkedNodesIndex[i], goal)));
        }

        thisNodesChildren.Sort();

        for (int i = 0; i < thisNodesChildren.Count - 1; i++)
        {
            if (!greedyPaintList.Contains(thisNodesChildren[i].childID))
            {
                greedyPaintList.Add(thisNodesChildren[i].childID);

                if (thisNodesChildren[i].childID == goal)
                {
                    path.Add(thisNodesChildren[i].childID);
                    return path;
                }

                path = GreedySearch(thisNodesChildren[i].childID, goal, path);

                if (path != null)
                {
                    path.Add(thisNodesChildren[i].childID);
                    return path;
                }
            }
        }

        return path;
    }

    public float Heuristic(int a, int b)
    {
        return Vector3.Distance(graphNodes.graphNodes[a].transform.position, graphNodes.graphNodes[b].transform.position);
    }

    public int bestOpenListFScore(int start, int goal)
    {
        int bestIndex = 0;

        for(int i = 0; i < openList.Count; i++)
        {
            if ((Heuristic(openList[i], start) + Heuristic(openList[i], goal)) < (Heuristic(openList[bestIndex], start) + Heuristic(openList[bestIndex], goal)))
            {
                bestIndex = i;
            }
        }

        int bestNode = openList[bestIndex];
        return bestNode;
    }

    public List<int> ReconstructPath(Dictionary<int, int>  CF, int current)
    {

        List<int> finalPath = new List<int>();

        finalPath.Add(current);

        while (CF.ContainsKey(current))
        {
            current = CF[current];
            finalPath.Add(current);
        }

        finalPath.Reverse();

        return finalPath;
    }
}
