[System.Serializable]

public class SummationNode : FunctionNode
{
    private static int param = 2;
    public SummationNode() : base(param)
    {

    }

    public override int fire(BotController bc)
    {
        int x = getInput();
        int y = getInput();
        return (x + y);
    }


}
