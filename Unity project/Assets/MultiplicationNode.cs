[System.Serializable]

public class MultiplicationNode : FunctionNode
{
    public MultiplicationNode() : base(2){}

    public override int fire(BotController bc)
    {
        int x = getInput();
        int y = getInput();
        return x * y;
    }

}
