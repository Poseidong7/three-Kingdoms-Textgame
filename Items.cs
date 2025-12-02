using System;

namespace TextRPG
{
    // [체력 물약 설정]
    public class HealthPotion : IItem
    {
        public string Name => "빨간 포션"; //이름

        public void Use(Unit target)
        {
            Console.WriteLine($"\n 🧉 {target.Name}이(가) [빨간 포션]을 사용했습니다!");

            // 체력 30회복
            target.Hp += 30;
            if (target.Hp > target.MaxHp) target.Hp = target.MaxHp; //최대 체력을 넘지 않게
            
            Console.WriteLine($"    => 체력이 회복되었습니다. (현재 HP : {target.Hp})");
        }
    }

    // [마나 물약]
    public class ManaPotion : IItem
    {
        public string Name => "파란 포션";

        public void Use(Unit target)
        {
            Console.WriteLine($"\n 🍹{target.Name}이(가) [파란 포션]을 사용했습니다!");
            
            //마나 20회복
            target.Mp += 20;
            if (target.Mp > target.MaxMp) target.Mp = target.MaxMp;

            Console.WriteLine($"    => 마나가 회복되었습니다. (현재 MP : {target.Mp})");
        }   
    }
}