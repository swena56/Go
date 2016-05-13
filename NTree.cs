using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

delegate void TreeVisitor(Node nodeData);


class Node
{
    Vector2 pos = new Vector2();
    string owner = "empty";
    Vector2 left = new Vector2();
    Vector2 right = new Vector2();
    Vector2 up = new Vector2();
    Vector2 down = new Vector2();
    public Node(Vector4 node, Vector2 pos)
    {

    }
}
    
class NTree
    {

        private List<List<Vector2>> blackPieces = new List<List<Vector2>>();
        private List<List<Vector2>> whitePieces = new List<List<Vector2>>();
        private List<Vector2> blackPlays = new List<Vector2>();
        private List<Vector2> whitePlays = new List<Vector2>();
        private LinkedList<NTree> children;

        public NTree(Node node, List<Vector2> blackPlays, List<Vector2> whitePlays)
        {
            this.blackPlays = blackPlays;
            this.whitePlays = whitePlays;
            
        }

        string whichPlayerOwns(Vector2 pos)
        {
            if (blackPlays.Contains(pos))
                return "black";
            else if (whitePlays.Contains(pos))
                return "white";
            else
                return "empty";
        }

    List<Vector2> getConnectingSpots(Vector2 pos)
    {
        int boardSize = 16;
        List<Vector2> adjcentSpots = new List<Vector2>();

        if (pos.x > 1)
            adjcentSpots.Add(new Vector2(pos.x - 1, pos.y));

        if (pos.x < (boardSize - 1))
            adjcentSpots.Add(new Vector2(pos.x + 1, pos.y));

        if (pos.y > 1)
            adjcentSpots.Add(new Vector2(pos.x, pos.y - 1));

        if (pos.y < (boardSize - 1))
            adjcentSpots.Add(new Vector2(pos.x, pos.y + 1));

        // Debug.Log("onboardLocation: " + pos + " has " + adjcentSpots.Count + " adjcent positions");

        return adjcentSpots;
    }

    List<Vector2> getPiecesThatConnectWith(Vector2 pos)
    {
        List<Vector2> list = new List<Vector2>();

        if (whichPlayerOwns(pos) == "black")
        {
            list.Add(pos);

            Queue<Vector2> queue = new Queue<Vector2>();
            foreach (Vector2 spot in getConnectingSpots(pos))
                queue.Enqueue(spot);

            do
            {
                Vector2 checkMe = queue.Dequeue();

                if (whichPlayerOwns(checkMe) == "black" && !list.Contains(checkMe))
                {
                    list.Add(checkMe);
                    foreach (Vector2 newPos in getConnectingSpots(checkMe))
                        if (whichPlayerOwns(checkMe) == "black")
                            queue.Enqueue(newPos);
                }
            }
            while (queue.Count > 0);
        }
        else if (whichPlayerOwns(pos) == "white")
        {
            list.Add(pos);

            Queue<Vector2> queue = new Queue<Vector2>();
            foreach (Vector2 spot in getConnectingSpots(pos))
                queue.Enqueue(spot);

            do
            {
                Vector2 checkMe = queue.Dequeue();

                if (whichPlayerOwns(checkMe) == "white" && !list.Contains(checkMe))
                {
                    list.Add(checkMe);
                    foreach (Vector2 newPos in getConnectingSpots(checkMe))
                        if (whichPlayerOwns(checkMe) == "white")
                            queue.Enqueue(newPos);
                }
            }
            while (queue.Count > 0);
        }

        return list;
    }
    List<Vector2> getConnectedEmptySpots(Vector2 pos)
    {
        List<Vector2> list = new List<Vector2>();
        Queue<Vector2> queue = new Queue<Vector2>();
        if (whichPlayerOwns(pos) == "empty")
        {
            list.Add(pos);


            foreach (Vector2 spot in getConnectingSpots(pos))
                queue.Enqueue(spot);

            Debug.Log("start");
            do
            {
                Vector2 checkMe = queue.Dequeue();

                if (whichPlayerOwns(checkMe) == "empty" && !list.Contains(checkMe))
                {
                    list.Add(checkMe);
                    foreach (Vector2 newPos in getConnectingSpots(checkMe))
                        if (whichPlayerOwns(newPos) == "empty")
                            queue.Enqueue(newPos);
                }
            }
            while (queue.Count > 0);
        }


        return list;
    }
    public void AddChild(Node data)
        {
          
        }

        public NTree GetChild(int i)
        {
            foreach (NTree n in children)
                if (--i == 0)
                    return n;
            return null;
        }

        public void Traverse(NTree node, TreeVisitor visitor)
        {
            //visitor(node.data);
            foreach (NTree kid in node.children)
                Traverse(kid, visitor);
        }
    }

