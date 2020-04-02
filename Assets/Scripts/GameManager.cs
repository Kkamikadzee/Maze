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
            mazeSpawner.SpawnMaze(8, 8, Vector2.one);

            var gameObjectStartCell = mazeSpawner.gameObjectsMazeCells[mazeSpawner.maze.positionStart.x, mazeSpawner.maze.positionStart.y];
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
