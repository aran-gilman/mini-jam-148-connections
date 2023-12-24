using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarPathfinder : IPathfinder
{
    private NavigationMap _navMap;
    private Vector3 _target;

    private class Node
    {
        public Vector3 Position = Vector3.zero;
        public Node CameFrom = null;
        public float GScore = float.PositiveInfinity;
        public float FScore = float.PositiveInfinity;
    }

    public AStarPathfinder(NavigationMap navMap)
    {
        _navMap = navMap;
    }

    // Derived from pseudocode at https://en.wikipedia.org/wiki/A*_search_algorithm
    public Stack<Vector3> CalculatePath(Vector3 start, Vector3 target)
    {
        // This is intended for a 2D game, so ignore the z-axis.
        start.z = 0;
        target.z = 0;

        _target = target;
        if (_navMap == null)
        {
            return new Stack<Vector3>();
        }

        Node startNode = new Node()
        {
            Position = start,
            GScore = 0,
            FScore = Heuristic(start, target)
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
            if (_navMap.HasDirectRoute(node.Position, target))
            {
                Node targetNode = new Node()
                {
                    Position = target,
                    CameFrom = node
                };
                Stack<Vector3> path = new Stack<Vector3>();
                ReconstructPath(targetNode, path);
                return path;
            }
            openNodes.Remove(node);

            foreach (Vector3 neighborPos in _navMap.GetNeighbors(node.Position))
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
                    neighbor.FScore = newGScore + Heuristic(neighborPos, target);
                }
            }
        }
        return new Stack<Vector3>();
    }

    public Stack<Vector3> CalculatePath(Vector3 current, IPathfindingTarget target)
    {
        // This is intended for a 2D game, so ignore the z-axis.
        current.z = 0;
        if (_navMap == null)
        {
            return new Stack<Vector3>();
        }

        Node startNode = new Node()
        {
            Position = current,
            GScore = 0,
            FScore = Heuristic(current, target.NearestPosition(current))
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
                .First();
            Vector3 nearestTarget = target.NearestPosition(node.Position);
            if (_navMap.HasDirectRoute(node.Position, nearestTarget))
            {
                Node targetNode = new Node()
                {
                    Position = nearestTarget,
                    CameFrom = node
                };
                Stack<Vector3> path = new Stack<Vector3>();
                ReconstructPath(targetNode, path);
                return path;
            }
            openNodes.Remove(node);

            foreach (Vector3 neighborPos in _navMap.GetNeighbors(node.Position))
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
                    neighbor.FScore = newGScore
                        + Heuristic(neighborPos, nearestTarget);
                }
            }
        }
        return new Stack<Vector3>();
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

    private float Heuristic(Vector3 current, Vector3 target)
    {
        return Vector3.Distance(current, target);
    }

    private void ReconstructPath(Node node, Stack<Vector3> path)
    {
        path.Push(node.Position);
        if (node.CameFrom != null)
        {
            ReconstructPath(node.CameFrom, path);
        }
    }
}
