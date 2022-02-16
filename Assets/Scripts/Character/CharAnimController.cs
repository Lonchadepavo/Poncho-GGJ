using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharAnimController : MonoBehaviour {
  GameManager gm_instance_;

  public enum AnimatorBoolParameters {APB_IsRunning, APB_HasJumped, APB_IsPushing};
  string[] animator_parameters_;

  public Animator player_animator3d_;
  public Animator player_animator2d_;

  void Start() {
    gm_instance_ = GameManager.gm_instance_;

    animator_parameters_ = new string[player_animator2d_.parameterCount];
    for (int i = 0; i < player_animator2d_.parameterCount; i++) {
      animator_parameters_[i] = player_animator2d_.GetParameter(i).name;
    }  
  }

  public void SetAnimatorBoolParameter(AnimatorBoolParameters parameter, bool value) {
    if (player_animator3d_.gameObject.activeSelf) 
      player_animator3d_.SetBool(animator_parameters_[(int)parameter], value);

    if (player_animator2d_.gameObject.activeSelf) 
      player_animator2d_.SetBool(animator_parameters_[(int)parameter], value);
  }

}
