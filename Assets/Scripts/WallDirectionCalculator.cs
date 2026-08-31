
using System;
using UnityEngine;

public enum WallDirection { Up, Down , Left, Right , UpRight, UpLeft, DownLeft , DownRight }
public class WallDirectionCalculator
{
    public WallDirection GetWallDirection(Vector2Int pos, int width, int height)
    {
        if (pos.x == 0 && pos.y == height - 1)
        {
            return WallDirection.UpLeft;
        }
        else if (pos.x == width - 1 && pos.y == height - 1)
        {
            return WallDirection.UpRight;
        }
        else if (pos.x == 0 && pos.y == 0)
        {
            return WallDirection.DownLeft;
        }
        else if (pos.x == width - 1 && pos.y == 0)
        {
            return WallDirection.DownRight;
        }
        else if (pos.y == height - 1)
        {
            return WallDirection.Up;
        }
        else if (pos.y == 0)
        {
            return WallDirection.Down;
        }
        else if (pos.x == 0)
        {
            return WallDirection.Left;
        }
        else if (pos.x == width - 1)
        {
            return WallDirection.Right;
        }
        else
        {
            throw new InvalidOperationException($"테두리가 아닌 좌표: {pos}");
        }
    }
}
