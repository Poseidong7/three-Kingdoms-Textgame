using System;
using System.Threading;
using System.Collections.Generic; //List를 쓰기 위한 도구

namespace TextRPG
{
    public class Unit //public을 붙여야 다른 파일에서도 보임.
                      // Unit 클래스는 플레이어와 몬스터를 모두 만드는 공통 설계도
    {
        //[1] 멤버 변수 (필드)
        public string Name;     // 장수 이름 (자, 호)
        public JobType Job;     // 병과 (맹장 / 책사)
        public Faction MyFaction;   // 소속 세력(위/촉/오/재야)

        public int Hp;      // 병력 (체력)
        public int MaxHp;
        public int Mp;      // 기력 (마나)
        public int MaxMp;
        public int Atk;     // 무력/지력 (공격력)
        public int Def;     // 통솔 (방어력)
        public int Gold;   // 군자금(돈)
        public bool IsDead;


        //인벤토리 (군수품)
        //List<IItem> : IItem 계약을 따르는 놈들은 다 담을 수 있는 리스트
        public List<IItem> Inventory = new List<IItem>();
        

        //[2]생성자
        // : 'new Unit(...)'을 할 때 딱 한 번 실행되는 초기화 함수.
        public Unit(string name, JobType job, int hp, int mp, int attack, int defense, int gold)
        {
            //외부에서 받은 매개변수를 멤버 변수에 넣음.
            Name = name;
            Job = job;
            MyFaction = Faction.None; // 처음엔 무소속 재야 장수로
            MaxHp = hp;
            Hp = hp;
            MaxMp = mp;
            Mp = mp;
            Atk = attack;
            Def = defense;
            Gold = gold;
            IsDead = false;
        }

        // [3] 행동(매서드)
        // 공격 기능 : 내가(this) 상대방(target)을 공격
        public void Attack(Unit target)
        {
            Random rand = new Random();
            
            // 1. 랜덤 데미지 계산(공격력의 90 ~ 110%)
            float variance = rand.Next(90, 111) / 100.0f; // 0.9 ~ 1.1 배율 생성
            int finalAtk = (int)(this.Atk * variance); // 최종뎀 계산

            // 2. 데미지 공식 : (최종뎀) - (상대 방어력)
            int damage = finalAtk - target.Def;

            // 3. 최소 데미지 보정(방어력이 아무리 높아도 최소 1은 달게 함.)
            if (damage < 1) damage = 1;

            // 4. 공격 메시지 출력 및 연출
            Console.WriteLine($"\n⚔️ {Name}의 공격! 상대의 병력에 타격을 줍니다!");
            Thread.Sleep(500); //0.5초 딜레이 (타격감)

            // 5. 상대방에게 데미지 입게 명령.
            target.TakeDamage(damage);
        }

        // 피격 기능 : 내가 데미지 입는 행동
        public void TakeDamage(int damage) 
        {   
            // 체력 감소
            Hp -= damage;

            //상태 메세지 출력
            Console.WriteLine($"💥{Name}의 부대, {damage}의 피해를 입었다! (병력 : {Hp} / {MaxHp})");

            // 사망 체크 : 체력이 0 이하로 떨어졌다면
            if (Hp <= 0)
            {
                Hp = 0; //음수가 될 수 없으니 0으로 고정
                IsDead = true; // 사망 플래그 true
                Console.WriteLine($"💀 {Name}은(는) 장렬히 전사했습니다...");
            }
        }
        
        // 회복 기능 : 마을이나 여관 등 사용 가능
        public void Heal()
        {
            Hp = MaxHp;
            Mp = MaxMp;
            Console.WriteLine($"{Name}의 부대가 재정비를 마쳤습니다!");
        }
        
        //스킬 사용
        public void UseSkill(ISkill skill, Unit target)
        {
            // 1. 기력 부족 체크
            if (this.Mp < skill.MpCost)
            {
                Console.WriteLine($"🚫기력이 부족합니다! (필요 : {skill.MpCost} / 현재 : {this.Mp})");
                return; //공격 취소하고 함수 종료
            }

            // 2. 기력 소모
            this.Mp -= skill.MpCost;
            
            // 3. 스킬 발동
            //Unit은 무슨 스킬인지, 데미지 얼마인지 계산 안함
            skill.Cast(this, target);
        }

        //[신규] 아이템 획득 기능
        public void GetItem(IItem item)
        {
            Inventory.Add(item); //리스트에 추가 add
            Console.WriteLine($"📦 {Name}은(는) [{item.Name}]을(를) 손에 넣었습니다!");
        }

        // [신규] 아이템 사용 기능 (몇 번째 아이템을 쓸 지)
        public void UseItem(int index)
        {
            //가방 범위를 벗어났는지 체크
            if (index < 0 || index >= Inventory.Count)
            {
                Console.WriteLine("그런 물건은 없습니다.");
                return;
            }
            //아이템 꺼내기
            IItem item = Inventory[index];

            // 사용
            item.Use(this); //내가 나한테 씀

            // 사용했으니 가방에서 삭제
            Inventory.RemoveAt(index);
        }
    }
}