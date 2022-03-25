using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower_Dijkstra : Kinematic
{
    public GameObject[] targets;
    public GameObject start;
    public GameObject end;
    Node[] nodes;

    bool flag = false;
    
    PathFollow myMoveType;
    LookWhereGoing myRotateType;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        Node s = null;
        Node e = null;

        yield return new WaitForSeconds(0.1f);
        flag = true;

        // Cast all targets into Nodes
        nodes = new Node[targets.Length];
        for(int i=0; i<targets.Length; i++){
            nodes[i] = targets[i].GetComponent<Node>();
            if( targets[i] == start )
                s = nodes[i];
            else if( targets[i] == end )
                e = nodes[i];
        }

        // Create connections between nodes
        List<Connection> conns = new List<Connection>();
        for(int i=0; i<nodes.Length; i++){
            for(int j=0; j<nodes[i].pathsTo.Length; j++){
                Connection c = new Connection();
                c.fromNode = nodes[i];
                c.toNode = nodes[i].pathsTo[j];
                conns.Add( c );
            }
        }



        // Create a graph of the connections
        Graph g = new Graph();
        g.connections = conns.ToArray();

        // Find the optimal path to the target node
        Dijkstra d = new Dijkstra();
        Connection[] path_conn = d.pathFindDijkstra( g, s, e );
        GameObject[] path = new GameObject[path_conn.Length+1];
        for(int i=0; i<path_conn.Length; i++)
            path[i] = path_conn[i].fromNode.go;
        path[path.Length-1] = path_conn[path_conn.Length-1].toNode.go;

        // Send the path to the path follower script
        myMoveType = new PathFollow();
        myMoveType.character = this;
        myMoveType.targets = path;

        myRotateType = new LookWhereGoing();
        myRotateType.character = this;
        myRotateType.target = myTarget;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if( !flag ) 
            return;
            
        steeringUpdate = new SteeringOutput();
        steeringUpdate.linear = myMoveType.getSteering().linear;
        steeringUpdate.angular = myRotateType.getSteering().angular;
        base.Update();
    }
}
