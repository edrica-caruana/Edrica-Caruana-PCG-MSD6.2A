using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To add the MeshFilter, MeshRenderer, and Mesh Collider Component
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class RaceTrackMaker : MonoBehaviour
{
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    MeshRenderer meshRenderer;
    MeshGenerator meshGenerator;

    [SerializeField]
    private float radius = 30f;

    [SerializeField]
    private float segments = 300f;

    [SerializeField]
    private float lineMarkerWidth = 0.3f;

    [SerializeField]
    private float trackWidth = 8f;

    [SerializeField]
    private float barrierEdgeWidth = 1f;

    [SerializeField]
    private float barrierEdgeHeight = 1f;

    [SerializeField]
    private int submeshSize = 6;

    [SerializeField]
    private float wavyness; 

    [SerializeField]
    private float waveScale;

    [SerializeField]
    private Vector2 waveOffset;

    [SerializeField]
    private Vector2 waveStep = new Vector2(0.01f, 0.01f);

    [SerializeField]
    private bool stripeCheck = true;

    private bool startLineMarker = false;
    int startLineWidth = 0;

    [SerializeField]
    private GameObject car;

    List<Vector3> vectors;
    int startingIndex;

    List<Material> listOfMaterials;

    void Start()
    {
        meshFilter = this.GetComponent<MeshFilter>();
        meshCollider = this.GetComponent<MeshCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshGenerator = new MeshGenerator(submeshSize);

        //Setting random values for the wayvness of the race track
        wavyness = Random.Range(30, 100);
        waveScale = Random.Range(2f, 4f);
        waveOffset = new Vector2(Random.Range(3, 6), Random.Range(3, 6));

        RoadTrackGenerator();
    }

    private void Update()
    {
    }

    void RoadTrackGenerator()
    {
        float segmentAngle = 360 / segments;

        vectors = new List<Vector3>();

        for(float degrees = 0; degrees < 360f; degrees += segmentAngle)
        {
            Vector3 vector = Quaternion.AngleAxis(degrees, Vector3.up) * Vector3.forward * radius;
            vectors.Add(vector);
        }

        Vector2 wave = this.waveOffset;

        for(int index=1; index < vectors.Count; index++)
        {
            wave += waveStep;

            Vector3 vector = vectors[index];
            Vector3 centerDirection = vector.normalized;

            float noise = Mathf.PerlinNoise(wave.x * waveScale, wave.y * waveScale);

            noise *= wavyness;

            float control = Mathf.PingPong(index, vectors.Count / 2f) / (vectors.Count / 2f);

            vectors[index] += centerDirection * noise * control;
        }

        startingIndex = Random.Range(1, vectors.Count);

        Vector3 pointPrevious;
        Vector3 pointCurrent;
        Vector3 pointNext;

        for (int pathSegments = startingIndex; pathSegments < vectors.Count + 1; pathSegments++)
        {
            pointPrevious = vectors[pathSegments - 1];
            pointCurrent = vectors[pathSegments % vectors.Count]; 
            pointNext = vectors[(pathSegments + 1) % vectors.Count];

            ExtrudeRoad(meshGenerator, pointPrevious, pointCurrent, pointNext);
        }

        for(int i = 1; i < startingIndex; i++)
        {
            pointPrevious = vectors[i - 1];
            pointCurrent = vectors[i % vectors.Count];
            pointNext = vectors[(i + 1) % vectors.Count];

            ExtrudeRoad(meshGenerator, pointPrevious, pointCurrent, pointNext);
        }

        car.transform.position = vectors[startingIndex];
        car.transform.LookAt(vectors[startingIndex + 1]);

        meshFilter.mesh = meshGenerator.MeshCreator();
        meshCollider.sharedMesh = meshFilter.mesh;

        StartCoroutine(FinishTriggerBoxes(vectors, startingIndex));

        //Calling the method to be able to populate the list
        IncludeMaterials();

        //Converting the list of materials to an array and assign it to the materials in the MehshRenderer
        meshRenderer.materials = listOfMaterials.ToArray();
    }

    IEnumerator FinishTriggerBoxes(List<Vector3> vectors, int startPointIndex)
    {
        yield return new WaitForSeconds(5f);

        GameObject finishBoxFirst = new GameObject();
        finishBoxFirst.transform.position = vectors[startPointIndex + 1];
        finishBoxFirst.transform.LookAt(vectors[startPointIndex + 2]);
        finishBoxFirst.transform.localScale = new Vector3((lineMarkerWidth + trackWidth) * 2, 3, 2);
        finishBoxFirst.name = "FinishBoxFirst";

        finishBoxFirst.AddComponent<BoxCollider>();
        finishBoxFirst.GetComponent<BoxCollider>().isTrigger = true;

        GameObject finishBoxSecond = new GameObject();
        finishBoxSecond.transform.position = vectors[startPointIndex + 2];
        finishBoxSecond.transform.LookAt(vectors[startPointIndex + 3]);
        finishBoxSecond.transform.localScale = new Vector3((lineMarkerWidth + trackWidth) * 2, 3, 2);
        finishBoxSecond.name = "FinishBoxSecond";

        finishBoxSecond.AddComponent<BoxCollider>();
    }

    private void ExtrudeRoad(MeshGenerator meshGenerator, Vector3 pointPrevious, Vector3 pointCurrent, Vector3 pointNext)
    {
        Vector3 offset = new Vector3();
        Vector3 targetOffset = new Vector3();

        if(startLineMarker == false)
        {
            offset = Vector3.zero;
            targetOffset = Vector3.forward * lineMarkerWidth;

            RoadQuadMaker(meshGenerator, pointPrevious, pointCurrent, pointNext, offset, targetOffset, 3);

            offset += targetOffset;
            targetOffset = Vector3.forward * trackWidth;

            RoadQuadMaker(meshGenerator, pointPrevious, pointCurrent, pointNext, offset, targetOffset, 3);

            startLineWidth++;

            if(startLineWidth >= 2)
            {
                startLineMarker = true;
            }
        }
        else
        {
            offset = Vector3.zero;
            targetOffset = Vector3.forward * lineMarkerWidth;
            RoadQuadMaker(meshGenerator, pointPrevious, pointCurrent, pointNext, offset, targetOffset, 0);

            offset += targetOffset;
            targetOffset = Vector3.forward * trackWidth;
            RoadQuadMaker(meshGenerator, pointPrevious, pointCurrent, pointNext, offset, targetOffset, 1);
        }

        int barrierStripe = 2;

        if (stripeCheck)
        {
            barrierStripe = 0;
        }

        stripeCheck = !stripeCheck;

        offset += targetOffset;
        targetOffset = Vector3.up * barrierEdgeHeight;
        RoadQuadMaker(meshGenerator, pointPrevious, pointCurrent, pointNext, offset, targetOffset, barrierStripe);

        offset += targetOffset;
        targetOffset = Vector3.forward * barrierEdgeWidth;
        RoadQuadMaker(meshGenerator, pointPrevious, pointCurrent, pointNext, offset, targetOffset, barrierStripe);

        offset += targetOffset;
        targetOffset = -Vector3.up * barrierEdgeHeight;
        RoadQuadMaker(meshGenerator, pointPrevious, pointCurrent, pointNext, offset, targetOffset, barrierStripe);
    }

    private void RoadQuadMaker(MeshGenerator meshGenerator, Vector3 pointPrevious, Vector3 pointCurrent, Vector3 pointNext, Vector3 offset, Vector3 targetOffset, int submesh)
    {
        Vector3 forward = (pointNext - pointCurrent).normalized;
        Vector3 forwardPrevious = (pointCurrent - pointPrevious).normalized;

        Quaternion perpendicularAngle = Quaternion.LookRotation(Vector3.Cross(forward, Vector3.up));
        Quaternion perpendicularPrevious = Quaternion.LookRotation(Vector3.Cross(forwardPrevious, Vector3.up));

        Vector3 topLeft = pointCurrent + (perpendicularPrevious * offset);
        Vector3 topRight = pointCurrent + (perpendicularPrevious * (offset + targetOffset));
        Vector3 bottomLeft = pointNext + (perpendicularAngle * offset);
        Vector3 bottomRight = pointNext + (perpendicularAngle * (offset + targetOffset));

        meshGenerator.BuildTriangle(topLeft, topRight, bottomLeft, submesh);
        meshGenerator.BuildTriangle(topRight, bottomRight, bottomLeft, submesh);

        perpendicularAngle = Quaternion.LookRotation(Vector3.Cross(-forward, Vector3.up));
        perpendicularPrevious = Quaternion.LookRotation(Vector3.Cross(-forwardPrevious, Vector3.up));

        topLeft = pointCurrent + (perpendicularPrevious * offset);
        topRight = pointCurrent + (perpendicularPrevious * (offset + targetOffset));
        bottomLeft = pointNext + (perpendicularAngle * offset);
        bottomRight = pointNext + (perpendicularAngle * (offset + targetOffset));

        meshGenerator.BuildTriangle(bottomLeft, bottomRight, topLeft, submesh);
        meshGenerator.BuildTriangle(bottomRight, topRight, topLeft, submesh);
    }

    public virtual void IncludeMaterials()
    {
        Material white = new Material(Shader.Find("Specular"));
        white.color = Color.white;

        Material black = new Material(Shader.Find("Specular"));
        black.color = Color.black;

        Material red = new Material(Shader.Find("Specular"));
        red.color = Color.red;

        Material yellow = new Material(Shader.Find("Specular"));
        yellow.color = Color.yellow;

        listOfMaterials = new List<Material>();

        //Add the material programatically to the list
        listOfMaterials.Add(white);
        listOfMaterials.Add(black);
        listOfMaterials.Add(red);
        listOfMaterials.Add(yellow);
    }
}
