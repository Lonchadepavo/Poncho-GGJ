using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHorizontal : MonoBehaviour
{
    Vector3 origin;
    public float distance;
    // Start is called before the first frame update
    void Start()
    {
        origin = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm_instance_.camera_mode_ == GameManager.GameMode.Mode3D)
        {
            transform.position = new Vector3( Mathf.Cos(Time.deltaTime) * distance + origin.x, origin.y, origin.z);
        }
    }
}
