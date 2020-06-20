using CsMessage;
using UnityEngine;

public class BloomSettingItem : MsgMonoBehaviour
{
    public Behaviour bloomComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        // bloomComponent.enabled = DataManager.Instance.settingData.isOpenBloom;
        //
        // AddMsg(GameEvent.BLOOM_TOGGLE_CHANGED, OnBloomToggleChanged);
    }

    private void OnBloomToggleChanged(object[] senders)
    {
        var isOpen = (bool) senders[0];
        bloomComponent.enabled = isOpen;
    }
}
