using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputManagement
{
    public class KeyBoardInput : IPlayerInput
    {
        public Vector2 MoveDirection =>
            new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        public Vector2 RotateDirection => 
            new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        public bool Equip => Input.GetMouseButtonDown(1);
        public bool UnEquip => Input.GetMouseButtonDown(1);
        public bool UseItem => Input.GetKeyDown(KeyCode.LeftShift);
        public bool Avoid => Input.GetKeyDown(KeyCode.LeftShift);
        public bool Attack1 => Input.GetKeyDown(KeyCode.Space);
        public bool Attack2 => Input.GetKeyDown(KeyCode.V);
        public bool Attack3 => Input.GetKeyDown(KeyCode.V);
        public bool Attack4 => Input.GetKey(KeyCode.B);
        public bool LockOnSwitch => Input.GetMouseButtonDown(2);
    }
}
