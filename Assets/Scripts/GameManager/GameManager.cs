using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  public enum GameMode {Mode2D, Mode3D};

  public static GameManager gm_instance_ = null;
  public GameMode camera_mode_;

  public bool lock_game_; //BLOQUEA TOTALMENTE AL PERSONAJE
  public bool lock_movement_; //BLOQUEA SOLO EL MOVIMIENTO DEL PERSONAJE
  public bool lock_interaction_; //BLOQUEA SOLO LA INTERACCIÓN DEL PERSONAJE

  public bool camera_mode_locked_;

  public delegate void OnCameraModeChange();
  public event OnCameraModeChange onCameraChangeEvent;
  public GameObject player_;
  public Animator player_animator3d_;
  public Animator player_animator2d_;

  public int coin_count_ = 0;

  void Awake() { 
    if (gm_instance_ == null) gm_instance_ = this;

    camera_mode_locked_ = false;
  }

  void Update() {
    GoToMenu();
  }

  public void SwapCameraMode() {
    if (camera_mode_ == GameMode.Mode2D) { 
      camera_mode_ = GameMode.Mode3D;
      player_animator3d_.gameObject.SetActive(true);
      player_animator2d_.gameObject.SetActive(false);
    }

    else if (camera_mode_ == GameMode.Mode3D) {
      camera_mode_ = GameMode.Mode2D;
      player_animator3d_.gameObject.SetActive(false);
      player_animator2d_.gameObject.SetActive(true);
    }

    camera_mode_locked_ = true;
    lock_game_ = true;

    if (player_animator3d_.gameObject.activeSelf) player_animator3d_.SetBool("is_running", false);
    if (player_animator2d_.gameObject.activeSelf) player_animator2d_.SetBool("is_running", false);

    player_.GetComponent<CharacterMovement>().audio_source_.Stop();

    onCameraChangeEvent();
  }

  void GoToMenu() {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
  }
}


