using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class loadGame : MonoBehaviour
{
    public Text txt;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            txt.text = "Loading";
            SceneManager.LoadScene(1);
        }
    }
}
