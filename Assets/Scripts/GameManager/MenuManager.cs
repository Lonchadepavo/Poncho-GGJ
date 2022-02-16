using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startPlay()
    {
        SceneManager.LoadScene("level 1", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    void OnTriggerEnter(Collider other) {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

}
