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

    public void OnPlayerReady()
    {
        SpawnGhosts();
        UpdateUI();
    }

    public void OnPlayerEatPellet(EntityPellet pellet)
    {
        Destroy(pellet.gameObject, 0.1f);
        _pellets.Remove(pellet);
        UpdateUI();
    }

    public void OnPlayerDamaged()
    {
        DespawnGhosts();
        UpdateUI();
    }

    public void OnPlayerKilled()
    {
        Restart();
    }

    public EntityPlayer GetPlayer()
    {
        return _player;
    }

    public void UpdateUI()
    {
        _gameUI.setLife(_player.LifeRemaining);
        _gameUI.setPelletCount(_pellets.Count, _mazeGeneration.GeneratedPelletCount);
        _gameUI.setLevel(1);
    }

    public void Restart()
    { 
        _mazeGeneration = _mazeGenerator.GenerateMaze();
        Debug.Log("Spawning Player at " + _mazeGeneration.PlayerSpawnPosition);
        _player.SetPosition(_mazeGeneration.PlayerSpawnPosition);
        _player.OnGameReset();
        
        ApplyPellets();
        UpdateUI();
    }

    public void ApplyPellets()
    {
        if (_pellets == null)
        {
            _pellets = new List<EntityPellet>();
        }
        
        foreach (var pellet in _pellets)
        {
            Destroy(pellet.gameObject);
        }
        _pellets.Clear();
        _pellets.AddRange(_mazeGeneration.Pellets);
    }

    public void DespawnGhosts()
    {
        if (_ghosts == null)
        {
            _ghosts = new List<EntityGhost>();
        }
        
        foreach (EntityGhost ghost in _ghosts)
        {
            Destroy(ghost.gameObject);
        }
        _ghosts.Clear();
    }

    public void SpawnGhosts()
    {
        if (_ghosts == null)
        {
            _ghosts = new List<EntityGhost>();
        }
        
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
        _player.OnPlayerDamaged += OnPlayerDamaged;
        _player.OnPlayerKilled += OnPlayerKilled;
        _player.OnPlayerEatPellet += OnPlayerEatPellet;
        _player.OnPlayerReady += OnPlayerReady;
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
