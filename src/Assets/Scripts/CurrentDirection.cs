using UnityEngine;

public enum Direction {
  Left = 90,
  Right = 270,
  Up = 180,
  Down = 0,
}

public class CurrentDirection : MonoBehaviour {
  public Direction Direction { get; private set; }

  public void SetDirection(int xDir, int yDir) {
    if (xDir == -1) {
      Direction = Direction.Left;
    }
    else if (xDir == 1) {
      Direction = Direction.Right;
    }
    else if (yDir == -1) {
      Direction = Direction.Up;
    }
    else if (yDir == 1) {
      Direction = Direction.Down;
    }

    transform.rotation = Quaternion.Euler(0, 0, (float)Direction);
  }
}
