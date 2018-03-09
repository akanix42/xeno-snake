using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartsOutOfBoundsComponent : MonoBehaviour {
  private void OnTriggerExit2D(Collider2D collision) {
    if (!collision.gameObject.GetComponent<BlockingObject>()) {
      return;
    }
    if (collision.gameObject.GetComponent<OuterWallComponent>()) {
      GetComponent<BoxCollider2D>().isTrigger = false;
    }
  }
}
