using EasyExcel;

public class ConfigDataManager : SingleBhv<ConfigDataManager>
{
    private readonly EEDataManager _eeDataManager = new EEDataManager();
    
    public EEDataManager Datas => _eeDataManager;
    
    public override void Initial()
    {
        base.Initial();
        
        if (Instance && Instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();
    }

    public override void Init()
    {
        isInited = true;
        base.Init();
        
        _eeDataManager.Load();
//        var list = Datas.Get<Data_Skin.Base_Row>(1001);
//        foreach (var priceKeyValue in list.priceDic)
//        {
//            Debug.LogError($"------------- 1001 price : {priceKeyValue.Key}:{priceKeyValue.Value}");
//        } 
    }

}
