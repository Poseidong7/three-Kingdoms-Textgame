namespace TextRPG
{
    // [인터페이스] : 껍데기만 있는 설계도
    // "모든 스킬은 반드시 Cast라는 함수를 가지고 있어야 한다"고 강제함

    public interface ISkill
    {
        string Name {get;} //스킬 이름
        int MpCost {get;} //마나 소모량

        //스킬 사용 행동 (시전자가 타겟을 때림)
        void Cast(Unit caster, Unit target);
    }
}