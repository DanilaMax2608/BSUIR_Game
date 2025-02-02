using UnityEngine;

public class BlockableObject : MonoBehaviour, IBlockable
{
    public bool IsBlocked { get; private set; }

    public void Block()
    {
        IsBlocked = true;
    }

    public void Unblock()
    {
        IsBlocked = false;
    }
}
