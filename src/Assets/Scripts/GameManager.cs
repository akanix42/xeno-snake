using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

  public static GameManager Instance = null;
  public BoardManager boardManager;

  //private int level = 1;
  private GameData gameData;
  void Awake() {
    Instance = this;
    //if (Instance == null) {
    //Instance = this;
    //}
    //else if (Instance != this) {
    //Destroy(gameObject);
    //return;
    //}

    //DontDestroyOnLoad(gameObject);
    gameData = GameData.Create();
    boardManager = GetComponent<BoardManager>();
    InitGame();
  }
  
  private void InitGame() {
    boardManager.SetupScene(gameData.level);
  }

  // Use this for initialization
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  internal void LoadNextLevel() {
    gameData.level++;
    SceneManager.LoadScene("Level");
    //boardManager.SetupScene(gameData.level);
  }
}
