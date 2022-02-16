using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour, IJumpable, IInteract {
  GameManager gm_instance_;
  public enum MovableType {NotMovable, AlwaysMovable, Movable2D, Movable3D};
  public MovableType movable_type_;

  public bool grabbed = false;
  Rigidbody rb_;

  void Awake() {
    rb_ = GetComponent<Rigidbody>();
  }

  void Start() {
    gm_instance_ = GameManager.gm_instance_;  
    GameManager.gm_instance_.onCameraChangeEvent += CheckCameraMode;  
  }

  public void Action() {
    GameManager.GameMode mode = gm_instance_.camera_mode_;

    if ((movable_type_ == MovableType.Movable2D || movable_type_ == MovableType.AlwaysMovable) && mode == GameManager.GameMode.Mode2D) {
      GrabObject();
    }

    if ((movable_type_ == MovableType.Movable3D || movable_type_ == MovableType.AlwaysMovable) && mode == GameManager.GameMode.Mode3D) {
      GrabObject();
    }
  }

  void GrabObject() {
    if (grabbed) {
      grabbed = false;
      gameObject.transform.parent = null;
      GetComponent<Rigidbody>().isKinematic = false;

      gm_instance_.player_.GetComponent<CharacterMovement>().anim_controller_.SetAnimatorBoolParameter(CharAnimController.AnimatorBoolParameters.APB_IsPushing, false);

    } else {
      grabbed = true;
      gameObject.transform.parent = gm_instance_.player_.transform;
      GetComponent<Rigidbody>().isKinematic = true;
      transform.rotation = Quaternion.identity;

      gm_instance_.player_.GetComponent<CharacterMovement>().anim_controller_.SetAnimatorBoolParameter(CharAnimController.AnimatorBoolParameters.APB_IsPushing, true);
    }    
  } 

  void CheckCameraMode() {
    GameManager.GameMode mode = gm_instance_.camera_mode_;

    if (grabbed) {      
      gm_instance_.player_.GetComponent<CharacterMovement>().anim_controller_.SetAnimatorBoolParameter(CharAnimController.AnimatorBoolParameters.APB_IsPushing, true);

      if ((movable_type_ == MovableType.Movable2D) && mode != GameManager.GameMode.Mode2D) {
        grabbed = false;
        gameObject.transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;

        gm_instance_.player_.GetComponent<CharacterMovement>().anim_controller_.SetAnimatorBoolParameter(CharAnimController.AnimatorBoolParameters.APB_IsPushing, false);
      }

      else if ((movable_type_ == MovableType.Movable3D) && mode != GameManager.GameMode.Mode3D) {
        grabbed = false;
        gameObject.transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;

        gm_instance_.player_.GetComponent<CharacterMovement>().anim_controller_.SetAnimatorBoolParameter(CharAnimController.AnimatorBoolParameters.APB_IsPushing, false);
      } 
    } 
  }     
}
