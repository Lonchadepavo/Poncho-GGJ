using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MatrixBlender))]
public class CameraSwitch : MonoBehaviour {
  public Camera cam;
  private Matrix4x4   ortho,
                      perspective;
  public float        fov     = 60f,
                      near    = .3f,
                      far     = 1000f,
                      orthographicSize = 50f;
  private float       aspect;
  private MatrixBlender blender;
  private bool        orthoOn;

  public float perspective_to_ortho_;
  public float ortho_to_perspective_;

  AudioSource audio_source_;
  public AudioClip change2d_3d_, change3d_2d_;

  void Start() {
    aspect = (float) Screen.width / (float) Screen.height;
    ortho = Matrix4x4.Ortho(-orthographicSize * aspect, orthographicSize * aspect, -orthographicSize, orthographicSize, near, far);
    perspective = Matrix4x4.Perspective(fov, aspect, near, far);
    GetComponent<Camera>().projectionMatrix = ortho;
    orthoOn = false;
    blender = (MatrixBlender) GetComponent(typeof(MatrixBlender));
    audio_source_ = GetComponent<AudioSource>();
    StartCoroutine(FixCamera());     
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Q) && AbleToChange()) {
      ChangeCameraMode();
    }
  }

  void ChangeCameraMode(bool first_time = false) {
    if (!GameManager.gm_instance_.camera_mode_locked_) {
      orthoOn = !orthoOn;
      
      if (orthoOn)
        blender.BlendToMatrix(ortho, perspective_to_ortho_);
      else
        blender.BlendToMatrix(perspective, ortho_to_perspective_);

      GameManager.gm_instance_.SwapCameraMode();

      if (!first_time) {
        if (GameManager.gm_instance_.camera_mode_ == GameManager.GameMode.Mode2D) {
          audio_source_.clip = change3d_2d_;
          if (!audio_source_.isPlaying) audio_source_.Play();
        }
        else if (GameManager.gm_instance_.camera_mode_ == GameManager.GameMode.Mode3D) {
          audio_source_.clip = change2d_3d_;
          if (!audio_source_.isPlaying) audio_source_.Play();       
        }
      }
    }    
  }

  bool AbleToChange() {
    GameManager gm = GameManager.gm_instance_;
    
    RaycastHit hit;
    if (Physics.Raycast(gm.player_.transform.position, gm.player_.transform.forward, out hit, 40)) {
      if (hit.transform.GetComponent<Obstacle>() == null) return true;
      return false;

    } else if (Physics.Raycast(new Vector3(gm.player_.transform.position.x, gm.player_.transform.position.y - 0.6f, gm.player_.transform.position.z), Vector3.forward, out hit, 40)){
      if (hit.transform.GetComponent<Obstacle>() == null) return true;
      return false;

    } else if (Physics.Raycast(new Vector3(gm.player_.transform.position.x, gm.player_.transform.position.y + 0.6f, gm.player_.transform.position.z), Vector3.forward, out hit, 40)){
      if (hit.transform.GetComponent<Obstacle>() == null) return true;
      return false;

    } else if (Physics.Raycast(new Vector3(gm.player_.transform.position.x, gm.player_.transform.position.y - 0.6f, gm.player_.transform.position.z) * -1.0f, Vector3.forward, out hit, 40)){
      if (hit.transform.GetComponent<Obstacle>() == null) return true;
      return false;

    } else if (Physics.Raycast(new Vector3(gm.player_.transform.position.x, gm.player_.transform.position.y + 0.6f, gm.player_.transform.position.z) * -1.0f, Vector3.forward, out hit, 40)){
      if (hit.transform.GetComponent<Obstacle>() == null) return true;
      return false;

    } else if (Physics.Raycast(new Vector3(gm.player_.transform.position.x, gm.player_.transform.position.y - 0.6f, gm.player_.transform.position.z) * -1.0f, Vector3.forward, out hit, 40)){
      if (hit.transform.GetComponent<Obstacle>() == null) return true;
      return false;

    } 
    else {
      return true;
    }

  }

  IEnumerator FixCamera() {
    yield return null;
    ChangeCameraMode(true);
  }
}
