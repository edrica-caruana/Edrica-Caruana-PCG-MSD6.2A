using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator
{
    //A list to store the points in our mesh
    private List<Vector3> vertices = new List<Vector3>();

    //A list of indices that points to the index location in our points list
    private List<int> indices = new List<int>();

    //A list to define the direction of each point
    private List<Vector3> normals = new List<Vector3>();

    //A list which will store the coordinates of the UV which is a texture that is going to wrap around our object
    private List<Vector2> uvs = new List<Vector2>();

    //A list of int arrays to for the submesh indices of each triangle
    private List<int>[] submeshIndices = new List<int>[] { };

    //A constructor to be used in other classes
    public MeshGenerator(int submeshNumber)
    {
        //Initialise the submeshIndices array by the number of submeshes given as the parameter
        submeshIndices = new List<int>[submeshNumber];

        //For each index in the submeshIndices list, a new list will be initialised
        for(int y=0; y<submeshNumber; y++)
        {
            submeshIndices[y] = new List<int>();
        }
    }

    //A method which will override the other build triangle method and only pass the points and the submesh size in it and the normal will be caluclated through the method
    public void BuildTriangle(Vector3 pt0, Vector3 pt1, Vector3 pt2, int submesh)
    {
        Vector3 normal = Vector3.Cross(pt1 - pt0, pt2 - pt0).normalized;

        BuildTriangle(pt0, pt1, pt2, normal, submesh);
    }

    //A method to build our triangle
    //As parameters we are going to pass the points of the triangle, the direction/normal and the index in our submesh
    public void BuildTriangle(Vector3 pt0, Vector3 pt1, Vector3 pt2, Vector3 normal, int submesh)
    {
        int p0Index = vertices.Count;
        int p1Index = vertices.Count + 1;
        int p2Index = vertices.Count + 2;

        indices.AddRange(new int[] { p0Index, p1Index, p2Index});

        submeshIndices[submesh].AddRange(new int[] { p0Index, p1Index, p2Index});

        //Add each parameter point to our vertices list
        vertices.AddRange(new Vector3[] { pt0, pt1, pt2 });

        //Adding the normals to the normal list
        normals.AddRange(new Vector3[] { normal, normal, normal });

        //Adding the coordinates of the uv to the uvs list
        uvs.AddRange(new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) });
    }

    //A method to build the actual mesh for the triangle
    public Mesh MeshCreator()
    {
        //Initialise the mesh object
        Mesh mesh = new Mesh();

        //Converting all lists declared at the top to arrays since Mesh accepts arrays
        mesh.vertices = vertices.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();

        //The length of the submeshIndices will specify how many submeshes there are
        //Thus, this is going to be equal to the subMeshCount
        mesh.subMeshCount = submeshIndices.Length;

        //A for loop to go throgh the array of indices and set the triangle accordingly to the vertices index
        for (int y=0; y<submeshIndices.Length; y++)
        {
            if(submeshIndices[y].Count < 3)
            {
                mesh.SetTriangles(new int[3] { 0, 0, 0 }, y);
            }
            else
            {
                mesh.SetTriangles(submeshIndices[y].ToArray(), y);
            }
        }

        //Return back the mesh for Unity to render
        return mesh;
    }
}
