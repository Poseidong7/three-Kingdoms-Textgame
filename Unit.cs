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

        //[신규] 레벨 시스템
        public int Level;   //현재 레벨
        public int Exp;     //현재 경험치
        public int MaxExp;  //레벨업에 필요한 경험치
        

        public int Hp; public int MaxHp;     // 병력 (체력)
        public int Mp; public int MaxMp;      // 기력 (마나)
        public int Atk;     // 무력/지력 (공격력)
        public int Def;     // 통솔 (방어력)
        public int Money;   // 군자금(돈)
        public bool IsDead;


        //인벤토리 (군수품)
        //List<IItem> : IItem 계약을 따르는 놈들은 다 담을 수 있는 리스트
        public List<IItem> Inventory = new List<IItem>();
        

        //[2]생성자
        // : 'new Unit(...)'을 할 때 딱 한 번 실행되는 초기화 함수.
        public Unit(string name, JobType job, int hp, int mp, int attack, int defense, int money)
        {
            //외부에서 받은 매개변수를 멤버 변수에 넣음.
            Name = name;
            Job = job;
            MyFaction = Faction.None; //기본은 재야

            //[신규] 레벨 초기화 (1레벨, 경험치 0, 필요경험치 100)
            Level = 1;
            Exp = 0;
            MaxExp = 100;


            MaxHp = hp; Hp = hp;
            MaxMp = mp; Mp = mp;
            Atk = attack;
            Def = defense;
            Money = money;
            IsDead = false;
        }

        // [3] 행동(매서드)
        //[신규][핵심] 경험치 획득 & 레벨업 로직
        public void GainExp(int amount)
        {
            Exp += amount;
            Console.WriteLine($"✨ {Name}은(는) {amount}의 공적(EXP)을 세웠다! ({Exp}/{MaxExp})");

            //경험치통이 꽉 찼으면
            while (Exp >= MaxExp)
            {
                LevelUp();
            }
        } 


        //[신규][핵심] 레벨업 효과 (각 병과별 차별화)
        void LevelUp()
        {
            Exp -= MaxExp; //남은 경험치는 다음 레벨로 이월
            Level++;       //레벨 증가
            MaxExp += 50 + (Level * 10);  //레벨이 오를수록 필요 경험치 대폭 증가

            //증가량 변수
            int incHp = 0, incMp = 0, incAtk = 0, incDef = 0;

            //병과별 성작폭 설정(밸런스 패치 여기서 하시면 됩니다.)
            switch(Job)
            {
                case JobType.Cavalry: //기병 : 공격/체력 균형
                    incHp = 25; incMp = 5; incAtk = 4; incDef = 2;
                    break;
                case JobType.Infantry: //보병 : 체력/방어 특화(탱커)
                    incHp = 40; incMp = 5; incAtk = 2; incDef = 4;
                    break;
                case JobType.Archer: //궁병 : 공격 올인(유리대포)
                    incHp = 15; incMp = 10; incAtk = 6; incDef = 1;
                    break;
                case JobType.Spearman: //창병 : 방어/체력 준수 (딜탱)
                    incHp = 30; incMp = 5; incAtk = 3; incDef = 3;
                    break;
                case JobType.Tactician: //책사 : 기력/공격(지력) 특화
                    incHp = 10; incMp = 30; incAtk = 5; incDef = 1;
                    break;
                default: //무직/기타
                    incHp = 20; incMp = 10; incAtk = 2; incDef = 2;
                    break;
            }

            //실제 스탯 반영
            MaxHp += incHp;
            MaxMp += incMp;
            Atk += incAtk;
            Def += incDef;

            //레벨업 시 회복 서비스
            Hp = MaxHp;
            Mp = MaxMp;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n🎉 [승진!] {Name} 장군이 Lv.{Level}로 승급했습니다! 🎉");
            Console.WriteLine($"   (병력+{incHp}, 기력+{incMp}, 무력+{incAtk}, 통솔+{incDef})\n");
            Console.ResetColor();
            Thread.Sleep(1000);
            
        }

        //[핵심] 상성 데미지 보정 함수(캡슐화)
        // 외부에는 안 보여주고, Attack 함수 안에서만 사용.
        public float GetTypeMultiplier(JobType targetJob)
        {
            if (this.Job == JobType.Tactician || targetJob == JobType.Tactician) return 1.0f; //책사는 무상성
            
            // 가위바위보 로직 (기병 > 보병 > 궁병 > 창병 > 기병)
            if (this.Job == JobType.Cavalry && targetJob == JobType.Infantry) return 1.5f;
            if (this.Job == JobType.Infantry && targetJob == JobType.Archer) return 1.5f;
            if (this.Job == JobType.Archer && targetJob == JobType.Spearman) return 1.5f;
            if (this.Job == JobType.Spearman && targetJob == JobType.Cavalry) return 1.5f;

            // 반대 경우(열세)
            if (this.Job == JobType.Infantry && targetJob == JobType.Cavalry) return 0.8f;
            if (this.Job == JobType.Archer && targetJob == JobType.Infantry) return 0.8f;
            if (this.Job == JobType.Spearman && targetJob == JobType.Archer) return 0.8f;
            if (this.Job == JobType.Cavalry && targetJob == JobType.Spearman) return 0.8f;

            return 1.0f; //그 외는 1배
        }


        // 공격 기능 : 내가(this) 상대방(target)을 공격
        public void Attack(Unit target)
        {
            Random rand = new Random();
            float variance = rand.Next(90, 111) / 100.0f; // 0.9 ~ 1.1 배율 생성

            // 1. 상성 배율 가져오기
            float typeMultiplier = GetTypeMultiplier(target.Job);

            // 2. 최종 공격력 계산 (기본공격력 * 랜덤배율 * 상성배율)
            int finalAtk = (int)(this.Atk * variance * typeMultiplier);

            int damage = finalAtk - target.Def;
            if (damage < 1) damage = 1;

            // 3. 연출(상성에 따라 멘트 다르게)
            string effectMsg = "";
            if (typeMultiplier > 1.0f) effectMsg = "(상대 병과의 약점을 파고 들었다! 🔥)";
            else if (typeMultiplier < 1.0f) effectMsg = "(우리 병과의 약점이 들어나고 있다... 💥)";            

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