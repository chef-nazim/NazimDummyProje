using System;

namespace gs.chef.game.enums
{
    [Serializable]
    public enum PlaySound
    {
        None = -1,
        Win = 0,
        Fail = 1,
        CantTap = 2,
        Tap = 3,
        Connect = 4,
        CloseItem = 5,
        MoveSlot = 6,
        ButtonClick = 7,
        TimeOut = 8,
        KeyMove = 9,
        KeyOpen = 10,
    }
}