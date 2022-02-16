using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Talkable : MonoBehaviour, IInteract, IJumpable {
  public string[] dialogues;
  public Canvas HUD;
  public TMP_Text textbox;
  int current_dialogue;
  public float delay;
  private string current_string = "";

  GameObject trigger_talk_;

  public GameObject master3d_;
  public GameObject master2d_;

  void Start() {
    trigger_talk_ = transform.GetChild(0).gameObject;
    GameManager.gm_instance_.onCameraChangeEvent += ChangeMasterModel; 
  }

  void openDialogues() {
    HUD.gameObject.SetActive(true);
    if (current_dialogue != 0) {
      current_dialogue = 0;
    }
    textbox.text = dialogues[current_dialogue];
  }

  void closeDialogues() {
    HUD.gameObject.SetActive(false);
    current_dialogue = 0;
  }

  public void Action() {
    openDialogues();
  }

  private void Update() {
    if (Input.GetMouseButtonDown(0)) {
      current_dialogue++;
      if (dialogues.Length == current_dialogue) {
        closeDialogues();
      }

      else {
        textbox.text = dialogues[current_dialogue];
      }
    }
      
  }

  void ChangeMasterModel() {
    if (GameManager.gm_instance_.camera_mode_ == GameManager.GameMode.Mode2D) {
      master3d_.SetActive(false);
      master2d_.SetActive(true);
    }
    else if (GameManager.gm_instance_.camera_mode_ == GameManager.GameMode.Mode3D) {
      master3d_.SetActive(true);
      master2d_.SetActive(false);            
    }
  }
}
