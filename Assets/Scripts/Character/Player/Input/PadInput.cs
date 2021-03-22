using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

namespace InputManagement
{
    public class PadInput : IPlayerInput
    {
        public Vector2 MoveDirection => 
            new Vector2(Input.GetAxis("L_Stick Horizontal"), Input.GetAxis("L_Stick Vertical"));
        public Vector2 RotateDirection =>
            new Vector2(Input.GetAxis("R_Stick Horizontal"), Input.GetAxis("R_Stick Vertical"));

        public bool Equip => Input.GetButtonDown("Y");
        public bool UnEquip => Input.GetButtonDown("X");
        public bool UseItem => UnEquip;
        public bool Avoid => Input.GetButtonDown("A");
        public bool Attack1 => Input.GetButtonDown("B");
        public bool Attack2 => Input.GetButtonDown("Y");
        public bool Attack3 => Input.GetButtonDown("Y");
        public bool Attack4 => Input.GetAxis("LT") > 0.2f && Input.GetAxis("RT") > 0.2f;
        public bool LockOnSwitch => Input.GetButtonDown("R_Stick Button");
    }
}

