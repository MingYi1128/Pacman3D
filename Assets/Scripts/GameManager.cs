using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance) return _instance;
            _instance = FindObjectOfType<GameManager>();
            if (_instance) return _instance;
            else throw new ApplicationException("");
        }
    }

    [Header("Parameters")]
    [SerializeField]
    private GameObject _ghostSpawn;

    [Header("Prefabs")] 
    [SerializeField]
    private EntityGhost _ghostPrefab;
    
    
    [Header("Entity")]
    [SerializeField]
    private EntityPlayer _player;
    private List<EntityGhost> _ghosts;
    private List<EntityPellet> _pellets;
    
    [SerializeField]
    private GameUI _gameUI;
    
    
    private MazeGeneration _mazeGeneration;
    private MazeGenerator _mazeGenerator;


    public void DestroyPellet(EntityPellet pellet)
    {
        Destroy(pellet.gameObject);
        _pellets.Remove(pellet);
        _gameUI.setPelletCount(_pellets.Count, _mazeGeneration.GeneratedPelletCount);
    }

    public EntityPlayer GetPlayer()
    {
        return _player;
    }

    public void Restart()
    {
        var mazeGeneration = _mazeGenerator.GenerateMaze();
        Debug.Log("Spawning Player at " + mazeGeneration.PlayerSpawnPosition);
        _player.SetPosition(mazeGeneration.PlayerSpawnPosition);
        _player.OnGameReset();

        foreach (var pellet in _pellets)
        {
            Destroy(pellet.gameObject);
        }
        _pellets.Clear();
        _pellets.AddRange(mazeGeneration.Pellets);
        
        foreach (EntityGhost ghost in _ghosts)
        {
            Destroy(ghost.gameObject);
        }
        _ghosts.Clear();
        for (int i = 0; i < 4; i++)
        {
            var ghost = Instantiate(_ghostPrefab);
            ghost.SetPosition(_ghostSpawn.transform.position);
            ghost.Activate();
            Debug.Log("Spawning Ghost at " + _ghostSpawn.transform.position);
            _ghosts.Add(ghost);
        }
        
    }

    private void Awake()
    {
        _mazeGenerator = GetComponent<MazeGenerator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Restart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
