using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using static PlacementHelper;

public class TileHelper : MonoBehaviour
{
    public GameObject[] middleTiles, straightTiles, CornerTiles, ThreeWayTiles, FourWayTiles, EndTiles;
    Dictionary<Vector3Int, GameObject> tileDictionary = new Dictionary<Vector3Int, GameObject>();
    HashSet<Vector3Int> fixTileCandidates = new HashSet<Vector3Int>();

    public List<Vector3Int> GetRoadPositions()
    {
        return tileDictionary.Keys.ToList();
    }

    public void PlaceTilePositions(Vector3 startPosition, Vector3Int direction, int length)
    {
        var rotation = Quaternion.identity;
        if (direction.x == 0)
        {
            rotation = Quaternion.Euler(0, 90, 0);
        }
        for (int i = 0; i < length; i++)
        {
            var position = Vector3Int.RoundToInt(startPosition + direction * i * 2);
            if (tileDictionary.ContainsKey(position))
            {
                continue;
            }
            var tile = Instantiate(GetRandomTile(straightTiles), position, rotation, transform);
            tileDictionary.Add(position, tile);
            if (i == 0 || i == length - 1)
            {
                fixTileCandidates.Add(position);
            }
        }
    }

    public Tuple<Vector3Int, Vector3Int> PlaceMiddleTile(Vector3Int startPosition)
    {
        var middleTile = Instantiate(GetRandomTile(middleTiles), startPosition, Quaternion.identity, transform);

        var corridorEntrance1 = startPosition + new Vector3Int(0, 0, 2);
        var corridorEntrance2 = startPosition + new Vector3Int(0, 0, -2);

        tileDictionary.Add(startPosition, middleTile);
        tileDictionary.Add(startPosition + new Vector3Int(2, 0, 0), middleTile);
        tileDictionary.Add(startPosition + new Vector3Int(-2, 0, 0), middleTile);

        tileDictionary.Add(corridorEntrance1, middleTile);
        tileDictionary.Add(corridorEntrance1 + new Vector3Int(2,0,0)  , middleTile);
        tileDictionary.Add(corridorEntrance1 + new Vector3Int(-2,0,0)  , middleTile);

        tileDictionary.Add(corridorEntrance2, middleTile);
        tileDictionary.Add(corridorEntrance2 + new Vector3Int(2, 0, 0), middleTile);
        tileDictionary.Add(corridorEntrance2 + new Vector3Int(-2, 0, 0), middleTile);

        return new Tuple<Vector3Int, Vector3Int>(corridorEntrance1, corridorEntrance2);
    }

    public void FixRoad()
    {
        foreach (var position in fixTileCandidates)
        {
            List<Direction> neighbourDirections = PlacementHelper.FindNeighbour(position, tileDictionary.Keys);

            Quaternion rotation = Quaternion.identity;

            if (neighbourDirections.Count == 1)
            {
                Destroy(tileDictionary[position]);
                if (neighbourDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                tileDictionary[position] = Instantiate(GetRandomTile(EndTiles), position, rotation, transform);
            }
            else if (neighbourDirections.Count == 2)
            {
                if (
                    neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Down)
                    || neighbourDirections.Contains(Direction.Right) && neighbourDirections.Contains(Direction.Left)
                    )
                {
                    continue;
                }
                Destroy(tileDictionary[position]);
                if (neighbourDirections.Contains(Direction.Up) && neighbourDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Right) && neighbourDirections.Contains(Direction.Down))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Down) && neighbourDirections.Contains(Direction.Left))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                tileDictionary[position] = Instantiate(GetRandomTile(CornerTiles), position, rotation, transform);
            }
            else if (neighbourDirections.Count == 3)
            {
                Destroy(tileDictionary[position]);
                if (neighbourDirections.Contains(Direction.Right)
                    && neighbourDirections.Contains(Direction.Down)
                    && neighbourDirections.Contains(Direction.Left)
                    )
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                }
                else if (neighbourDirections.Contains(Direction.Down)
                    && neighbourDirections.Contains(Direction.Left)
                    && neighbourDirections.Contains(Direction.Up))
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (neighbourDirections.Contains(Direction.Left)
                    && neighbourDirections.Contains(Direction.Up)
                    && neighbourDirections.Contains(Direction.Right))
                {
                    rotation = Quaternion.Euler(0, -90, 0);
                }
                tileDictionary[position] = Instantiate(GetRandomTile(ThreeWayTiles), position, rotation, transform);
            }
            else
            {
                Destroy(tileDictionary[position]);
                tileDictionary[position] = Instantiate(GetRandomTile(FourWayTiles), position, rotation, transform);
            }
        }
    }

    private GameObject GetRandomTile(GameObject[] tileArray)
    {
        return tileArray[UnityEngine.Random.Range(0, tileArray.Length)];
    }

    public void Reset()
    {
        foreach (var item in tileDictionary.Values)
        {
            Destroy(item);
        }
        tileDictionary.Clear();
        fixTileCandidates = new HashSet<Vector3Int>();
    }
}