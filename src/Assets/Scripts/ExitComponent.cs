using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitComponent : MonoBehaviour {

  public GameObject exitWall;

  private bool isExitVisible = false;

  void Update () {
    ShowExitWhenApplicable();
  }

  void ShowExitWhenApplicable() {
    if (isExitVisible) {
      return;
    }

    if (GameManager.Instance.boardManager.foodRemaining > 0) {
      return;
    }

    /**
     * Once all the food is gone, disable the wall and display the exit.
     **/
    isExitVisible = true;
    GetComponent<SpriteRenderer>().enabled = true;
    GetComponent<BoxCollider2D>().enabled = true;
    exitWall.GetComponent<BoxCollider2D>().enabled = false;
    exitWall.GetComponent<SpriteRenderer>().enabled = false;
  }
  
  private void OnTriggerExit2D(Collider2D collision) {
    if (collision.gameObject.GetComponent<PlayerComponent>() && collision.gameObject.GetComponent<TailSegmentComponent>()) {
      GameManager.Instance.LoadNextLevel();
    }
    collision.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    HideSegment(collision);
    ParalyzePlayer(collision);

  }

  private void HideSegment(Collider2D collision) {
    var bodySegment = collision.gameObject.GetComponent<BodySegment>();
    if (!bodySegment) {
      return;
    }
    bodySegment.Hide();
  }

  private void ParalyzePlayer(Collider2D collision) {
    var playerComponent = collision.gameObject.GetComponent<Player>();
    if (playerComponent == null) {
      return;
    }
    playerComponent.isParalyzed = true;
  }
}
