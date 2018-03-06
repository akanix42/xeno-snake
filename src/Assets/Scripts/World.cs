using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

  public GameObject LeftBorder;
  public GameObject RightBorder;
  public GameObject TopBorder;
  public GameObject BottomBorder;

  public struct Size {
    public float Left;
    public float Right;
    public float Top;
    public float Bottom;
  }

  public struct Point2d {
    public int X;
    public int Y;
  }

  public Size size = new Size() {
    Bottom = 0,
    Top = 8,
    Left = 0,
    Right = 8,
  };
  // Use this for initialization
  void Start() {
    //size.Top = TopBorder.t
  }

  public bool CanMoveTo(Point2d newPoint) {
    if (newPoint.X < size.Left || newPoint.X > size.Right) {
      return false;
    }
    if (newPoint.Y < size.Bottom || newPoint.Y > size.Top) {
      return false;
    }

    return true;
  }
}
