using UnityEngine;

public class Multicam : MonoBehaviour
{
    [SerializeField] private int maxWorldSpace;
    [SerializeField] private int minRenderSize;

    [SerializeField] private Transform agentsParent;
    private int numAgents;

    private bool camsInitialized = false;
    private bool visualInitialized = false;
    private bool overlayInitialized = false;

    [SerializeField] private GameObject baseCam;
    [SerializeField] private Material baseMat;
    [SerializeField] private GameObject baseRenderObj; // individual object with material that renders from indexed cam
    private enum CameraMode
    {
        grid,
        additive
    }
    [SerializeField] private CameraMode camMode = CameraMode.grid;

    private Camera[] cameras;
    private RenderTexture[] rts;
    private Material[] renderMats;
    private GameObject[] renderObjects;

    private Vector3 firstCamPos = new Vector3(0, 0, -10);
    private int yOffset = -20;

    // Update is called once per frame
    void Update()
    {
        if (!camsInitialized && agentsParent.childCount != 0)
            InitializeCamGrid();

        if (camMode.Equals(CameraMode.grid) && !visualInitialized && camsInitialized)
            InitializeGrid();

        if (camMode.Equals(CameraMode.additive) && !overlayInitialized && camsInitialized)
            InitializeOverlap();
    }

    void InitializeCamGrid()
    {
        // parent for all Cameras
        GameObject renderCamsParent = new GameObject();
        renderCamsParent.name = "RenderCams";
        renderCamsParent.transform.parent = this.gameObject.transform;

        numAgents = agentsParent.childCount;
        cameras = new Camera[numAgents];

        for(int i = 0; i < numAgents; i++)
        {
            // instantiate new camera and add to array
            cameras[i] = Instantiate(baseCam, firstCamPos + (Vector3.down * yOffset * i), Quaternion.identity, renderCamsParent.transform).GetComponent<Camera>();
        }

        camsInitialized = true;
    }

    void InitializeGrid()
    {
        // Array inits
        rts = new RenderTexture[numAgents];
        renderMats = new Material[numAgents];
        renderObjects = new GameObject[numAgents];

        // Calculate the grid size (square root of the number of agents, rounded up)
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(numAgents));

        // Calculate the maximum size each tile can have while keeping the grid within maxWorldSpace
        float availableSpacePerTile = maxWorldSpace / gridSize;

        // Tile size is the larger of minRenderSize and the available space per tile, but capped at availableSpacePerTile
        float tileSize = Mathf.Min(availableSpacePerTile, minRenderSize);

        // parent for all RenderObjects
        GameObject renderObjParent = new GameObject();
        renderObjParent.name = "RenderObjects";
        renderObjParent.transform.parent = this.gameObject.transform;

        for (int i = 0; i < numAgents; i++)
        {
            // make render texture
            RenderTexture rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);

            // set matching camera to this render texture
            cameras[i].forceIntoRenderTexture = true;
            cameras[i].targetTexture = rt;

            // make material with render texture as mainTexture
            Material mat = new Material(baseMat);
            mat.name = "RenderMaterial_" + i;
            mat.mainTexture = rt;
            // mat.SetTexture("RenderGridTexture_" + i, rt);

            // Calculate row and column positions for worldspace object
            int row = i / gridSize;
            int col = i % gridSize;

            // Calculate the position of the tile in world space, ensuring the grid is centered
            float xPos = (col * tileSize) - (gridSize * tileSize) / 2 + tileSize / 2;
            float yPos = (row * tileSize) - (gridSize * tileSize) / 2 + tileSize / 2;

            Vector3 gridPosition = new Vector3(xPos, yPos, 0);
            // create square tile gameobject in worldspace at gridPosition with material
            // this object is the actual display
            GameObject renderObj = Instantiate(baseRenderObj, gridPosition, Quaternion.identity, renderObjParent.transform);
            renderObj.transform.localScale = new Vector3(tileSize, tileSize, tileSize);
            renderObj.GetComponent<MeshRenderer>().material = mat;

            // update arrays
            rts[i] = rt;
            renderMats[i] = mat;
            renderObjects[i] = renderObj;
        }

        renderObjParent.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);

        // initialized true
        visualInitialized = true;
    }

    void InitializeOverlap()
    {
        // initialized true
        overlayInitialized = true;

        // parent for all RenderObjects
        GameObject renderObjParent = new GameObject();
        renderObjParent.name = "RenderObjects";
        renderObjParent.transform.parent = this.gameObject.transform;

        for (int i = 0; i < numAgents; i++)
        {
            // make render texture
            RenderTexture rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);

            // set matching camera to this render texture
            cameras[i].forceIntoRenderTexture = true;
            cameras[i].targetTexture = rt;

            // make material with render texture as mainTexture
            Material mat = new Material(baseMat);
            mat.name = "RenderMaterial_" + i;
            // mat.SetColor("_Color", new Color(Color.white.r, Color.white.g, Color.white.b, 0.25f));
            mat.mainTexture = rt;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.1f);
            // mat.SetTexture("RenderGridTexture_" + i, rt);

            // create gameobject in worldspace with material
            // this object is the actual display
            GameObject renderObj = Instantiate(baseRenderObj, this.transform.position, Quaternion.identity, renderObjParent.transform);
            renderObj.transform.localScale = new Vector3(minRenderSize, minRenderSize, minRenderSize);
            renderObj.GetComponent<MeshRenderer>().material = mat;
        }

        renderObjParent.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
    }
}
