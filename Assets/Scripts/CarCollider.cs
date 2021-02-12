using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarCollider : MonoBehaviour
{
    Scene scene;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if(scene.name.Equals("Task3.1-3.2-Level1"))
        {
            if (other.gameObject.name == "FinishBoxFirst")
            {
                SceneManager.LoadScene("Task3.1-3.2-Level2");
            }
        }
        else if (scene.name.Equals("Task3.1-3.2-Level2"))
        {
            if (other.gameObject.name == "FinishBoxFirst")
            {
                SceneManager.LoadScene("Task3.1-3.2-Level3");
            }
        }
        else if (scene.name.Equals("Task3.1-3.2-Level3"))
        {
            if (other.gameObject.name == "FinishBoxFirst")
            {
                UnityEditor.EditorApplication.isPlaying = false;
                Application.Quit();
            }
        }
    }
}
