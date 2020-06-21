
public class BaseData
{
    public void Save(IDataManager dataManager, bool isSaveNow = false)
    {
        dataManager.Save(this, isSaveNow);
    }
}
