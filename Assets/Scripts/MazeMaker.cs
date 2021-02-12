using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeMaker : MonoBehaviour
{
    GameObject parentMaze;
    GameObject planeMaze;

    GameObject wallMaze;

    GameObject playerCube;

    GameObject startPyramid;
    GameObject endPyramid;

    List<GameObject> listOfWalls;

    // Start is called before the first frame update
    void Start()
    {
        parentMaze = this.gameObject;
        parentMaze.transform.position = Vector3.zero;
        parentMaze.name = "ParentMaze";

        wallMaze = new GameObject();
        wallMaze.transform.position = Vector3.zero;
        wallMaze.transform.SetParent(parentMaze.transform);
        wallMaze.name = "WallMaze";

        mazePlayer();

        listOfWalls = new List<GameObject>();

        mazeFloor();

        mazeWalls();

        startPosition();
        endPositions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Generating the player of the maze using the cube primitive
    //The player will be a cube and attached it will contain the player controller script to move it with arrow keys
    //The instantiated position of the player cube is randomly chosen
    void mazePlayer()
    {
        playerCube = new GameObject();
        playerCube.transform.position = new Vector3(Random.Range(2, 28), 0.1f, Random.Range(2, 28));
        playerCube.AddComponent<CubeMaker>();
        playerCube.AddComponent<PlayerController>();
        playerCube.name = "Player Cube";
        playerCube.tag = "Player";
    }

    //Generating the floor of the maze using the plane primitive
    //The plane floor will containa a box collider so that the player, start and finish marker won't fall from the maze
    void mazeFloor()
    {
        planeMaze = new GameObject();
        planeMaze.transform.position = Vector3.zero;
        planeMaze.transform.SetParent(parentMaze.transform);
        planeMaze.name = "Plane";

        planeMaze.AddComponent<BoxCollider>();

        planeMaze.AddComponent<PlaneMaker>();

        PlaneMaker planeMaker = planeMaze.GetComponent<PlaneMaker>();

        planeMaker.PrimitiveColour = new Color32(166, 195, 201, 255);

        planeMaze.GetComponent<BoxCollider>().size = new Vector3(30, 0.1f, 30);
        planeMaze.GetComponent<BoxCollider>().center = new Vector3(15, 0, 15);
    }

    //Generating the walls of the maze using the cube primitve
    //Each cube is being scaled and positioned according to where it is supposed to be on the plane
    //Moreover, each cube also contains a box collider so that the player will not go through the walls of the maze
    void mazeWalls()
    {
        for (int i = 0; i < 15; i++)
        {
            GameObject wall = new GameObject();
            wall.transform.position = Vector3.zero;
            wall.transform.SetParent(wallMaze.transform);
            wall.name = "Wall " + (i + 1);
            wall.tag = "Wall";

            wall.AddComponent<BoxCollider>();

            wall.AddComponent<CubeMaker>();

            listOfWalls.Add(wall);

            CubeMaker cubeMaker = listOfWalls[i].GetComponent<CubeMaker>();
            cubeMaker.PrimitiveColour = new Color32(82, 140, 160, 255);
        }

        listOfWalls[0].GetComponent<CubeMaker>().Size = new Vector3(0.5f, 1.5f, 15.5f);
        listOfWalls[0].transform.position = new Vector3(0.5f, 1.5f, 15);

        listOfWalls[1].GetComponent<CubeMaker>().Size = new Vector3(15f, 1.5f, 0.5f);
        listOfWalls[1].transform.position = new Vector3(15f, 1.5f, 30);

        listOfWalls[2].GetComponent<CubeMaker>().Size = new Vector3(0.5f, 1.5f, 15.5f);
        listOfWalls[2].transform.position = new Vector3(30, 1.5f, 15);

        listOfWalls[3].GetComponent<CubeMaker>().Size = new Vector3(15, 1.5f, 0.5f);
        listOfWalls[3].transform.position = new Vector3(15, 1.5f, 0);

        listOfWalls[4].GetComponent<CubeMaker>().Size = new Vector3(0.5f, 1.5f, 2.5f);
        listOfWalls[4].transform.position = new Vector3(15, 1.5f, 2);

        listOfWalls[5].GetComponent<CubeMaker>().Size = new Vector3(0.5f, 1.5f, 6);
        listOfWalls[5].transform.position = new Vector3(25, 1.5f, 5.5f);

        listOfWalls[6].GetComponent<CubeMaker>().Size = new Vector3(0.5f, 1.5f, 2.5f);
        listOfWalls[6].transform.position = new Vector3(5, 1.5f, 8);

        listOfWalls[7].GetComponent<CubeMaker>().Size = new Vector3(10, 1.5f, 0.5f);
        listOfWalls[7].transform.position = new Vector3(10, 1.5f, 10);

        listOfWalls[8].GetComponent<CubeMaker>().Size = new Vector3(0.5f, 1.5f, 4);
        listOfWalls[8].transform.position = new Vector3(20, 1.5f, 13.5f);

        listOfWalls[9].GetComponent<CubeMaker>().Size = new Vector3(0.5f, 1.5f, 2);
        listOfWalls[9].transform.position = new Vector3(10, 1.5f, 12);

        listOfWalls[10].GetComponent<CubeMaker>().Size = new Vector3(5, 1.5f, 0.5f);
        listOfWalls[10].transform.position = new Vector3(5, 1.5f, 20);

        listOfWalls[11].GetComponent<CubeMaker>().Size = new Vector3(0.5f, 1.5f, 2.5f);
        listOfWalls[11].transform.position = new Vector3(5, 1.5f, 22);

        listOfWalls[12].GetComponent<CubeMaker>().Size = new Vector3(0.5f, 1.5f, 5);
        listOfWalls[12].transform.position = new Vector3(15, 1.5f, 25);

        listOfWalls[13].GetComponent<CubeMaker>().Size = new Vector3(5, 1.5f, 1);
        listOfWalls[13].transform.position = new Vector3(25, 1.5f, 24);

        listOfWalls[14].GetComponent<CubeMaker>().Size = new Vector3(0.5f, 1.5f, 2.5f);
        listOfWalls[14].transform.position = new Vector3(25, 1.5f, 22);

        for (int i = 0; i < 15; i++)
        {
            listOfWalls[i].GetComponent<BoxCollider>().size = (listOfWalls[i].GetComponent<CubeMaker>().Size) * 2;
        }
    }

    //Finish Point Marker will be a pyramid primitive in orange colour
    //The instantiated position of the finish marker is randomly chosen
    void endPositions()
    {
        endPyramid = new GameObject();
        endPyramid.name = "Finish Pyramid";
        endPyramid.tag = "Finish";
        endPyramid.transform.position = new Vector3(Random.Range(2,28), 0.1f, Random.Range(2, 28));
        endPyramid.AddComponent<PyramidMaker>();

        PyramidMaker pyramidMaker = endPyramid.GetComponent<PyramidMaker>();

        pyramidMaker.PyramidSize = 2f;
        pyramidMaker.PrimitiveColour = new Color32(255, 69, 0, 255);

        endPyramid.AddComponent<BoxCollider>();
        endPyramid.GetComponent<BoxCollider>().size = new Vector3(4, 2, 3);
        endPyramid.GetComponent<BoxCollider>().center = new Vector3(0, 1, 0);
        endPyramid.GetComponent<BoxCollider>().isTrigger = true;
        endPyramid.AddComponent<WallCollider>();
        endPyramid.AddComponent<Rigidbody>();
        endPyramid.GetComponent<Rigidbody>().useGravity = false;
    }

    //Starting Point Marker will be a pyramid primitive in green colour
    //The instantiated position of the start marker will be exactly like the player as this is a marker to show from where the player started
    void startPosition()
    {
        startPyramid = new GameObject();
        startPyramid.name = "Start Pyramid";
        startPyramid.tag = "Start";
        startPyramid.transform.position = playerCube.transform.position;
        startPyramid.AddComponent<PyramidMaker>();

        PyramidMaker pyramidMaker = startPyramid.GetComponent<PyramidMaker>();

        pyramidMaker.PyramidSize = 2f;
        pyramidMaker.PrimitiveColour = Color.green;
    }
}
