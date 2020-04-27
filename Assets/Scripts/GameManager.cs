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
    [SerializeField]
    public GameObject cellPrefab;

    [Range(0, 32)]
    public int MazeWidth = 5;
    [Range(0, 32)]
    public int MazeHeight = 5;
    [Range(0.0f, 2.0f)]
    public float CellSize = 1f;

    private InputModel inputModel;
    private SystemInput systemInput;

    private PlayerMoveSystem playerMove;

    MazeSpawner mazeSpawner;

    private void Start()
    {
        inputModel = new InputModel();
        systemInput = new SystemInput(inputModel);

        playerMove = new PlayerMoveSystem(inputModel, player);

        mazeSpawner = new MazeSpawner(cellPrefab);
        var sizeCell = new Vector2(CellSize, CellSize);
        //mazeSpawner.SpawnOrthogonalMaze(MazeWidth, MazeHeight, sizeCell);
        mazeSpawner.SpawnThetaMaze(4, 1, 4);

        var coordinateStart = mazeSpawner.CoordinateStart;
        player.transform.position = new Vector3()
        {
            x = coordinateStart.x + sizeCell.x / 2,
            y = coordinateStart.y + sizeCell.y / 2
        };
    }
    private void FixedUpdate()
    {
        playerMove.FixedUpdate();
    }
    private void Update()
    {
        systemInput.Update();
    }
}