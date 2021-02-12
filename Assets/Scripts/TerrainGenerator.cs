using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class TerrainTextureData
{
    public Texture2D terrainTexture;
    public float minHeight;
    public float maxHeight;
    public Vector2 tileSize;
}

[System.Serializable]
class TreeData
{
    public GameObject treeMesh;
    public float minHieght;
    public float maxHeight;
}

[ExecuteInEditMode]

public class TerrainGenerator : MonoBehaviour
{
    private Terrain terrain;

    private TerrainData terrainData;

    [SerializeField]
    private List <Texture2D> heightMapImageList = new List<Texture2D>();

    private Texture2D heightMapImage;

    [SerializeField]
    private Vector3 heightMapScale = new Vector3(1, 1, 1);

    [SerializeField]
    private bool generatePerlinNoiseTerrain = false;

    [SerializeField]
    private bool flattenTerrain = false;

    [SerializeField]
    private bool addTexture = false;

    [SerializeField]
    private bool addTree = false;

    [SerializeField]
    private bool addWater = false;

    [SerializeField]
    private bool removeTexture = false;

    [SerializeField]
    private float perlinNoiseWidthScale;

    [SerializeField]
    private float perlinNoiseHeightScale;

    [SerializeField]
    private List<TerrainTextureData> terrainTextureDataList;

    [SerializeField]
    private float terrainTextureBlendOffset = 0.01f;

    [SerializeField]
    private List<TreeData> treeDataList;

    //42
    [SerializeField]
    private int maxTree;

    //43
    [SerializeField]
    private int treeSpacing = 10;

    //51
    [SerializeField]
    private float randomXRange = 5.0f;

    //52
    [SerializeField]
    private float randomZRange = 5.0f;

    //55
    [SerializeField]
    private int terrainLayerIndex = 8;

    [SerializeField]
    private GameObject water;

    //60
    [SerializeField]
    private float waterHeight;

    void Start()
    {
        terrain = GetComponent<Terrain>();

        terrainData = Terrain.activeTerrain.terrainData;

        CreateTerrain();
        TerrainTexture();
        AddTree();
    }

    void initialise()
    {
#if UNITY_EDITOR
        if (terrain == null)
        {
            terrain = GetComponent<Terrain>();
        }
        if (terrainData == null)
        {
            terrainData = Terrain.activeTerrain.terrainData;
        }
#endif
    }

    private void OnValidate()
    {
        initialise();

        if (flattenTerrain)
        {
            generatePerlinNoiseTerrain = false;
        }

        if (generatePerlinNoiseTerrain || flattenTerrain)
        {
            CreateTerrain();
        }

        if (removeTexture)
        {
            addTexture = false;
        }

        if (addTexture)
        {
            TerrainTexture();
        }

        if (addTree)
        {
            AddTree();
        }

        if (addWater)
        {
            AddWater();
        }
    }

    void CreateTerrain()
    {
        int random = Random.Range(0, 3);

        heightMapImage = heightMapImageList[random];

        print(heightMapImageList[random]);

        perlinNoiseWidthScale = Random.Range(0.005f, 0.009f);

        perlinNoiseHeightScale = Random.Range(0.005f, 0.009f);

        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                if (generatePerlinNoiseTerrain)
                {
                    heightMap[width, height] = Mathf.PerlinNoise(width * perlinNoiseWidthScale * (heightMapImage.GetPixel((int)(width * heightMapScale.x),
                                                                       (int)(height * heightMapScale.z)).grayscale
                                                                       * heightMapScale.y), height * perlinNoiseHeightScale);
                }
                if (flattenTerrain)
                {
                    heightMap[width, height] = 0;
                }
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    void TerrainTexture()
    {
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureDataList.Count];

        for (int i = 0; i < terrainTextureDataList.Count; i++)
        {
            if (addTexture)
            {
                terrainLayers[i] = new TerrainLayer();

                terrainLayers[i].diffuseTexture = terrainTextureDataList[i].terrainTexture;

                terrainLayers[i].tileSize = terrainTextureDataList[i].tileSize;
            }
            else if (removeTexture)
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = null;
            }
        }

        terrainData.terrainLayers = terrainLayers;

        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        float[,,] alphaMapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int height = 0; height < terrainData.alphamapHeight; height++)
        {
            for (int width = 0; width < terrainData.alphamapWidth; width++)
            {
                float[] splatmap = new float[terrainData.alphamapLayers];

                for (int i = 0; i < terrainTextureDataList.Count; i++)
                {
                    float minHeight = terrainTextureDataList[i].minHeight - terrainTextureBlendOffset;
                    float maxHieght = terrainTextureDataList[i].maxHeight + terrainTextureBlendOffset;

                    if (heightMap[width, height] >= minHeight && heightMap[width, height] <= maxHieght)
                    {
                        splatmap[i] = 1;
                    }
                }

                NormalizeSplatMap(splatmap);

                for (int j = 0; j < terrainTextureDataList.Count; j++)
                {
                    alphaMapList[width, height, j] = splatmap[j];
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMapList);
    }

    void NormalizeSplatMap(float[] splatMap)
    {
        float total = 0;

        for (int i = 0; i < splatMap.Length; i++)
        {
            total += splatMap[i];
        }

        for (int i = 0; i < splatMap.Length; i++)
        {
            splatMap[i] = splatMap[i] / total;
        }
    }

    void AddTree()
    {
        maxTree = Random.Range(2000, 6000);

        TreePrototype[] trees = new TreePrototype[treeDataList.Count];

        for (int i = 0; i < treeDataList.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeDataList[i].treeMesh;
        }

        terrainData.treePrototypes = trees;

        List<TreeInstance> treeInstanceList = new List<TreeInstance>();

        if (addTree)
        {
            for (int z = 0; z < terrainData.size.z; z += treeSpacing)
            {
                for (int x = 0; x < terrainData.size.z; x += treeSpacing)
                {
                    for (int treePrototypeIndex = 0; treePrototypeIndex < trees.Length; treePrototypeIndex++)
                    {
                        if (treeInstanceList.Count < maxTree)
                        {
                            float currentHeight = terrainData.GetHeight(x, z) / terrainData.size.y;

                            if (currentHeight >= treeDataList[treePrototypeIndex].minHieght && currentHeight <= treeDataList[treePrototypeIndex].maxHeight)
                            {
                                float randomX = (x + Random.Range(-randomXRange, randomXRange)) / terrainData.size.x;
                                float randomZ = (z + Random.Range(-randomZRange, randomZRange)) / terrainData.size.z;

                                TreeInstance treeInstance = new TreeInstance();

                                treeInstance.position = new Vector3(randomX, currentHeight, randomZ);

                                Vector3 treePosition = new Vector3(treeInstance.position.x * terrainData.size.x,
                                                                    treeInstance.position.y * terrainData.size.y,
                                                                    treeInstance.position.z * terrainData.size.z) + this.transform.position;

                                RaycastHit raycastHit;

                                int layerMask = 1 << terrainLayerIndex;

                                if (Physics.Raycast(treePosition, Vector3.down, out raycastHit, 100, layerMask) ||
                                    Physics.Raycast(treePosition, Vector3.up, out raycastHit, 100, layerMask))
                                {
                                    float treeHieght = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;

                                    treeInstance.position = new Vector3(treeInstance.position.x, treeHieght, treeInstance.position.z);

                                    treeInstance.rotation = Random.Range(0, 360);
                                    treeInstance.prototypeIndex = treePrototypeIndex;

                                    treeInstance.color = Color.white;
                                    treeInstance.lightmapColor = Color.white;
                                    treeInstance.heightScale = 0.95f;
                                    treeInstance.widthScale = 0.95f;

                                    treeInstanceList.Add(treeInstance);
                                }
                            }
                        }
                    }
                }
            }
        }

        terrainData.treeInstances = treeInstanceList.ToArray();
    }

    void AddWater()
    {
        waterHeight = Random.Range(0.2f, 0.4f);

        GameObject waterGameObject = GameObject.Find("water");

        if (!waterGameObject)
        {
            waterGameObject = Instantiate(water, this.transform.position, this.transform.rotation);
            waterGameObject.name = "water";
        }

        waterGameObject.transform.position = this.transform.position + new Vector3(
            terrainData.size.x / 2,
            waterHeight * terrainData.size.y,
            terrainData.size.z / 2);

        waterGameObject.transform.localScale = new Vector3(terrainData.size.x, 1, terrainData.size.z);
    }
}
