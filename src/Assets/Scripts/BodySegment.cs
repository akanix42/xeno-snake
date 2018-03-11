using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySegment : MovingObject {

  public void SlaveMove(Vector3 newPosition, float moveTime) {
    this.moveTime = moveTime;
    GetComponent<PreviousPosition>().position = transform.position;
    //var newPosition = (Vector3)previousSegment.transform.position;
    Vector3Int directionVector = Vector3Int.FloorToInt(newPosition - transform.position);
    GetComponent<CurrentDirection>().SetDirection(directionVector.x, directionVector.y);

    StartCoroutine(SmoothMovement(newPosition));
  }

  public void Hide() {
    GetComponent<SpriteRenderer>().enabled = false;
    smoothMovementSpriteRenderer.enabled = false;
    showSpriteWhileMoving = false;

  }
  protected override void OnCantMove<T>(T component) {
    
  }

  protected override void OnMove(Vector3 previousPosition) {
    
  }

}
