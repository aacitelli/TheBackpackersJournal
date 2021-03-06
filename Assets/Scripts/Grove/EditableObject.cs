using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableObject : MonoBehaviour
{
    [Header("Gallery")]
    public GameObject galleryStorageGO;
    public string wallName;

    public Dictionary<GameObject, Vector3> storedOnThisWall;
    private GalleryStorage gallery;
    private BoxCollider validPlacement;
    // Start is called before the first frame update
    void Start()
    {
        gallery = galleryStorageGO.GetComponent<GalleryStorage>();
        validPlacement = gameObject.GetComponent<BoxCollider>();

        storedOnThisWall = new Dictionary<GameObject, Vector3>();
        //Load GalleryRoll
    }

    public void StoreOnWall(GameObject frame)
    {
        frame.name = wallName + storedOnThisWall.Count;
        print("New frame " + frame.name + " added to wall");
        Vector3 storePoint = frame.transform.position;
        //probably dont need
        frame.transform.position = storePoint;
        
        transform.InverseTransformPoint(storePoint);
        frame.transform.parent = transform;
        storedOnThisWall.Add(frame, storePoint);
        gallery.FinishStoragePlace(frame, storePoint);
    }

    public bool CanPlaceHere(GameObject frame)
    {
        Bounds validBounds = validPlacement.bounds;
        BoxCollider frameCol = frame.GetComponentInChildren<BoxCollider>();
        bool canPlace = validBounds.Intersects(frameCol.bounds);
        bool overlapDetect = CheckExistingFrames(frameCol.bounds);
        canPlace = canPlace && !overlapDetect;
        return canPlace;
    }

    public bool CheckExistingFrames(Bounds checkB)
    {
        bool overlapDetect = false;
        
        foreach(KeyValuePair<GameObject,Vector3> frame in storedOnThisWall)
        {
            BoxCollider frameCol = frame.Key.GetComponentInChildren<BoxCollider>();
            overlapDetect = checkB.Intersects(frameCol.bounds);
            if (overlapDetect)
            {
                Debug.Log("Overlap detected");
                break;
            }
        }

        return overlapDetect;
    }

    public void LoadFrame(GameObject newFrame, Vector3 storeCoords)
    {
        newFrame.transform.localPosition = storeCoords;
        Vector3 worldCoords = transform.TransformVector(storeCoords);
        Vector3 ptOnBound = validPlacement.ClosestPoint(worldCoords);
        storedOnThisWall.Add(newFrame, worldCoords);
        //newFrame.transform.LookAt(ptOnBound.normalized);
        //gallery.FinishStoragePlace(newFrame, storeCoords);
    }

    public bool DictChange(int removedIndex)
    {
        GameObject retHold = null;
        bool found = false;
        foreach(KeyValuePair<GameObject,Vector3> grabPair in storedOnThisWall)
        {

            if(grabPair.Key.name.Contains("" + removedIndex))
            {
                retHold = grabPair.Key;
                found = true;
                break;
            }
        }
        if(retHold != null)
        {
            storedOnThisWall.Remove(retHold);
        } else
        {
            print("frame not saved in searched wall");
        }
        
        return found;
    }
}

