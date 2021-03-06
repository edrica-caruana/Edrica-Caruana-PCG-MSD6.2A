﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To add the MeshFilter and MeshRenderer Component
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class TriangleMaker : MonoBehaviour
{
    //Declaring the MeshFilter and MeshRenderer
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    //Declaring the MeshGenerator
    MeshGenerator meshGenerator;

    //A varibale for the size of the object
    [SerializeField]
    private Vector3 size = Vector3.one;

    //A list which will contain all the materials
    private List<Material> listOfMaterials;

    // Start is called before the first frame update
    void Start()
    {
        //Initialising the MeshFilter Component
        meshFilter = this.GetComponent<MeshFilter>();

        //Initialising the MeshRenderer Component
        meshRenderer = this.GetComponent<MeshRenderer>();

        //Initialising the MeshGenerator and passing 1 as parameter since the triangle is only going to contain 1 submesh
        meshGenerator = new MeshGenerator(1);

        //Calling the triangle generator method
        GenerateTriangle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //A method to create the triangle
    void GenerateTriangle()
    {
        //The 3 points required to do the triangle by setting the x, y and z of the size of the triangle accordingly
        Vector3 pt0 = new Vector3(size.x, size.y, -size.z);
        Vector3 pt1 = new Vector3(-size.x, size.y, -size.z);
        Vector3 pt2 = new Vector3(-size.x, size.y, size.z);

        //Make the actual triangle by passing the points created before and assign the index to 0 to fetch the first material
        meshGenerator.BuildTriangle(pt0, pt1, pt2, 0);

        //Specify the MeshFilter generated by the MeshGenerator
        meshFilter.mesh = meshGenerator.MeshCreator();

        //Calling the method to be able to populate the list
        IncludeMaterials();

        //Converting the list of materials to an array and assign it to the materials in the MehshRenderer
        meshRenderer.materials = listOfMaterials.ToArray();
    }

    //A method to add materials
    private void IncludeMaterials()
    {
        Material shinyGrey = new Material(Shader.Find("Specular"));

        shinyGrey.color = Color.grey;

        listOfMaterials = new List<Material>();

        //Add the material programatically to the list
        listOfMaterials.Add(shinyGrey);
    }
}
