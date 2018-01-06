[System.Serializable]

public class SubtractionNode : FunctionNode
{
    private static int param = 2;
    public SubtractionNode() : base(param)
    {
    }

    public override int fire(BotController bc)
    {
        int x = getInput();
        int y = getInput();
        return (x - y);
    }

}
