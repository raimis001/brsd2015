using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class PathFinder
{
    public static Path<TilePoint> FindPath(
		TilePoint start,
		TilePoint destination)
    {
		var closed = new HashSet<TilePoint>();
		var queue = new PriorityQueue<double, Path<TilePoint>>();
		queue.Enqueue(0, new Path<TilePoint>(start));

        while (!queue.IsEmpty)
        {
            var path = queue.Dequeue();

            if (closed.Contains(path.LastStep))
                continue;
            if (path.LastStep.Equals(destination))
                return path;

            closed.Add(path.LastStep);
			//Debug.Log("path " + path.LastStep);
			foreach (TilePoint n in path.LastStep.AllNeighbours)
            {
                double d = distance(path.LastStep, n);
                var newPath = path.AddStep(n, d);
                queue.Enqueue(newPath.TotalCost + estimate(n, destination), 
                    newPath);
            }
        }

        return null;
    }

	static double distance(TilePoint tile1, TilePoint tile2)
    {
        return 1;
    }

	static double estimate(TilePoint tile, TilePoint destTile)
    {
        float dx = Mathf.Abs(destTile.x - tile.x);
        float dy = Mathf.Abs(destTile.y - tile.y);
        int z1 = -(tile.x + tile.y);
        int z2 = -(destTile.x + destTile.y);
        float dz = Mathf.Abs(z2 - z1);

        return Mathf.Max(dx, dy, dz);
    }
}