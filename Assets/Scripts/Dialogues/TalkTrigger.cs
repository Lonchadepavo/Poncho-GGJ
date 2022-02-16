using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkTrigger : MonoBehaviour {

    void OnTriggerStay(Collider other) {
        if (other.transform.tag.Equals("Player")) {
            if (GameManager.gm_instance_.camera_mode_ == GameManager.GameMode.Mode2D) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    transform.parent.GetComponent<Talkable>().Action();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.transform.tag.Equals("Player")) {
            if (GameManager.gm_instance_.camera_mode_ == GameManager.GameMode.Mode2D) {
                if (Input.GetKeyDown(KeyCode.E)) {
                    transform.parent.GetComponent<Talkable>().Action();
                }
            }
        }
    }

}
