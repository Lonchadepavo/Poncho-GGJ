using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObject : MonoBehaviour, IJumpable {
  public enum PlatformType {VerticalMovement, HorizontalMovement, NoMovement};
  public PlatformType platform_type_;

}
