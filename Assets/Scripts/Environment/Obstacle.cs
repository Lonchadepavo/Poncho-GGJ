using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IJumpable {
  float saved_z_;
  float new_z_;

  bool is_centering_;

  float time_elapsed_ = 0.0f;
  float lerp_duration_ = 2.0f;

  void Start() {
    GameManager.gm_instance_.onCameraChangeEvent += CenterObstacle;
    is_centering_ = false;
    saved_z_ = 0.0f;
    new_z_ = 0.0f;
  }

  void Update() {
    if (is_centering_) {
      if (time_elapsed_ < lerp_duration_) {
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, new_z_, time_elapsed_/lerp_duration_));
        time_elapsed_ += Time.deltaTime;

      } else {
        is_centering_ = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, new_z_);
        time_elapsed_ = 0.0f;
      }
    }
  }

  void CenterObstacle() {
    if (transform.parent == null)  {
      is_centering_ = true;
      float aux_z_ = saved_z_;
      saved_z_ = transform.position.z;
      new_z_ = aux_z_;
    } else {
      if (!transform.parent.tag.Equals("Player")) {
        is_centering_ = true;
        float aux_z_ = saved_z_;
        saved_z_ = transform.position.z;
        new_z_ = aux_z_;
      }
    }
  }
}
