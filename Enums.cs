namespace TextRPG
{
    //병과 (직업)
    public enum JobType
    {
        None = 0,

        Cavalry,    // 기병 🐎 (보병에 강함)
        Infantry,   // 보병 🛡️ (궁병에 강함)
        Archer,     // 궁병 🏹 (창병에 강함)
        Spearman,   // 창병 🔱 (기병에 강함)
        Tactician,   // 책사 📜 (상성 무시)

        //적 직업
        Bandit       //도적(황건적용)
    }

    //세력 (나중에 스토리 분기 때 사용)
    public enum Faction
    {
        None = 0, //재야 (무소속)
        Wei,    // 위(조조) 
        Shu,    //촉(유비)
        Wu      //오(손권)
    }


    //[추가] 장수 등급
    public enum Rank
    {
    N, //일반
    R, //희귀
    SR, //네임드
    SSR //전설
    }
}