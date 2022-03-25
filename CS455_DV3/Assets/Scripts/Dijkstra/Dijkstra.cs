using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class Node : MonoBehaviour
{
    public GameObject go;

    public Node( GameObject g ){
        go = g;
    }
}
*/

public class Connection
{
    public Node fromNode;
    public Node toNode;

    public float getCost(){
        RaycastHit hit;
        Vector3 d = toNode.go.transform.position - fromNode.go.transform.position;
        if( Physics.Raycast(fromNode.go.transform.position, d, out hit, d.magnitude) && hit.transform.name != toNode.go.transform.name ){
            return (float)Mathf.Infinity;
        }

        return d.magnitude;
    }

}

public class Graph
{
    public Connection[] connections;

    public Connection[] getConnections( Node fromNode ){
        List<Connection> ln = new List<Connection>();

        for(int i=0; i<connections.Length; i++)
            if( connections[i].fromNode == fromNode )
                ln.Add( connections[i] );

        return ln.ToArray();
    }
}

public class Dijkstra : MonoBehaviour
{
    class NodeRecord{
        public Node node;
        public Connection connection;
        public float costSoFar;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    NodeRecord getSmallestElement( List<NodeRecord> l ){
        NodeRecord smallest = null;

        for(int i=0; i<l.Count; i++)
            if( smallest == null || smallest.costSoFar > l[i].costSoFar )
                smallest = l[i];

        return smallest;
    }

    NodeRecord findNode( List<NodeRecord> l, Node n ){
        NodeRecord ret = null;

        for(int i=0; i<l.Count; i++)
            if( l[i].node == n ){
                ret = l[i];
                break;
            }

        return ret;
    }

    public Connection[] pathFindDijkstra( Graph graph, Node start, Node end){

        NodeRecord startRecord = new NodeRecord();
        startRecord.node = start;
        startRecord.connection = null;
        startRecord.costSoFar = 0;

        List<NodeRecord> open = new List<NodeRecord>();
        open.Add( startRecord );
        List<NodeRecord> closed = new List<NodeRecord>();
        NodeRecord current = null;

        while( open.Count > 0 ){
            current = getSmallestElement( open );

            if( current.node == end )
                break;

            Connection[] connections = graph.getConnections( current.node );

            for(int i=0; i<connections.Length; i++){
                Node endNode = connections[i].toNode;
                NodeRecord endNodeRecord = null;
                float endNodeCost = current.costSoFar + connections[i].getCost();

                if( findNode( closed, endNode ) != null )
                    continue;
                else if( findNode( open, endNode ) != null ){
                    endNodeRecord = findNode( open, endNode );
                    if( endNodeRecord.costSoFar <= endNodeCost )
                        continue;
                }
                else{
                    endNodeRecord = new NodeRecord();
                    endNodeRecord.node = endNode;
                }
                endNodeRecord.costSoFar = endNodeCost;
                endNodeRecord.connection = connections[i];

                if( findNode(open, endNode) == null )
                    open.Add( endNodeRecord );
            }

            open.Remove( current );
            closed.Add( current );

        }

        if( current.node != end )
            return null;
        else{
            List<Connection> path = new List<Connection>();

            while( current.node != start ){
                path.Add( current.connection );
                current = findNode( closed, current.connection.fromNode );
            }

            path.Reverse();
            return path.ToArray();
        }
    }
}
