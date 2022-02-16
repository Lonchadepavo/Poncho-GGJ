using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
  AudioSource audio_source_;

  void Start() {
    audio_source_ = GetComponent<AudioSource>();
  }

  void OnTriggerEnter(Collider other) {
    if (other.transform.tag.Equals("Player")) {
      GameManager.gm_instance_.coin_count_++;
      audio_source_.Play();
      GetComponent<Renderer>().enabled = false;
      StartCoroutine(DisableCrystal());
    }
  }

  IEnumerator DisableCrystal() {
    yield return new WaitForSeconds(2.0f);
    this.gameObject.SetActive(false);
  }

}
