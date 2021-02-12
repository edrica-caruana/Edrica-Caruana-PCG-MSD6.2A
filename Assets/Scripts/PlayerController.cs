using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	float speed = 10f;
    public static bool collidedWithFinish = false;

    //Adding a RigidBody and BoxCollider to the player cube, so that the cube can be able to move
	void Start()
    {
		this.gameObject.AddComponent<Rigidbody>();
		this.gameObject.AddComponent<BoxCollider>();

        //FreezeRotation so that it does not rotate while moving
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(2, 2, 2);
    }

	void Update()
    {
        Move();
    }

    //Moving the cube with arrow keys left, right, up and down by using the RigidBody attached to it along with the speed given
	private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * speed;
        var deltaY = Input.GetAxis("Vertical") * speed;

        this.GetComponent<Rigidbody>().velocity = new Vector3(deltaX, 0, deltaY);
    }

    //If the player triggers the finish collider, the boolean declared at the top will be set to true.
    //This way it can be passed to the GameManager script in which in it we are checking whenever this collision is made to be able to exit play mode as the user would have solved the maze
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Finish")
        {
            collidedWithFinish = true;
            print("You finished the maze");
        }
    }
}
