public class TransportEdge
{
    public string id { get; set; }

    public float cost { get; set; }
    public SupplyNode Source { get; set; }
    public DemandNode Sink { get; set; }

    public int flow = 0;

    public TransportEdge(string id, float cost, SupplyNode Source, DemandNode Sink)
    {
        this.id = id;
        this.cost = cost;
        this.Source = Source;
        this.Sink = Sink;
        this.flow = 0;
    }

}