using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySegment : MovingObject {

  public void SlaveMove(GameObject previousSegment) {
    GetComponent<PreviousPosition>().position = transform.position;
    var newPosition = (Vector3)previousSegment.transform.position;

    StartCoroutine(SmoothMovement(newPosition));
  }
  protected override void OnCantMove<T>(T component) {
    
  }

  protected override void OnMove(Vector3 previousPosition) {
    
  }
}
