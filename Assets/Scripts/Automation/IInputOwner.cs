using System;
using UnityEngine;

public interface IInputOwner
{
    bool TryInsertInput(ItemData item, int portIndex);
}
