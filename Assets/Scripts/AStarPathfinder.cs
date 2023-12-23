using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarPathfinder : IPathfinder
{
    private Stack<Vector3> _path = new Stack<Vector3>();
    private Vector3 _target;

    private class Node
    {
        public Vector3 Position = Vector3.zero;
        public Node CameFrom = null;
        public float GScore = float.PositiveInfinity;
        public float FScore = float.PositiveInfinity;
    }

    // Derived from pseudocode at https://en.wikipedia.org/wiki/A*_search_algorithm
    public void CalculatePath(Vector3 start, Vector3 target)
    {
        _path.Clear();
        NavigationMap navMap = Object.FindObjectOfType<NavigationMap>();
        if (navMap == null)
        {
            return;
        }

        Node startNode = new Node()
        {
            Position = start,
            GScore = 0,
            FScore = Heuristic(start)
        };
        List<Node> openNodes = new List<Node>()
        {
            startNode
        };
        List<Node> allNodes = new List<Node>()
        {
            startNode
        };

        while (openNodes.Count > 0)
        {
            Node node = openNodes
                .OrderBy(n => n.FScore)
                .FirstOrDefault();
            if (VectorApproximately(node.Position, target))
            {
                ReconstructPath(node);
                return;
            }
            openNodes.Remove(node);

            foreach (Vector3 neighborPos in navMap.GetNeighbors(node.Position))
            {
                Node neighbor = FindNode(neighborPos, allNodes);
                if (neighbor == null)
                {
                    neighbor = new Node() { Position = neighborPos };
                    allNodes.Add(neighbor);
                    openNodes.Add(neighbor);
                }

                float newGScore = node.GScore +
                    Vector3.Distance(node.Position, neighborPos);
                if (newGScore < neighbor.GScore)
                {
                    neighbor.CameFrom = node;
                    neighbor.GScore = newGScore;
                    neighbor.FScore = newGScore + Heuristic(neighborPos);
                }
            }
        }
    }

    public Vector3 PopNextNode()
    {
        if (_path.TryPop(out Vector3 node))
        {
            return node;
        }
        return _target;
    }

    private static bool VectorApproximately(Vector3 a, Vector3 b)
    {
        return Mathf.Approximately(a.x, b.x)
            && Mathf.Approximately(a.y, b.y);
    }

    private static Node FindNode(Vector3 position, IEnumerable<Node> nodes)
    {
        foreach (Node node in nodes)
        {
            if (VectorApproximately(node.Position, position))
            {
                return node;
            }
        }
        return null;
    }

    private float Heuristic(Vector3 current)
    {
        return Vector3.Distance(current, _target);
    }

    private void ReconstructPath(Node node)
    {
        _path.Push(node.Position);
        if (node.CameFrom != null)
        {
            ReconstructPath(node.CameFrom);
        }
    }
}
