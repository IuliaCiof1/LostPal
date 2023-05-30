using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class SimpleRandomMapGenerator : MonoBehaviour
{
   [SerializeField] private Vector2 startPosition = Vector2.zero;
   [SerializeField] private int iterations = 10; //used for more randomness
   [SerializeField] private int walkLength = 10; //controll the size of the path
   [SerializeField] public bool startRandomlyEachIteration = true;
   [SerializeField] private int width, height;
   [SerializeField] private TilemapVisualizer tilemapVisualizer;
   [SerializeField] private GameObject coinPrefab;
   [SerializeField] private GameObject playerPrefab;
   public void Start()
   {
      HashSet<Vector2> floorPositions = RunRandomWalk();
      tilemapVisualizer.PaintFloorTiles(floorPositions);
      WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
      
      //spawn coin
      Instantiate(coinPrefab, GetRandomFloorPosition(floorPositions), Quaternion.identity);
      //spawn player
      Instantiate(playerPrefab, GetRandomFloorPosition(floorPositions), Quaternion.identity);
   }

   public Vector2 GetRandomFloorPosition(HashSet<Vector2> floorPositions)
   {
      Vector2 pos = Vector2.zero;
      for (int i = 0; i < 10; i++)
      {
         pos = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
      }

      return pos;
   } 
   
   public HashSet<Vector2> RunRandomWalk()
   {
      var currentPosition = startPosition;
      HashSet<Vector2> floorPositions = new HashSet<Vector2>();

      //Endpoints
      //float[] widthPos = { startPosition.x - width / 2f, startPosition.x + width / 2f };
      float[] widthPos = { startPosition.x, startPosition.x + width };
      float[] heightPos = { startPosition.y - height / 2f, startPosition.y + height / 2f };
      
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
