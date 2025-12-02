namespace TextRPG
{
    //병과 (직업)
    public enum JobType
    {
        None = 0,
        Warlord, //맹장(전사)
        Strategist //책사(마법사)
    }

    //세력 (나중에 스토리 분기 때 사용)
    public enum Faction
    {
        None = 0, //재야 (무소속)
        Wei,    // 위(조조) 
        Shu,    //촉(유비)
        Wu      //오(손권)
    }
}