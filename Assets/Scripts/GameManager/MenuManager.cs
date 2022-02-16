using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
  public void startPlay() {
    SceneManager.LoadScene("level 1", LoadSceneMode.Single);
  }

  public void Exit() {
    Application.Quit();
  }

  public void Credits() {
    SceneManager.LoadScene("Credits", LoadSceneMode.Single);
  }

  void OnTriggerEnter(Collider other) {
    Credits();
  }

}
