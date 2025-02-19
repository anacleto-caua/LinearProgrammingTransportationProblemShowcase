using System;

public class DemandNode : Node
{
    int demand { get; set; }

    public DemandNode(string id, int matrix_position, int demand) : base(id, matrix_position)
    {
        this.demand = demand;
    }

    public int MissingDemand()
    {
        int missingDemand = demand;
        foreach (var edge in this.Edges)
        {
            missingDemand -= edge.flow;
        }

        return missingDemand;
    }

}