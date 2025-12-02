using System; //console 사용 위해 필요

namespace TextRPG
{
    // ==== 스킬 모음집 ====


    // --- [전사 스킬] ---
    public class Skill_Smash : ISkill
    // 1. 강타 / 150% 
    {
        public string Name => "강타"; //이름 설정
        public int MpCost => 10; //마나 설정

        //실제 스킬 행동 구현
        public void Cast(Unit caster, Unit target)
        {
            Console.WriteLine($"\n💥 {caster.Name}의 [강타] 발동!");
            
            // 데미지 공식 : 공격력 * 1.5 - 방어력
            int damage = (int)(caster.Atk * 1.5f) - target.Def;
            if (damage < 1) damage = 1;

            target.TakeDamage(damage); //타겟 때리기
        }
    }

    // --- [마법사 스킬] ---
    public class Skill_Fireball : ISkill
    {
        // 1. 파이어볼 / 3.0 + 방무
        public string Name => "파이어볼";
        public int MpCost => 25;

        public void Cast(Unit caster, Unit target)
        {
            Console.WriteLine($"\n☄️ {caster.Name}의 [파이어볼] 발동!");
            
            //데미지 공식 : 공격력 * 3.0(+ 방무)
            int damage = (int)(caster.Atk * 3.0f);
            
            target.TakeDamage(damage);
        }
    }


}