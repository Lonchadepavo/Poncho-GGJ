using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[RequireComponent (typeof(Camera))]
public class MatrixBlender : MonoBehaviour {

  public static Matrix4x4 MatrixLerp(Matrix4x4 from, Matrix4x4 to, float time) {
    Matrix4x4 ret = new Matrix4x4();
    for (int i = 0; i < 16; i++)
      ret[i] = Mathf.Lerp(from[i], to[i], time);
    return ret;
  }

  private IEnumerator LerpFromTo(Matrix4x4 src, Matrix4x4 dest, float duration) {
    float startTime = Time.time;
    
    while (Time.time - startTime < duration) {
      GetComponent<Camera>().projectionMatrix = MatrixLerp(src, dest, (Time.time - startTime) / duration);
      yield return 1;
    }

    GetComponent<Camera>().projectionMatrix = dest;
    GameManager.gm_instance_.camera_mode_locked_ = false;
    GameManager.gm_instance_.lock_game_ = false;
  }

  public Coroutine BlendToMatrix(Matrix4x4 targetMatrix, float duration) {
    StopAllCoroutines();
    return StartCoroutine(LerpFromTo(GetComponent<Camera>().projectionMatrix, targetMatrix, duration));
  }
}
