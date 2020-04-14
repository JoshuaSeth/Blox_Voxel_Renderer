using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMouseBehaviour : MonoBehaviour
{
    public static BuildMouseBehaviour active;
    public TempBuildComponent currentBuildPart;
    public TempBuildComponent previous;
    public Vector3 lastRot;
    public float clickTimer;

    private void Awake()
    {
        active = this;
    }

    // Update is called once per frame
    void Update()
    {
        clickTimer += Time.deltaTime;



        if (currentBuildPart != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 roundedPos;
                if (!Mathf.Approximately(0, currentBuildPart.go.transform.rotation.eulerAngles.y % 180))
                     roundedPos = new Vector3(Mathf.Round(hit.point.x*2)/2 + currentBuildPart.item.xOffSet, Mathf.Ceil(hit.point.y) + currentBuildPart.item.yOffSet, Mathf.Round(hit.point.z*2)/2);
                else
                     roundedPos = new Vector3(Mathf.Round(hit.point.x*2)/2, Mathf.Ceil(hit.point.y) + currentBuildPart.item.yOffSet, Mathf.Round(hit.point.z*2)/2 - currentBuildPart.item.zOffSet);

                if (Input.GetKeyUp(KeyCode.R))
                {
                    currentBuildPart.go.transform.Rotate(new Vector3(0, 90, 0));
                    lastRot = currentBuildPart.go.transform.eulerAngles;
                }
                currentBuildPart.go.transform.position = roundedPos;
            }



            if (Input.GetMouseButtonUp(0) && clickTimer > 0.1f)
            {
                Debug.Log("Clicker");
                BuildActions.active.AddBuildPartToBuilding(currentBuildPart, hit.point);
                previous = currentBuildPart;
                currentBuildPart = null;
                if (Input.GetKey(KeyCode.LeftAlt))
                    BuildActions.active.MakeBuildPart(previous.item, previous.face);
            }

        }

    }

    public void SetGOFollowMouse(TempBuildComponent comp)
    {
        currentBuildPart = comp;
    }

    public void DetachGOFromMouse()
    {
        currentBuildPart = null;
    }


}

public class TempBuildComponent{
    public GameObject go;
    public Item item;
    public Cube.Type face;}
