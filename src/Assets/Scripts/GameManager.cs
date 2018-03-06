using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

  public BoardManager boardScript;
  
  private int level = 1;

  void Awake() {
    boardScript = GetComponent<BoardManager>();
    InitGame();
  }

  private void InitGame() {
    boardScript.SetupScene(level);
  }

  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
