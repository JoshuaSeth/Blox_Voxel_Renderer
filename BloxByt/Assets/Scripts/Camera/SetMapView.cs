using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMapView : MonoBehaviour
{
    bool mapView = false;

    public void ChangeMapView()
    {
        if (!mapView)
        {
            mapView = true;
            Frustrum.cam.transform.rotation = Quaternion.Euler(new Vector3(90, Frustrum.cam.transform.rotation.eulerAngles.y, Frustrum.cam.transform.rotation.eulerAngles.z));
            Settings.onlyRenderTopFaces = true;
        }
        else if (mapView)
        {
            mapView = false;
            Frustrum.cam.transform.rotation = Quaternion.Euler(new Vector3(58, Frustrum.cam.transform.rotation.eulerAngles.y, Frustrum.cam.transform.rotation.eulerAngles.z));
            Settings.onlyRenderTopFaces = false;
        }
    }
}
