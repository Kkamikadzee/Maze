using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maze;
using UnityEngine;

class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject player;

    public Maze.TessellationType Tessellation;
    [Range(0, 16)]
    public int MazeWidth = 5;
    [Range(0, 16)]
    public int MazeHeight = 5;
    [Range(0, 16)]
    public int OuterDiameter = 5;
    [Range(1, 16)]
    public int InnerDiameter = 1;
    [Range(1, 16)]
    public int AmountCellInFirstLayer = 6;
    [Range(0.25f, 2.0f)]
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

        _mazeManager = new Maze.MazeManager(Tessellation, Maze.GenerationAlgorithmEnum.RecursiveBacktracker);

        switch (Tessellation)
        {
            case TessellationType.Orthigonal:
                _mazeManager.CreateMaze(new Vector3(0, 0), CellSize, MazeWidth, MazeHeight);
                break;
            case TessellationType.Theta:
                _mazeManager.CreateMaze(new Vector3(0, 0), CellSize, OuterDiameter, InnerDiameter, AmountCellInFirstLayer, 0);
                break;
        }

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
        _systemInput.Update();
    }
}