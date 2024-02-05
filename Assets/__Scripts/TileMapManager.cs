using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    static public Tile[] Delver_Tiles;
    static public Dictionary<char, Tile> Coll_Tile_DICT;
    [Header("Inscribed")]
    public Tilemap visualMap;
    public Tilemap collisionMap;
    private TileBase[] visualTileBaseArray;
    private TileBase[] CollTileBaseArray;
    // Start is called before the first frame update
    void Awake()
    {
        LoadTiles();
    }
    private void Start()
    {
        ShowTiles();
    }
    void LoadTiles()
    {
        int num;
        Tile[] tempTiles = Resources.LoadAll<Tile>("Tiles_Visual");
        Delver_Tiles = new Tile[tempTiles.Length];
        for(int i=0; i< tempTiles.Length; i++)
        {
            string[] bits = tempTiles[i].name.Split('_');
            if(int.TryParse(bits[1],out num))
            {
                Delver_Tiles[num] = tempTiles[i];
            }
            else
            {
                Debug.LogError("Failed to parse num of: " + tempTiles[i].name);
            }
        }
        Debug.Log("Parsed " + Delver_Tiles.Length + " tiles into Tiles_Visual");
        //Collsions
        tempTiles = Resources.LoadAll<Tile>("Tiles_Collision");
        Coll_Tile_DICT = new Dictionary<char, Tile>();
        for(int i=0; i < tempTiles.Length; i++)
        {
            char c = tempTiles[i].name[0];
            Coll_Tile_DICT.Add(c, tempTiles[i]);
        }
        Debug.Log("Coll_Tile_DICT contains: " + Coll_Tile_DICT.Count + " tiles");
    }
    void ShowTiles()
    {
        visualTileBaseArray = GetMapTiles();
        visualMap.SetTilesBlock(MapInfo.Get_Map_Bounds(), visualTileBaseArray);
        //collisions
        CollTileBaseArray = GetCollisionTiles();
        collisionMap.SetTilesBlock(MapInfo.Get_Map_Bounds(), CollTileBaseArray);
        
    }
    public TileBase[] GetMapTiles()
    {
        int tileNum;
        Tile tile;
        TileBase[] mapTiles = new TileBase[MapInfo.W * MapInfo.H];
        for(int y = 0; y < MapInfo.H; y++)
        {
            for(int x = 0; x < MapInfo.W; x++)
            {
                tileNum = MapInfo.Map[x, y];
                tile = Delver_Tiles[tileNum];
                mapTiles[y * MapInfo.W + x] = tile;
            }
        }
        return mapTiles;
    }
    public TileBase[] GetCollisionTiles()
    {
        Tile tile;
        int tileNum;
        char tileChar;
        TileBase[] mapTiles = new TileBase[MapInfo.W * MapInfo.H];
        for (int y = 0; y < MapInfo.H; y++)
        {
            for(int x =0; x < MapInfo.W; x++)
            {
                tileNum = MapInfo.Map[x, y];
                tileChar = MapInfo.COLLISIONS[tileNum];
                tile = Coll_Tile_DICT[tileChar];
                mapTiles[y * MapInfo.W + x] = tile;

            }
        }
        return mapTiles;
    }
}
