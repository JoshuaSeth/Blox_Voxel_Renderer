using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

/// <summary>
/// Keeps track of which chunks are in view
/// </summary>
public class Frustrum : MonoBehaviour
{
    public static Camera cam;

    Vector3 lastPos = new Vector3();
    Vector3 lastRot = new Vector3();
    float lastSize;


    /// <summary>
    /// Coordinates of corners of frustrum. 0 = middle, 1 = top right, 2 = top left, 3 = bottom left, 4 = bottom right
    /// </summary>
    public static Vector3[] viewCorners = new Vector3[5];


    public static bool isUpdated;

    public static int currentMinChunkPosXInView, currentMaxChunkPosXInView, currentMinChunkPosZInView, currentMaxChunkPosZInView;

    public static Vector2Int PreviousLowestChunkPos, PreviousHighestChunkPos, CurrentMinChunkPos, CurrentMaxChunkPos;

    public static float xRemain, zRemain, xChunkCount, zChunkCount, downCornXRemain, downCornZRemain, downCornChunkPosX, downCornChunkPosZ;

    public enum Corner { Middle, UpRight, UpLeft, DownLeft, DownRight}


    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void Start()
    {
        StartCoroutine(UpdateFrustrumData());
    }


    public IEnumerator UpdateFrustrumData()
    {
        SetFrustrumCorners();

        //only if the camera moved
        if (Settings.frustrumUpdateOnlyWhenMoved)
            if (Vector3.Distance(lastPos, cam.transform.position) > 0.001f || Vector3.Distance(lastRot, cam.transform.rotation.eulerAngles) > 0.5f || Mathf.Abs(lastSize - cam.orthographicSize) > 0.1f)
                UpdateFrustrumInfo();



        //Refresh
        if (Mathf.Approximately(Settings.frustrumDataRefreshRate, 0))
        {
            yield return null;
            StartCoroutine(UpdateFrustrumData());
        }
        else
        {
            yield return new WaitForSeconds(Settings.frustrumDataRefreshRate);
            StartCoroutine(UpdateFrustrumData());
        }
    }

    private void UpdateFrustrumInfo()
    {
        //Save data of previous calculation
        PreviousLowestChunkPos = CurrentMinChunkPos;
        PreviousHighestChunkPos = CurrentMaxChunkPos;

        //Save the position to detect camera movement
        lastPos = cam.transform.position;
        lastRot = cam.transform.rotation.eulerAngles;
        lastSize = cam.orthographicSize;


        Vector3 cornPos1 = CornerCoordinate(Corner.UpRight);
        Vector3 cornPos2 = CornerCoordinate(Corner.DownLeft);

        //Calculation to get to the corners and the chunk keys between them
        Vector3 cornersDist = cornPos1 - cornPos2;




        xRemain = cornersDist.x % 8;
        xChunkCount = (cornersDist.x + (8-xRemain)) / 8; 
        downCornXRemain = cornPos2.x % 8;
        downCornChunkPosX = (cornPos2.x - (8 - downCornXRemain)) / 8;

        zRemain = cornersDist.z % 8;
        zChunkCount = (cornersDist.z + (8-zRemain)) / 8;
        downCornZRemain = cornPos2.z % 8;
        downCornChunkPosZ = (cornPos2.z -(8- downCornZRemain)) / 8;

        currentMinChunkPosXInView = Mathf.FloorToInt(downCornChunkPosX);
        currentMinChunkPosZInView = Mathf.FloorToInt(downCornChunkPosZ);
        currentMaxChunkPosXInView = Mathf.CeilToInt(downCornChunkPosX + xChunkCount);
        currentMaxChunkPosZInView = Mathf.CeilToInt(downCornChunkPosZ + zChunkCount);


        CurrentMinChunkPos = new Vector2Int(Mathf.CeilToInt(downCornChunkPosX), Mathf.CeilToInt(downCornChunkPosZ));
        CurrentMaxChunkPos = new Vector2Int(Mathf.CeilToInt(currentMaxChunkPosXInView), Mathf.CeilToInt(currentMaxChunkPosZInView));

        //Debug.Log("Previous: " +PreviousLowestChunkPos.ToString() + " " + PreviousHighestChunkPos.ToString());
        //Debug.Log("Current: "+CurrentMinChunkPos.ToString() + " " + CurrentMaxChunkPos.ToString());

        isUpdated = true;
    }

    private void SetFrustrumCorners()
    {
        viewCorners[0] = ScreenToWorldCoord(Screen.width/ 2, Screen.height/ 2);
        viewCorners[1] = ScreenToWorldCoord(Screen.width, Screen.height);
        viewCorners[2] = ScreenToWorldCoord(0, Screen.height);
        viewCorners[3] = ScreenToWorldCoord(0, 0);
        viewCorners[4] = ScreenToWorldCoord(Screen.width, 0);
    }

    public Vector3 CornerCoordinate(Corner corner)
    {
        if (corner == Corner.Middle)
            return viewCorners[0];
        if (corner == Corner.UpRight)
            return viewCorners[1];
        if (corner == Corner.UpLeft)
            return viewCorners[2];
        if (corner == Corner.DownLeft)
            return viewCorners[3];
        if (corner == Corner.DownRight)
            return viewCorners[4];
        return Vector3.zero;
    }


    private Vector3 ScreenToWorldCoord(float screenXCd, float screenYCd)
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(screenXCd, screenYCd));
        // create a plane at 0,0,0 whose normal points to +Y:
        Plane hPlane = new Plane(Vector3.up, new Vector3(0,-50,0));
        // Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
        // if the ray hits the plane...
        if (hPlane.Raycast(ray, out float distance))
            return ray.GetPoint(distance);
        return Vector3.zero;
    }

}
