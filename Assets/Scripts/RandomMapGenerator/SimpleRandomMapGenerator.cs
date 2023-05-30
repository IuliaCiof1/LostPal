using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RandomMapGenerator
{
   public class SimpleRandomMapGenerator : MonoBehaviour
   {
      [SerializeField] private Vector2 startPosition = Vector2.zero;
      [SerializeField] private int iterations = 10; //used for more randomness
      [SerializeField] private int walkLength = 10; //controll the size of the path
      [SerializeField] public bool startRandomlyEachIteration = true;
      [SerializeField] private float width, height;
      [SerializeField] private TilemapVisualizer tilemapVisualizer;
      [SerializeField] private GameObject coinPrefab;
      [SerializeField] private GameObject playerPrefab, treePrefab;
      [SerializeField] private int minNumberOfObstacles=1, maxNumberOfObstacles = 10;
      [SerializeField] private int minNumberOfTrees=1, maxNumberOfTrees = 10;
      public void Start()
      {
         //Endpoints
         float[] widthPos = { startPosition.x, startPosition.x + width };
         float[] heightPos = { startPosition.y - height / 2f, startPosition.y + height / 2f };
         
         HashSet<Vector2> floorPositions = RunRandomWalk(widthPos, heightPos);
         tilemapVisualizer.PaintFloorTiles(floorPositions);
         
         HashSet<Vector2> wallPositions = WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
         WaterGenerator.CreateWater(floorPositions, tilemapVisualizer, widthPos, heightPos);
         
         //spawn obstacle
         tilemapVisualizer.PaintObstacles(minNumberOfObstacles, maxNumberOfObstacles, floorPositions);
         //spawn trees
         InstantiateTrees(floorPositions);
         
         //spawn coin
         Instantiate(coinPrefab, GetRandomFloorPosition(floorPositions), Quaternion.identity);
         //spawn player
         Instantiate(playerPrefab, GetRandomFloorPosition(floorPositions), Quaternion.identity);

         

      }

      public void InstantiateTrees(HashSet<Vector2> floorPositions)
      {
         int range = Random.Range(minNumberOfTrees, maxNumberOfTrees);
         for (int i = 0; i <= maxNumberOfTrees; i++)
         {
            Vector2 pos = GetRandomFloorPosition(floorPositions);
            Instantiate(treePrefab, pos, Quaternion.identity);
            floorPositions.Remove(pos);
         }
      }
      public Vector3 GetRandomFloorPosition(HashSet<Vector2> floorPositions)
      {
         Vector2 pos = Vector2.zero;
         pos = floorPositions.ElementAt(Random.Range(0, floorPositions.Count-1));
         
         //var posit=tilemap.WorldToCell((Vector3)pos);
         //Debug.Log("player " + pos);
         return pos;
      } 
   
      public HashSet<Vector2> RunRandomWalk(float[] widthPos, float[] heightPos)
      {
         var currentPosition = startPosition;
         HashSet<Vector2> floorPositions = new HashSet<Vector2>();

         // //Endpoints
         // float[] widthPos = { startPosition.x, startPosition.x + width };
         // float[] heightPos = { startPosition.y - height / 2f, startPosition.y + height / 2f };
      
         for (int i = 0; i < iterations; i++)
         {
            //create a path
            var path = ProceduralGenerationAlgorithm.SimpleRandomWalk(currentPosition, walkLength, widthPos, heightPos);
        
            floorPositions.UnionWith(path); //copy path positions to floorPositions hash set
         
            if (startRandomlyEachIteration)
            {
               //at each iteration continue path from a random location of the already generated path
               currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }

         }

         return floorPositions;
      }

     
   }
}
