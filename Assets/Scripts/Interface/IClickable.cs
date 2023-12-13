public interface IClickable
{
    public void Click(int handledItemId, out int consumedCount);
    public string GetName();
}
