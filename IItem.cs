namespace TextRPG
{
    //아이템이라면 가져야 할 기본 기능
    public interface IItem
    {
        string Name {get;} //아이템 이름
        
        //아이템 사용 (누가 누구에게?)
        //보통 물약 자신, 폭탄은 상대방에게 쓸 수 있으니
        void Use(Unit target);
    }
}