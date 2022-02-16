using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour {
  GameManager gm_instance_;
  CharacterController cc_;

  [HideInInspector]
  public CharAnimController anim_controller_;

  public float movement_speed_;
  public float jump_speed_;
  public float gravity_apply_speed_;

  public AnimationCurve jump_curve_;

  public float gravity_;
  public bool apply_gravity_;

  public bool is_grounded_;
  public bool is_jumping_;
  public bool is_moving_;

  bool can_jump_;

  float jump_pressing_time_;
  float jump_falling_time_;
  float time_jumping = 0.0f;

  Vector3 movement_axis_ = new Vector3();
  Vector3 jumping_axis_ = new Vector3();

  Quaternion last_rotation_;

  public GameObject floor_raycast_origin_;

  public AudioSource audio_source_;
  public AudioClip walking_clip_, jumping_clip_;

  void Awake() {
    cc_ = GetComponent<CharacterController>();
    anim_controller_ = GetComponent<CharAnimController>();
  }

  void Start() {
    gm_instance_ = GameManager.gm_instance_;
    audio_source_ = GetComponent<AudioSource>();
    can_jump_ = true;
  }

  void Update() {
    GetMovementInputs(gm_instance_.camera_mode_);
    MovementJump();
    ApplyGravity();
    Movement();
    CheckFloor();
    GetInteraction();
  }

  void GetMovementInputs(GameManager.GameMode game_mode) {
    if (game_mode == GameManager.GameMode.Mode2D) {
      movement_axis_.x = Input.GetAxis("Horizontal");
      movement_axis_.z = 0.0f;
    }

    else {
      movement_axis_.x = Input.GetAxis("Horizontal");
      movement_axis_.z = Input.GetAxis("Vertical");
    }
  }

  void Movement() {
    if (!GameManager.gm_instance_.lock_movement_ && !GameManager.gm_instance_.lock_game_) {
      cc_.Move(movement_axis_ * Time.deltaTime * movement_speed_);

      if (movement_axis_.x != 0 || movement_axis_.z != 0) {
        anim_controller_.SetAnimatorBoolParameter(CharAnimController.AnimatorBoolParameters.APB_IsRunning, true);

        Vector3 last_direction = movement_axis_;

        last_rotation_ = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (new Vector3(-last_direction.z, 0.0f, last_direction.x)), Time.deltaTime * 40f);
        transform.rotation = last_rotation_;


        if (!is_jumping_) {
          audio_source_.clip = walking_clip_;
          if (!audio_source_.isPlaying) {
            audio_source_.Play();
          }
        }
      } else {
        anim_controller_.SetAnimatorBoolParameter(CharAnimController.AnimatorBoolParameters.APB_IsRunning, false);

        if (audio_source_.isPlaying) audio_source_.Stop();
      }
    }
  }

  void MovementJump() {
    if (Input.GetKeyDown(KeyCode.Space)) {
      if (!is_jumping_ && is_grounded_ && can_jump_) {
        is_jumping_ = true;
        jumping_axis_.y = gravity_;

        anim_controller_.SetAnimatorBoolParameter(CharAnimController.AnimatorBoolParameters.APB_HasJumped, true);

        can_jump_ = false;

        if (audio_source_.clip != jumping_clip_) audio_source_.Stop();
        audio_source_.clip = jumping_clip_;
        if (!audio_source_.isPlaying) {
          audio_source_.Play();
        }       
      }
    }

    if (is_jumping_) {
      if (jump_pressing_time_ == 0.0f) jump_pressing_time_ = Time.time;
      jump_falling_time_ = 0.0f;

      time_jumping = Time.time - jump_pressing_time_;
      
      if (time_jumping >= 1.0f) is_jumping_ = false;
      
      jumping_axis_.y = jump_curve_.Evaluate(time_jumping);
      if (!GameManager.gm_instance_.lock_movement_ && !GameManager.gm_instance_.lock_game_) {
        cc_.Move(jumping_axis_ * Time.deltaTime * jump_speed_);
      }

      StartCoroutine(CancelJump());

    } else {
      if (jump_falling_time_ == 0.0f) jump_falling_time_ = time_jumping;
      jump_pressing_time_ = 0.0f;

      if (!is_grounded_) {
        float time_falling = Time.time - jump_falling_time_;
        if (time_falling >= 1.0f) jump_falling_time_ = 0.0f;

        jumping_axis_.y = jump_curve_.Evaluate(time_falling);
        if (!GameManager.gm_instance_.lock_movement_ && !GameManager.gm_instance_.lock_game_) {
          cc_.Move(jumping_axis_ * Time.deltaTime * jump_speed_);
        }
      }

    }
  }

  IEnumerator CancelJump() {
    yield return null;
    is_jumping_ = !is_grounded_;
  }

  IEnumerator CooldownJump() {
    yield return new WaitForSeconds(1.5f);
    can_jump_ = true;
  }

  void CheckFloor() {
    RaycastHit hit;
    Debug.DrawRay(floor_raycast_origin_.transform.position, -floor_raycast_origin_.transform.up * 0.5f, Color.red);
    if (Physics.Raycast(floor_raycast_origin_.transform.position, -floor_raycast_origin_.transform.up, out hit, 0.5f)){
      is_grounded_ = hit.transform.GetComponent<IJumpable>() != null;
      if (is_grounded_) { 
        anim_controller_.SetAnimatorBoolParameter(CharAnimController.AnimatorBoolParameters.APB_HasJumped, false);
      }
      StartCoroutine(CooldownJump());
    } else {
      is_grounded_ = false;
    }
  }

  void ApplyGravity() {
    if (apply_gravity_) {
      if (!is_grounded_) {
        movement_axis_.y = Mathf.Lerp(movement_axis_.y, gravity_, gravity_apply_speed_ * Time.deltaTime);

      } else {
        movement_axis_.y = 0.0f;

      }
    }
  }

  void GetInteraction() {
    if (Input.GetKeyDown(KeyCode.E)) {
      RaycastHit hit;
      Debug.DrawRay(gm_instance_.player_.transform.position, gm_instance_.player_.transform.right * 5);

      if (Physics.Raycast(gm_instance_.player_.transform.position, gm_instance_.player_.transform.right, out hit, 1.0f)) {
        if (hit.collider.gameObject.GetComponent<IInteract>() != null) {
          if (!hit.transform.tag.Equals("NPC")) {
            hit.collider.gameObject.GetComponent<IInteract>().Action();
          }
        }
      }
    }
  }

}

