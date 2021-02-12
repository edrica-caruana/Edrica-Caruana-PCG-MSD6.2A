using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To add the MeshFilter and MeshRenderer Component
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class PrimitivesBehaviours : MonoBehaviour
{
    //Declaring the MeshFilter and MeshRenderer
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;

    //Declaring the MeshGenerator
    protected MeshGenerator meshGenerator;

    //A varibale for the size of the object
    protected Vector3 size;

    public Vector3 Size
    {
        get
        {
            return size;
        }
        set
        {
            size = value;
        }
    }

    //A variable to specifiy the submesh size
    protected int meshSize;

    //Pyramid height size
    protected float pyramidSize;

    public float PyramidSize
    {
        get
        {
            return pyramidSize;
        }
        set
        {
            pyramidSize = value;
        }
    }

    //Size of cell in the plane
    protected float cellSize = 1f;

    //Width of the plane
    protected int widthPlane = 26;

    //Height of the Plane
    protected int heightPlane = 26;

    //A list which will contain all the materials
    protected List<Material> listOfMaterials;

    protected Color primitiveColour;

    public Color PrimitiveColour 
    {
        get 
        {
            return primitiveColour;
        }
        set
        {
            primitiveColour = value;
        }
    }

    //A method to add materials
    public virtual void IncludeMaterials()
    {
        Material fillColour = new Material(Shader.Find("Specular"));
        fillColour.color = primitiveColour;

        listOfMaterials = new List<Material>();

        //Add the material programatically to the list
        listOfMaterials.Add(fillColour);
    }
}
