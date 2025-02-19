using NUnit.Framework;
using System.Collections.Generic;

public class SupplyNode : Node
{
    int supply {  get; set; } 

    public SupplyNode(string id, int matrix_position, int supply): base(id, matrix_position)
    {

        this.id = id;
        this.supply = supply;

    }
    public int AvailableSupply()
    {
        int missingSupply = supply;
        foreach (var edge in this.Edges)
        {
            missingSupply -= edge.flow;
        }

        return missingSupply;
    }
}