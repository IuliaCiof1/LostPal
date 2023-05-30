using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SpawnCoinAtRandom
{
    
    public static Vector2 GetValidPosition(HashSet<Vector2> floorPositions)
    {
        return floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
    }
}
