using UnityEngine;

public class PlayerDataManager : SingletonMonobehaviour<PlayerDataManager>
{
    private SOPlayerData playerDataRuntime = default;
    public SOPlayerData PlayerDataRuntime => playerDataRuntime;

    private SaveDataSlot activeSlot = default;

    //===========================================================================
    protected override void Awake()
    {
        base.Awake();

        playerDataRuntime = ScriptableObject.CreateInstance<SOPlayerData>();
    }

    private void OnDisable()
    {
        Destroy(playerDataRuntime);
    }

    //===========================================================================
    public void TransferData(SOPlayerData saveData)
    {
        playerDataRuntime.TransferData(saveData);

        Player.Instance.GetComponent<PlayerMovement>().UpdateMovementParameters();
        Player.Instance.GetComponentInChildren<BasicAbility>().UpdateAbilityParameters();
        Player.Instance.GetComponentInChildren<RangeAbility>().UpdateAbilityParameters();
        Player.Instance.GetComponentInChildren<BombAbility>().UpdateAbilityParameters();
    }

    public void SetActiveSlot(SaveDataSlot newActiveSlot)
    {
        activeSlot = newActiveSlot;
    }

    public void SaveRunTimeDataToPlayerDataSlot()
    {
        SaveDataManager.Instance.SaveRuntimeDataToPlayerDataSlot(activeSlot, playerDataRuntime);
    }
}