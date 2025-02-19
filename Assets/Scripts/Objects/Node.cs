using System.Collections.Generic;

public class Node
{
    public string id { get; set; }
    
    public int matrix_position;

    public List<TransportEdge> Edges { get; set; }

    public Node(string id, int matrix_position)
    {
        this.id = id;
        this.matrix_position = matrix_position;

        Edges = new List<TransportEdge>();

    }

    public void AddEdge(TransportEdge edge)
    {
        Edges.Add(edge);
    }
}