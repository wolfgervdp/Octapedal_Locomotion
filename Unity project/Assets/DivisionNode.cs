[System.Serializable]

public class DivisionNode : FunctionNode
{
    private static int param = 2;

    public DivisionNode() : base(param)
    {
    }

    public override int fire(BotController bc)
    {
        int x = this.getInput();
        int y = this.getInput();
        if (y > 0) {
            return x / y;

        }
        return 1;// just to get something different during testing...

    }

}
