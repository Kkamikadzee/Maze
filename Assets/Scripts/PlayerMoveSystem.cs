using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class PlayerMoveSystem
    {
        private InputModel inputModel;
        private GameObject player;
        private Rigidbody2D playerRB;

        float maxSpeed = 3f;
        float acceleration = 0.5f;
        float friction = 0.25f;


        public PlayerMoveSystem(InputModel inputModel, GameObject player)
        {
            this.inputModel = inputModel;
            this.player = player;
            this.playerRB = player.GetComponent<Rigidbody2D>();
        }

        public void FixedUpdate()
        {
            if(playerRB.velocity.magnitude >= 0.05f)
            {
                playerRB.velocity -= playerRB.velocity.normalized * friction;
            }
            else
            {
                playerRB.velocity = Vector2.zero;
            }

            if (inputModel.Horizontal != 0 || inputModel.Vertical != 0)
            {
                if (playerRB.velocity.magnitude <= maxSpeed)
                {
                    var delta = new Vector2(inputModel.Horizontal, inputModel.Vertical);

                    playerRB.velocity += delta * acceleration;
                }
            }
        }
    }
}
