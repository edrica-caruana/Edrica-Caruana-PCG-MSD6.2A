﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidMaker : PrimitivesBehaviours
{
    // Start is called before the first frame update
    void Start()
    {
        //Initialising the MeshFilter Component
        meshFilter = GetComponent<MeshFilter>();

        //Initialising the MeshRenderer Component
        meshRenderer = GetComponent<MeshRenderer>();

        size = Vector3.one;

        meshSize = 1;

        if (pyramidSize == 0f)
        {
            pyramidSize = 6f;
        }

        if (primitiveColour == new Color(0.0f, 0.0f, 0.0f, 0.0f))
        {
            primitiveColour = Color.cyan;
        }

        //Initialising the MeshGenerator and passing the meshsize as parameter since the pyramid is going to contain 1 submesh
        meshGenerator = new MeshGenerator(meshSize);

        //Calling the plane generator method
        GeneratePyramid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //A method to create the pyramid
    void GeneratePyramid()
    {
        //Set the points of the heaxgon-based pyramid
        Vector3 topPoint = new Vector3(0, pyramidSize, 0);
        Vector3 base0Point = Quaternion.AngleAxis(0f, Vector3.up) * Vector3.forward * pyramidSize;
        Vector3 base1Point = Quaternion.AngleAxis(72f, Vector3.up) * Vector3.forward * pyramidSize;
        Vector3 base2Point = Quaternion.AngleAxis(144f, Vector3.up) * Vector3.forward * pyramidSize;
        Vector3 base3Point = Quaternion.AngleAxis(216f, Vector3.up) * Vector3.forward * pyramidSize;
        Vector3 base4Point = Quaternion.AngleAxis(288f, Vector3.up) * Vector3.forward * pyramidSize;

        //Build the sides and base of our hexagon-based pyramid
        meshGenerator.BuildTriangle(topPoint, base0Point, base1Point, 0);
        meshGenerator.BuildTriangle(topPoint, base1Point, base2Point, 0);
        meshGenerator.BuildTriangle(topPoint, base2Point, base3Point, 0);
        meshGenerator.BuildTriangle(topPoint, base3Point, base4Point, 0);
        meshGenerator.BuildTriangle(topPoint, base4Point, base0Point, 0);
        meshGenerator.BuildTriangle(base0Point, base4Point, base3Point, 0);
        meshGenerator.BuildTriangle(base0Point, base3Point, base2Point, 0);
        meshGenerator.BuildTriangle(base0Point, base2Point, base1Point, 0);

        //Specify the MeshFilter generated by the MeshGenerator
        meshFilter.mesh = meshGenerator.MeshCreator();

        //Calling the method to be able to populate the list
        IncludeMaterials();

        //Converting the list of materials to an array and assign it to the materials in the MehshRenderer
        meshRenderer.materials = listOfMaterials.ToArray();
    }
}
