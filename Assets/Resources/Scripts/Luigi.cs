using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luigi : MarioMovement
{
    protected override bool facingRight { get; set; } = false;
    protected override void ProccessInput()
    {
        if(isAlive)
        {

        if (Input.GetKey(KeyCode.LeftArrow))
            MoveLeft();
        
        if (Input.GetKey(KeyCode.RightArrow))
            MoveRight();
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            BurstLeft();
        if (Input.GetKeyDown(KeyCode.RightArrow))
            BurstRight();
        if (Input.GetKeyUp(KeyCode.LeftArrow))
            DampenLeft();
        if (Input.GetKeyUp(KeyCode.RightArrow))
            DampenRight();

        if (Input.GetKey(KeyCode.DownArrow))
            MoveDown();

        if (Input.GetKeyDown(KeyCode.Keypad1))
            GroundRoll(false);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            GroundRoll(true);

        if (Input.GetKeyDown(KeyCode.Keypad0))
            StartChargingBamb();
        if (Input.GetKeyUp(KeyCode.Keypad0))
            ThrowBomb();

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Jump();
        }
    }

}
