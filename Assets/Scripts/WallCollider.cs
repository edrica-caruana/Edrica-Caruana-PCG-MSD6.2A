using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    //Each time the finish primitive pyramid triggers the walls of the maze, its position is going to be resetted with a 2f gap from the maze walls
    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Wall"))
        {
            this.transform.position = new Vector3(Random.Range(2, 28) + 2, 0.1f, Random.Range(2, 28) + 2);
        }
    }
}
