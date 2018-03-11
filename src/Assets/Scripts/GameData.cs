using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour {
  public int level = 1;

  private static GameData instance;
  public static GameData Create() {
    if (instance == null) {
      instance = new GameData();
    }

    return instance;
  }
}
