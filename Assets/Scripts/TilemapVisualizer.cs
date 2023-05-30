using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilemapVisualizer : MonoBehaviour
{
   [SerializeField] private Tilemap floorTilemap;
   [SerializeField] private Tilemap wallTilemap;
   [SerializeField] private TileBase[] floorTiles;
   [SerializeField] private TileBase wallTiles;

   [SerializeField] private TileBase wallFull,
      wallTop,
      wallRight,
      wallBottom,
      wallLeft,
      wallInnerCornerUpLeft,
      wallInnerCornerUpRight,
      wallInnerCornerDownLeft,
      wallInnerCornerDownRight,
      wallDiagonalCornerDownRight,
      wallDiagonalCornerDownLeft,
      wallDiagonalCornerUpRight,
      wallDiagonalCornerUpLeft;
   public void PaintFloorTiles(IEnumerable<Vector2> floorPositions)
   {
      PaintTiles(floorPositions, floorTilemap, floorTiles);
   }

   public void PaintTiles(IEnumerable<Vector2>positions, Tilemap tilemap, TileBase[] tiles)
   {
      foreach (var position in positions)
      {
         
         PaintSingleTile(tilemap, tiles[Random.Range(0, tiles.Length)], position);
      }
   }

   public void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2 position)
   {
      var tilePosition = tilemap.WorldToCell((Vector3)position);
      tilemap.SetTile(tilePosition, tile);
   }

   public void PaintSingleCornerWall(Vector2 position, string binaryType)
   {
      int typeAsInt = Convert.ToInt32(binaryType, 2);
      TileBase tile = null;

      if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
         tile = wallInnerCornerDownLeft;
      else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
         tile = wallInnerCornerDownRight;
      else if (WallTypesHelper.wallInnerCornerUpLeft.Contains(typeAsInt))
         tile = wallInnerCornerUpLeft;
      else if (WallTypesHelper.wallInnerCornerUpRight.Contains(typeAsInt))
         tile = wallInnerCornerUpRight;
      else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
         tile = wallDiagonalCornerDownLeft;
      else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
         tile = wallDiagonalCornerDownRight;
      else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
         tile = wallDiagonalCornerUpLeft;
      else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
         tile = wallDiagonalCornerUpRight;
      else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
         tile = wallFull;
      else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
         tile = wallBottom;
      
      if(tile!=null)
         PaintSingleTile(wallTilemap, tile, position);
   }
   
   public void PaintSingleBasicWall(Vector2 position, string binaryType)
   {
     
      int typeAsInt = Convert.ToInt32(binaryType, 2);
      TileBase tile = null;

      if (WallTypesHelper.wallTop.Contains(typeAsInt))
         tile = wallTop;
      else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
         tile = wallRight;
      else if (WallTypesHelper.wallBottom.Contains(typeAsInt))
         tile = wallBottom;
      else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
         tile = wallLeft;
      else if (WallTypesHelper.wallFull.Contains(typeAsInt))
         tile = wallFull;
      
      if(tile!=null)
         PaintSingleTile(wallTilemap, tile, position);
   }
   
}
