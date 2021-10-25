using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : MarioMovement
{
    protected override bool facingRight { get; set; } = true;
     
    
    protected override void ProccessInput()
    {
        if (isAlive  )
        {

        if (Input.GetKey(KeyCode.A))
            MoveLeft();
        if (Input.GetKey(KeyCode.D))
            MoveRight();
        if (Input.GetKeyDown(KeyCode.A))
            BurstLeft();
        if (Input.GetKeyDown(KeyCode.D))
            BurstRight();
        if (Input.GetKey(KeyCode.S))
            MoveDown();

        if (Input.GetKeyDown(KeyCode.Q))
            GroundRoll(false);
        if (Input.GetKeyDown(KeyCode.E))
            GroundRoll(true);

        if (Input.GetKeyDown(KeyCode.X))
        {

            StartChargingBamb();
           // ThrowBombSpriteLine();

        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            ThrowBomb();
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        }
    }

}
