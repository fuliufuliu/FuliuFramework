
public class BaseData
{
    public void Save(bool isSaveNow = false)
    {
        DataManager.Instance.Save(this, isSaveNow);
    }
}
