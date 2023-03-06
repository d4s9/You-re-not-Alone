using System.Collections;
using System.Collections.Generic;
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
    
    public PathMarker(Vector3 l, float h, float g, float f, GameObject marker, PathMarker p)
    {
        location = l;
        H = h;
        G = g;
        F = f;
        this.marker = marker;
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

    bool done;

    public void BeginSearch()
    {
        Vector3 startLocation = _start.transform.position;
        startCell = new PathMarker(startLocation, 0, 0, 0, Instantiate(_pathCells, startLocation, Quaternion.identity), null);

        Vector3 goalLocation = _end.transform.position;
        goalCell = new PathMarker(goalLocation, 0, 0, 0, Instantiate(_pathCells, goalLocation, Quaternion.identity), null);
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
            for(int i = 1; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        dir = new Vector3(1, 0, 0);
                        break;
                    case 1:
                        dir = new Vector3(0, 0, 1);
                        break;
                    case 2:
                        dir = new Vector3(-1, 0, 0);
                        break;
                    case 3:
                        dir = new Vector3(0, 0, -1);
                        break;
                }
                Vector3 neighbourCellLoc = thisCell.location + dir;
 
                Collider[] hitColliders = Physics.OverlapBox(neighbourCellLoc, _pathCells.transform.localScale/2, Quaternion.identity);
                if(hitColliders.Length != 0 )
                {
                    continue;
                }

            }
        }
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
    }

}
