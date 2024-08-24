using Unity.VisualScripting;
using UnityEngine;

namespace Player.States {
    public class Die : State
    {

        private PlayerController controller;

        public Die(PlayerController controller) : base("Die")
        {
            this.controller = controller;
        }
        public override void Enter()
        {
            base.Enter();

        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {

        }


        }

    }
