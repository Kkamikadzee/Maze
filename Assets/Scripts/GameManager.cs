using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject player;

    [Range(0, 32)]
    public int MazeWidth = 5;
    [Range(0, 32)]
    public int MazeHeight = 5;
    [Range(0.0f, 2.0f)]
    public float CellSize = 1f;

    private InputModel _inputModel;
    private SystemInput _systemInput;

    private PlayerMoveSystem _playerMove;

    Maze.MazeManager _mazeManager;

    private void Start()
    {
        _inputModel = new InputModel();
        _systemInput = new SystemInput(_inputModel);

        _playerMove = new PlayerMoveSystem(_inputModel, player);

        _mazeManager = new Maze.MazeManager(Maze.TessellationType.Theta, Maze.GenerationAlgorithmEnum.RecursiveBacktracker);

        _mazeManager.CreateMaze(new Vector3(0, 0), CellSize, 5, 1, 6, 1);

        player.transform.position = new Vector3()
        {
            x = 0,
            y = 0
        };
    }
    private void FixedUpdate()
    {
        _playerMove.FixedUpdate();
    }
    private void Update()
    {
        if (Input.GetAxis("Fire1") > 0.5f) //KEK
        {
            _mazeManager.DestroyMaze();
            _mazeManager.CreateMaze(new Vector3(0, 0), CellSize, MazeWidth, MazeHeight);
        }
        _systemInput.Update();
    }
}