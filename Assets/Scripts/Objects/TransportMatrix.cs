using System;
using System.Collections.Generic;
using UnityEngine;
public class TransportMatrix
{
    public List<DemandNode> DemandNodes { get; set; }
    public List<SupplyNode> SupplyNodes { get; set; }

    public TransportEdge[,] Matrix { get; set; }

    public TransportMatrix(List<SupplyNode> SupplyNodes, List<DemandNode> DemandNodes)
    {
        this.SupplyNodes = SupplyNodes;
        this.DemandNodes = DemandNodes;

        Matrix = new TransportEdge[SupplyNodes.Count, DemandNodes.Count];
        TransportEdge edge;
        for (int i = 0; i < Matrix.GetLength(0); i++)
        {
            for (int j = 0; j < Matrix.GetLength(1); j++)
            {
                edge = new TransportEdge("e-" + i + "_" + j, -1, SupplyNodes[i], DemandNodes[j]);
                Matrix[i, j] = edge;
                SupplyNodes[i].AddEdge(edge);
                DemandNodes[j].AddEdge(edge);
            }
        }
    }

    public void FindFeasibleSolutionLeastCostMethod()
    {
        List<TransportEdge> ignoredEdges = new List<TransportEdge>();
        TransportEdge lowestEdge = Matrix[0, 0];
        
        while (ignoredEdges.Count < (SupplyNodes.Count * DemandNodes.Count))
        {
            // Find the lowest cost edge
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    if (!ignoredEdges.Contains(Matrix[i, j]))
                    {
                        if(lowestEdge == null)
                        {
                            lowestEdge = Matrix[i, j];
                        }
                     
                        if ( Matrix[i, j].cost < lowestEdge.cost )
                        {
                            lowestEdge = Matrix[i, j];
                        }
                    }
                }
            }

            // Threats this edge
            if (lowestEdge != null)
            {
                int missingDemand = lowestEdge.Sink.MissingDemand();
                if (missingDemand > 0)
                {
                    int availableSupply = lowestEdge.Source.AvailableSupply();
                    if (availableSupply > 0)
                    {
                        //:TOCLEAN: Someone should clean this if messy monstrosity
                        int transferSupply = 0;

                        if (missingDemand < availableSupply)
                        {
                            transferSupply = missingDemand;
                            //This Sink is full! Ignore all Transport Edges leading to it
                            for(int i = 0; i < SupplyNodes.Count; i++)
                            {
                                AddTransportEdgeUnique(ignoredEdges, Matrix[i, lowestEdge.Sink.matrix_position]);
                            }
                        }else if(availableSupply < missingDemand)
                        {
                            transferSupply = availableSupply;
                            //This Source is empty! Ignore all Transport Edges getting out of it
                            for (int j = 0; j < DemandNodes.Count; j++)
                            {
                                AddTransportEdgeUnique(ignoredEdges, Matrix[lowestEdge.Source.matrix_position, j]);
                            }
                        }else
                        {
                            transferSupply = availableSupply;
                            //Both the Sink will be full and the Source empty
                            for (int i = 0; i < SupplyNodes.Count; i++)
                            {
                                AddTransportEdgeUnique(ignoredEdges, Matrix[i, lowestEdge.Sink.matrix_position]);
                            }
                            for (int j = 0; j < DemandNodes.Count; j++)
                            {
                                AddTransportEdgeUnique(ignoredEdges, Matrix[lowestEdge.Source.matrix_position, j]);
                            }
                        }

                        lowestEdge.flow = transferSupply;
                        lowestEdge = null;
                    }
                }
            }
        }
    }

    public void AddTransportEdgeUnique(List<TransportEdge> edgesList, TransportEdge newEdge)
    {
        if (!edgesList.Contains(newEdge))
        {
            edgesList.Add(newEdge);
        }
    }

    public void PrintCurrentSolution()
    {
        for(int i = 0; i < SupplyNodes.Count; i++)
        {
            for(int j = 0; j < DemandNodes.Count; j++)
            {
                Debug.Log("[ " + Matrix[i, j].flow + " * " + Matrix[i, j].cost + " ] --- ");
            }
            Debug.Log("");
        }
    }

    public void FillMatrix(float[,] newCostsMatrix)
    {
        if (Matrix.GetLength(0) != newCostsMatrix.GetLength(0) || Matrix.GetLength(1) != newCostsMatrix.GetLength(1))
        {
            throw new ArgumentException("Matrix dimensions do not match.");
        }

        for (int i = 0; i < newCostsMatrix.GetLength(0); i++) 
        {
            for(int j = 0; j < newCostsMatrix.GetLength(1); j++)
            {
                Matrix[i,j].cost = newCostsMatrix[i,j];
            }
        }
    }

    public float Cost()
    {
        float cost = 0;
        for (int i = 0; i < SupplyNodes.Count; i++)
        {
            for (int j = 0; j < DemandNodes.Count; j++)
            {
                cost += Matrix[i, j].flow * Matrix[i, j].cost;
            }
        }

        return cost;
    }

}