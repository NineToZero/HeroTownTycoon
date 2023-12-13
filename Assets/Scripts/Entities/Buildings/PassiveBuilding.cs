using System.Collections.Generic;
using UnityEngine;

public class PassiveBuilding : BaseBuilding
{
    [SerializeField] private Rigidbody _rigid;
    [SerializeField] private Collider _collider;

    private PassiveBuildingData _passiveData;
    public IBuildingAbility Ability;
    private bool isActivation;
    private List<BaseBuilding> detectivedBuilding = new List<BaseBuilding>();

    public BuildingPassiveType PassiveType { get { return _passiveData.PassiveType; } }
    public float Range { get { return _passiveData.Range; } }
    public float Value { get { return _passiveData.Value; } }
    public List<BaseBuilding> DetectivedBuilding { 
        get
        {
            return detectivedBuilding;
        }
        set 
        {
            detectivedBuilding = value;
        }
    }


    private void OnDisable()
    {
        Ability.Deactivation();
        isActivation = false;
    }


    public override void Init(BuildingType type, bool isUpgrade = false)
    {
        base.Init(type);
        _passiveData = Managers.Instance.DataManager.PassiveBuildings[type];

        switch (PassiveType)
        {
            case BuildingPassiveType.Water:
                Ability = new AbilityGroundStat();
                break;
            case BuildingPassiveType.HeroCount:
                Ability = new AbilityHeroStat();
                break;
            default:
                break;
        }

        _collider.enabled = (int)PassiveType < 2000; // 코드 리뷰 : 콜라이더 켰다 끄는 게 좋나요?

        if (isActivation)
            return;

        Managers.Instance.DayManager.DayChangeEvent += Ability.Activation;

        if (Ability is IColliderProcessable)
        {
            IColliderProcessable cAbility = Ability as IColliderProcessable;
            cAbility.ColliderInit(detectivedBuilding);
        }

        Ability.Init(_passiveData);
        isActivation = true;
    }

    public override void Interact()
    {
        base.Interact();
    }
}

public interface IBuildingAbility
{
    public void Init(PassiveBuildingData data);
    public void Activation();
    public void Deactivation();
}

public class AbilityHeroStat : IBuildingAbility
{
    private PassiveBuildingData _data;

    public void Init(PassiveBuildingData data)
    {
        _data = data;
    }

    public void Activation()
    {
        // TO DO 히어로 스탯 증가
    }

    public void Deactivation()
    {
    }
}

public class AbilityGroundStat : IColliderProcessable, IBuildingAbility
{
    private List<BaseBuilding> _baseBuilding;
    private PassiveBuildingData _data;

    public void ColliderInit(List<BaseBuilding> baseBuilding)
    {
        _baseBuilding = baseBuilding;
    }

    public void Init(PassiveBuildingData data)
    {
        _data = data;
    }

    public void Activation()
    {
        Farmland farmland;
        for (int i = 0; i < _baseBuilding.Count; i++)
        {
            if (!(farmland = _baseBuilding[i] as Farmland))
                return;

            switch (_data.PassiveType)
            {
                case BuildingPassiveType.Water:
                    farmland.WaterRatio += (int)_data.Value;
                    break;
            }
        }
    }

    public void Deactivation()
    {
    }
}