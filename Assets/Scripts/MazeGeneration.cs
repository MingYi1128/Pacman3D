
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneration
{


    private int[,] _map;
    private int _generatedPelletCount;
    private List<EntityPellet> _entityPellets;
    private Vector3 _playerSpawnPosition;


    public Vector3 PlayerSpawnPosition => _playerSpawnPosition;
    public List<EntityPellet> Pellets => _entityPellets;
    public int GeneratedPelletCount => this._generatedPelletCount;

    public MazeGeneration(int[,] map, Vector3 playerSpawnPosition, List<EntityPellet> pellets)

    {
        this._map = map;
        this._playerSpawnPosition = playerSpawnPosition;
        this._generatedPelletCount = pellets.Count;
        this._entityPellets = pellets;

    }
}
