using System;
using System.Collections.Generic;
using UnityEngine;

public class MaestroController : MonoBehaviour
{

    public List<SupplyNode> SupplyNodes;
    public List<DemandNode> DemandNodes;

    public TransportMatrix TransportMatrix;

    void Start()
    {
        // Start by setting the problem
        // - Example by manual setting the data
        SupplyNodes = new List<SupplyNode>
        {
            new SupplyNode("s" + 1, 0, 10),
            new SupplyNode("s" + 2, 1, 30),
            new SupplyNode("s" + 3, 2, 20)
        };

        DemandNodes = new List<DemandNode>
        {
            new DemandNode("d" + 1, 0, 20),
            new DemandNode("d" + 2, 1, 15),
            new DemandNode("d" + 3, 2, 25)
        };

        // - Creating the transport matrix
        TransportMatrix = new TransportMatrix(SupplyNodes, DemandNodes);

        // - Filling the costs for each path
        float[,] costsMatrix = {
            { 5, 2, 14},
            { 15, 9, 7},
            { 6, 12, 9}};
       
        TransportMatrix.FillMatrix(costsMatrix);

        // - Creating a feasible solution
        TransportMatrix.FindFeasibleSolutionLeastCostMethod();

        // - Print solution out(debug only)
        TransportMatrix.PrintCurrentSolution();
    }

    void Update()
    {

    }
}
