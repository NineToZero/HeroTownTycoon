public class IndividualityStat
{
    public string Name;
    public int OriginCode;
    public int NatureCode;
    public int JobCode;
    public int FlavorCode;

    public int SpriteCode;

    public IndividualityStat()
    {
        //Only for Easy Save 3 !!!
    }
    
    public IndividualityStat(IndividualityStatSO individualityStatSO)
    {
        Name = $"{individualityStatSO.FirstName.GetRandomValueFromArray()}{individualityStatSO.MiddleName.GetRandomValueFromArray()}{individualityStatSO.LastName.GetRandomValueFromArray()}";

        OriginCode = individualityStatSO.Origins.GetRandomValueFromArray().id;
        NatureCode = individualityStatSO.Natures.GetRandomValueFromArray().id;
        FlavorCode = individualityStatSO.Flavors.GetRandomValueFromArray().id;

        Job job = individualityStatSO.Jobs.GetRandomValueFromArray();
        JobCode = job.id;
        SpriteCode = job.SpriteIds.GetRandomValueFromArray();
    }
}
