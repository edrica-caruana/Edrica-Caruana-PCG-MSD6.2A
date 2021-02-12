using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Constantly checking the either the user clicked the escape button from the keyboard or the player finished the maze, so that the game could end
        if ((Input.GetKeyDown(KeyCode.Escape)) || ( PlayerController.collidedWithFinish == true))
        {
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}
