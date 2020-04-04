using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject player;
        [SerializeField]
        private GameObject cellPrefab;

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
            mazeSpawner.SpawnMaze(MazeWidth, MazeHeight, new Vector2(CellSize, CellSize));

            var gameObjectStartCell = mazeSpawner.gameObjectsMazeCells[Mathf.RoundToInt(mazeSpawner.maze.PositionStart.x), Mathf.RoundToInt(mazeSpawner.maze.PositionStart.y)];
            Vector2 sizeStartCell = gameObjectStartCell.GetComponent<GameObjectMazeCell>().size;
            player.transform.position = new Vector3()
            {
                x = gameObjectStartCell.transform.position.x + sizeStartCell.x / 2,
                y = gameObjectStartCell.transform.position.y + sizeStartCell.y / 2
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
}
