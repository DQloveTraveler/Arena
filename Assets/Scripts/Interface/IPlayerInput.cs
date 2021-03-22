using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInput
{
    Vector2 MoveDirection { get; }
    Vector2 RotateDirection { get; }

    bool Equip { get; }
    bool UnEquip { get; }
    bool UseItem { get; }
    bool Avoid { get; }
    bool Attack1 { get; }
    bool Attack2 { get; }
    bool Attack3 { get; }
    bool Attack4 { get; }
    bool LockOnSwitch { get; }

}
