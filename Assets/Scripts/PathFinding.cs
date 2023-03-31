using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PathMarker
{
    public Vector3 location;
    public float H;
    public float G;
    public float F;
    public GameObject marker;
    public PathMarker parent;
    
    public PathMarker(Vector3 l, float h, float g, float f/*, GameObject marker*/, PathMarker p)
    {
        location = l;
        H = h;
        G = g;
        F = f;
        //this.marker = marker;
        parent = p;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        } else {
            return location.Equals(((PathMarker)obj).location);
        }

    }
    public override int GetHashCode()
    {
        return 0;
    }
}

public class PathFinding : MonoBehaviour
{

    [SerializeField] private Material _closedMat;
    [SerializeField] private Material _openMat;

    List<PathMarker> openList = new List<PathMarker>();
    List<PathMarker> closedList = new List<PathMarker>();

    [SerializeField] private GameObject _start;
    [SerializeField] private GameObject _end;
    [SerializeField] private GameObject _pathCells;

    PathMarker goalCell;
    PathMarker startCell;
    PathMarker lastCell;


    bool done;

    public void RemoveAllMarkers()
    {
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
        foreach (GameObject marker in markers)
        {
            Destroy(marker);
        }
    }
    

    public void BeginSearch()
    {
        done = false;
        RemoveAllMarkers();

        Vector3 startLocation = _start.transform.position;
        startCell = new PathMarker(startLocation, 0, 0, 0/*, Instantiate(_pathCells, startLocation, Quaternion.identity)*/, null);

        Vector3 goalLocation = _end.transform.position;
        goalCell = new PathMarker(goalLocation, 0, 0, 0/*, Instantiate(_pathCells, goalLocation, Quaternion.identity)*/, null);
        lastCell = startCell;
        closedList.Clear();
 
        do
        {
            Search(lastCell);
        } while (!done);
        Debug.Log("path found");

    }
 
    public void Search(PathMarker thisCell)
    {
        Vector3 dir = Vector3.zero;
        if (thisCell.Equals(goalCell))
        {
            done = true;
            return;
        }
        else
        {
            for(int i = 0; i < 8; i++)
            {
                switch (i)
                {
                    case 0:
                        dir = new Vector3(1f, 0, 0);
                        break;
                    case 1:
                        dir = new Vector3(0, 0, 1f);
                        break;
                    case 2:
                        dir = new Vector3(-1f, 0, 0);
                        break;
                    case 3:
                        dir = new Vector3(0, 0, -1f);
                        break;
                    case 4:
                        dir = new Vector3(1f, 0, 1f);
                        break;
                    case 5:
                        dir = new Vector3(1f, 0, -1f);
                        break;
                    case 6:
                        dir = new Vector3(-1f, 0, 1f);
                        break;
                    case 7:
                        dir = new Vector3(-1f, 0, -1f);
                        break;
                        /*
                    case 0:
                        dir = new Vector3(0.5f, 0, 0);
                        break;
                    case 1:
                        dir = new Vector3(0, 0, 0.5f);
                        break;
                    case 2:
                        dir = new Vector3(-0.5f, 0, 0);
                        break;
                    case 3:
                        dir = new Vector3(0, 0, -0.5f);
                        break;
                    case 4:
                        dir = new Vector3(0.5f, 0, 0.5f);
                        break;
                    case 5:
                        dir = new Vector3(0.5f, 0, -0.5f);
                        break;
                    case 6:
                        dir = new Vector3(-0.5f, 0, 0.5f);
                        break;
                    case 7:
                        dir = new Vector3(-0.5f, 0, -0.5f);
                        break;
                        */

                }
                Vector3 neighbourCellLoc = thisCell.location + dir;
 
                Collider[] hitColliders = Physics.OverlapBox(neighbourCellLoc, _pathCells.transform.localScale/2, Quaternion.identity);
                if(hitColliders.Length != 0 && hitColliders[0] != _start.gameObject.GetComponent<BoxCollider>())
                {
                    foreach(Collider c in hitColliders)
                    {                      
                        if (c == _end.gameObject.GetComponent<BoxCollider>())
                        {
                            done= true;                            
                        }
                    }
                    continue;      
                }
                else if (IsClosed(neighbourCellLoc) == false && IsWalkable(neighbourCellLoc, thisCell) == false)
                {                  
                    float g = thisCell.G + 0.5f;
                    float h = Vector3.Distance(neighbourCellLoc, goalCell.location);
                    float f = g + h;

                    PathMarker neighbourCell = new PathMarker(neighbourCellLoc, h, g, f/*, Instantiate(_pathCells, neighbourCellLoc, Quaternion.identity)*/, thisCell);
                    openList.Add(neighbourCell);
                }
            }
            if(!done)
            {
                openList = openList.OrderBy(p => p.F).ToList<PathMarker>();
                PathMarker pm = (PathMarker)openList.ElementAt(0);
                Debug.Log(pm.location);
                closedList.Add(pm);
                /*
                TextMesh[] values = pm.marker.GetComponentsInChildren<TextMesh>();
                values[2].text = "G " + pm.G.ToString("0.00");
                values[1].text = pm.H.ToString("0.00");
                values[0].text = pm.F.ToString("0.00");
                */

                openList.RemoveAt(0);
              // pm.marker.GetComponent<Renderer>().material = _closedMat;
                lastCell = pm;
            }
        }
    }
    
    public bool IsClosed(Vector3 markerLoc)
    {
        /*
        if (closedListT[Mathf.RoundToInt(markerLoc.x), Mathf.RoundToInt(markerLoc.z)] == true)
        {
            return true;
        }
        */

        foreach(PathMarker pathMarker in closedList)
        {            
            if(pathMarker.location.Equals(markerLoc)){
                return true;
            }
            
          /*
            Collider[] hitColliders = Physics.OverlapBox(markerLoc, _pathCells.transform.localScale / 2, Quaternion.identity);

            if(hitColliders.Length == 1 && hitColliders[0] == pathMarker.marker)
            {
                return true;
            }
          */
        }
        return false;
    }
    public bool IsWalkable(Vector3 markerLoc, PathMarker markerParent)
    {
        int layerMask = 1 << 30;
        layerMask = ~layerMask;
        RaycastHit hit;
        float distance = (markerLoc - markerParent.location).magnitude;
        Vector3 direction = (markerLoc - markerParent.location) / (markerLoc - markerParent.location).magnitude;

        return Physics.Raycast(markerParent.location, direction, out hit, distance, layerMask);
        
    }
    public bool IsAtEnd(PathMarker marker)
    {
        foreach (PathMarker pathMarker in closedList)
        {
            Collider[] hitColliders = Physics.OverlapBox(marker.location, _pathCells.transform.localScale / 2, Quaternion.identity);

            if (hitColliders.Length == 1 && hitColliders[0] == pathMarker.marker)
            {
                return true;
            }
        }
        return false;
    }

    public void GetPath()
    {
        RemoveAllMarkers();
        PathMarker begin = lastCell;

        while(begin != null && !IsAtEnd(begin))
        {
            GameObject path = Instantiate(_pathCells, begin.location, Quaternion.identity);
            path.GetComponent<Renderer>().material = _closedMat;
            begin = begin.parent;
        }
        Instantiate(_pathCells, startCell.location, Quaternion.identity);
        done = false;
    }

    void Start()
    {
         
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            BeginSearch();
        }
/*
       if (!done && startCell != null)
        {
            Search(lastCell);            
        }
*/
        if(done)
        {
            GetPath();
        }
  
    }

}
