using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildActions : MonoBehaviour
{
    public static BuildActions active;

    public List<GameObject> activeBuildings = new List<GameObject>();

    private void Awake()
    {
        active = this;
    }

    public void MakeBuildPart(Item buildPart, Cube.Type face)
    {
        Mesh mesh = MakeMesh(buildPart, face,0,0,0);

        //Create a temporary GO for the buildpart
        GameObject buildPartGO = new GameObject();
        buildPartGO.AddComponent<MeshFilter>();
        buildPartGO.AddComponent<MeshRenderer>();

        //Add the mesh
        buildPartGO.GetComponent<MeshFilter>().mesh = mesh;
        buildPartGO.GetComponent<MeshRenderer>().sharedMaterial = ChunksManager.blockMat;

        buildPartGO.transform.eulerAngles = BuildMouseBehaviour.active.lastRot;

        //Communicate with mouse 
        BuildMouseBehaviour.active.clickTimer = 0;
        TempBuildComponent comp = new TempBuildComponent();
        comp.go = buildPartGO;
        comp.item = buildPart;
        comp.face = face;
        BuildMouseBehaviour.active.SetGOFollowMouse(comp);

    }

    private static Mesh MakeMesh(Item buildPart, Cube.Type face, int x, int y, int z)
    { 
        //Create the mesh
        MeshData meshForObject = new MeshData();
        meshForObject = GetItemMeshData.MeshForObject(x, y, z, meshForObject, buildPart, face);

        //Place values to the mesh
        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = meshForObject.vertices.ToArray();
        mesh.triangles = meshForObject.triangles.ToArray();
        mesh.uv = meshForObject.uv.ToArray();
        mesh.RecalculateNormals();
        return mesh;
    }

    public void AddBuildPartToBuilding(TempBuildComponent buildPart, Vector3 pos)
    {
        GameObject goToAddMeshTo = GetActiveBuilding(pos);
        buildPart.go.transform.SetParent(goToAddMeshTo.transform);
        goToAddMeshTo.AddComponent<CombineMeshes>();
    }

    private GameObject GetActiveBuilding(Vector3 pos)
    {
        //Make first starting building
        if (activeBuildings.Count == 0)
            AddNewBuilding(pos);

        //Select building to add to
        if (activeBuildings[activeBuildings.Count - 1].GetComponent<MeshFilter>().mesh.triangles.Length > 250)
            AddNewBuilding(pos);
        return activeBuildings[activeBuildings.Count - 1];
    }

    private GameObject AddNewBuilding(Vector3 pos)
    {
        GameObject goToAddMeshTo = new GameObject();
        //goToAddMeshTo.transform.position = pos;
        goToAddMeshTo.name = "Building "+activeBuildings.Count.ToString();
        goToAddMeshTo.AddComponent<MeshFilter>();
        goToAddMeshTo.AddComponent<MeshRenderer>();
        goToAddMeshTo.GetComponent<MeshRenderer>().sharedMaterial = ChunksManager.blockMat;
        activeBuildings.Add(goToAddMeshTo);
        return goToAddMeshTo;
    }

    private Mesh CombineMeshes(Mesh one, Mesh two)
    {
        var combine = new CombineInstance[2];
        combine[0].mesh = one;
        combine[0].transform = transform.localToWorldMatrix;
        combine[1].mesh = two;
        combine[1].transform = transform.localToWorldMatrix;

        Debug.Log(combine[1].transform);

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        return mesh;
    }
}
