using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PathRequaestManager : MonoBehaviour
{
    Queue<PathResult> results = new Queue<PathResult>();
   
    FindPath findPath;

  
    static PathRequaestManager instance;

    private void Awake()
    {
        instance = this;
        findPath = GetComponent<FindPath>();
    }
    private void Update()
    {
        if(results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                for(int i = 0; i < itemsInQueue; i++)
                {
                    PathResult result = results.Dequeue();
                    result.callBack(result.path, result.sucess);
                }
            }
        }
    }

    public static void RequestPath(PathRequest request)
    {
        ThreadStart threadStart = delegate
        {
            instance.findPath.PathFinding(request, instance.FinishedProcessingPath);
        };
        threadStart.Invoke();
    }

   

    public void FinishedProcessingPath(PathResult result)
    {
        lock (results)
        {
            results.Enqueue(result);
        }
    }
}

public struct PathResult
{
    public Vector3[] path;
    public bool sucess;
    public Action<Vector3[], bool> callBack;

    public PathResult(Vector3[] _path, bool _sucess, Action<Vector3[], bool> _callBack)
    {
        this.path = _path;
        this.sucess = _sucess;
        this.callBack = _callBack;
    }
}
public struct PathRequest
{
    public Vector3 pathStart;
    public Vector3 pathEnd;
    public Action<Vector3[], bool> callBack;

    public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callBack)
    {
        this.pathStart = _start;
        this.pathEnd = _end;
        this.callBack = _callBack;
    }
}
