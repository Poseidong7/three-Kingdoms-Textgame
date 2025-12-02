using System;
using System.Threading; //시간지연을 사용하기 위한 도구

namespace TextRPG
{
    class Program
    {
        //[전역 변수 선언]
        //함수들이 서로 공유해야 하는 변수는 이곳에 둠.
        // static : 프로그램이 시작될 때 미리 만들어두는 변수.

        static void Main(string[] args)
        {
            // 1. 텍스트 깨짐 방지
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            
            // 2. Program은 준비만 하고 실제 진행은 story~에게 넘김
            StoryManager story = new StoryManager();
            story.StartStory(); 
        }

        // [신규 기능] 키보드 입력 통(버퍼) 청소부
        public static void ClearInputBuffer()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true); // 남은 키 입력 읽은 후 삭제
            }
        }



        

        // [전투 시스템]
        //Story Manager가 요청하면 그때 실행 (승리 : true / 패배 : false 반환)
        public static bool StartBattle(Unit player, Unit enemy)
        {

            ClearInputBuffer();
            Console.Clear();
            Console.WriteLine("⚠️ 적군과 조우했습니다!");
            Thread.Sleep(1000);
            
            //스킬 목록 준비(임시)
            ISkill skill_1 = new Skill_Smash();
            ISkill skill_2 = new Skill_Fireball();

            Console.WriteLine("대화 수정 요망");
            Thread.Sleep(1000);

            while(true)
            {
                Console.Clear();
                Console.WriteLine("================ [ 격전 중! ] ================");
                Console.WriteLine($"[아군] {player.Name} ♥️{player.Hp} | 💧 {player.Mp}");
                Console.WriteLine($"       VS");
                Console.WriteLine($"[적군] {enemy.Name} (❤️ {enemy.Hp})");
                Console.WriteLine("==============================================");

                Console.WriteLine("1. ⚔️ 공격 (돌격)");
                Console.WriteLine("2. ⚡ 전법 (스킬)");
                Console.WriteLine("3. 🎒 군수품 (아이템)");
                Console.WriteLine("4. 🏃 퇴각 (도망)");
                Console.Write("명령 >> ");

                string input = Console.ReadLine() ?? "";
                
                // --- 플레이어 턴 ---
                if (input == "1")
                {
                    player.Attack(enemy);
                }
                else if (input == "2")
                {
                    if (player.Job == "전사") player.UseSkill(skill_1, enemy);
                    else if (player.Job == "마법사") player.UseSkill(skill_2, enemy);
                    else player.Attack(enemy); // 직업 없으면 평타
                }
                
                else if (input == "3")
                {
                    //인벤토리 기능 (여기서 구현 || Unit 함수 호출)
                    if (player.Inventory.Count > 0)
                    {
                        player.UseItem(0); //임시 0번 아이템 사용
                    }
                    else
                    {
                        Console.WriteLine("가진 물건이 없습니다.");
                        Thread.Sleep(500);
                    }
                }
                else if (input == "4")
                {
                    Console.WriteLine("전략적 후퇴를 선택했습니다!");
                    Thread.Sleep(1000);
                    return false; //전투 중단(패배 처리는 아님)
                }
                else
                {
                    Console.WriteLine("잘못된 명령입니다! (턴 낭비)");
                    Thread.Sleep(500);
                }

                // --- 결과 판정 ---
                if (enemy.IsDead)
                {
                    Console.WriteLine("\n🎉 적장의 목을 베었습니다! 적 군대가 와해됩니다! 승리!");
                    player.Money += 100; //전리품
                    Thread.Sleep(2000);
                    return true; //승리 반환
                }
                
                // -- 적군 턴 --
                Console.WriteLine("\n[적의 반격!]");
                enemy.Attack(player);

                if (player.IsDead)
                {
                    Console.WriteLine("\n💀 장군께서 전사했습니다... (게임 오버)");
                    return false; //패배 반환
                }

                Console.WriteLine("\n(계속하려면 엔터...)");
                Console.ReadLine();
            }
        }

    }
}

        // // --- 본진 (로비) ---
        // static void EnterBase()
        // {
        //     Console.WriteLine("때는 서기 184년.. 매관매직이 판치며 한 황실은 점점 패망의 길을 걷게 된다.. 그때 한 사내가 이 난세를 끝내기 위해 출정을 나서는데..")

        //     while (true)
        //     {
        //         Console.Clear();
        //         Console.WriteLine("======== [⛺ 본진] ========");
        //         Console.WriteLine($"현재 위치 : 낙양 근교 ()")
        //         Console.WriteLine("1. ⚔️ 전장으로(황건적 토벌)");
        //         Console.WriteLine("2. 🛌 막사 휴식 (병력 및 기력 회복)");
        //         Console.WriteLine("3. 📊 장수 정보 확인");
        //         Console.WriteLine("4. 🚪 하야 (게임 종료)");
        //         Console.WriteLine("====================");
        //         Console.Write("무엇을 하시겠소? >> ");
                
        //         string input = Console.ReadLine() ?? "";

        //         if (input == "1")
        //         {
        //             EnterDungeon(); //사냥터 입장
        //         }
                
        //         else if (input == "2")
        //         {
        //             Hotel(); //여관 입장
        //         }

        //         else if (input == "3")
        //         {
        //             State(); // 상태창 열기
        //         }

        //         else if (input == "4")
        //         {
        //             Console.WriteLine("안녕히 가세요!");
        //             break;
        //         }
        //     }
        // }

        // //---던전 함수---
        // static void EnterDungeon()
        // {
        //     Console.Clear();
        //     Console.WriteLine("⚠️ 사냥터에 진입했습니다!");
        //     Thread.Sleep(1000);

        //     //1. 몬스터 생성(추후 랜덤 변수로 생성하는 게 좋을듯?)
        //     Unit monster = new Unit("오크", "몬스터", 100, 0, 15, 3, 50);
            
        //     // 2. 스킬 목록 준비(인터페이스 활용)
        //     ISkill skill_1 = new Skill_Smash();
        //     ISkill skill_2 = new Skill_Fireball();

        //     Console.WriteLine($"\n야생의 😈 {monster.Name} (HP:{monster.Hp}이(가) 나타났다!)");
        //     Thread.Sleep(1000);

        //     //3. 전투 루프
        //     while (true)
        //     {
        //         Console.Clear();
        //         Console.WriteLine("================ [ 전투 시작! ] ================");
        //         Console.WriteLine($"[나] {player.Name} ♥️{player.Hp} | 💧 {player.Mp}");
        //         Console.WriteLine($"       VS");
        //         Console.WriteLine($"[적] {monster.Name} (❤️ {monster.Hp})");
        //         Console.WriteLine("==============================================");

        //         //메뉴 출력
        //         Console.WriteLine("1. ⚔️ 일반 공격");
        //         Console.WriteLine("2. ⚡ 스킬 사용");
        //         Console.WriteLine("3. 🎒가방 열기(아이템)");
        //         Console.WriteLine("4. 🏃 도망치기");
        //         Console.Write("행동 선택 >>");
                
        //         string input = Console.ReadLine() ?? "";
                
        //         // --- 플레이어 턴 ---
        //         if (input == "1")
        //         {
        //             player.Attack(monster); //일반 공격
        //         }
        //         else if (input == "2")
        //         {
        //             if (player.Job == "전사")
        //             {
        //                 player.UseSkill(skill_1, monster);
        //                 //전사면 강타 사용
        //             }
        //             else if (player.Job == "마법사")
        //             {
        //                 player.UseSkill(skill_2, monster);
        //                 //마법사면 파이어볼
        //             }
        //         }

        //         else if (input == "3")
        //         {
        //             Open_Inventory();
        //         }




        //         else if (input == "4")
        //         {
        //             Console.WriteLine("도망쳤습니다!");
        //             Thread.Sleep(1000);
        //             break;
        //         }
        //         else
        //         {
        //             Console.WriteLine("잘못된 입력입니다! (턴 낭비!)");
        //             Thread.Sleep(500);
        //         }

        //         // --- 승리 판정 ---
        //         if (monster.IsDead)
        //         {
        //             Console.WriteLine("\n 🎉 승리했습니다!");
        //             Thread.Sleep(2000);
        //             break;
        //         }
        //         // --- 몬스터 턴 ---
        //         Console.WriteLine("\n[적의 턴!]");
        //         monster.Attack(player);

        //         // --- 패배 판정 ---
        //         if (player.IsDead)
        //         {
        //             Console.WriteLine("\n💀 사망했습니다... (게임 오버)");
        //             Environment.Exit(0);
        //         }

        //         Console.WriteLine("\n(계속하려면 엔터...)");
        //         Console.ReadLine();
        //     }

        // }

        // //--- 여관 함수 ---
        // static void Hotel()
        // {
        //     Console.WriteLine("여관에 도착했다.");
        //     Console.WriteLine("여관 주인 : 하루 숙박 20 그로센이오");
        //     Console.WriteLine($"숙박 하시겠습니까? (현재 그로센 : {player.Money} 그로센)");
        //     Console.WriteLine("1. 한다 (-20 그로센) / 2. 안 한다 (돌아가기)");
        //     string input = Console.ReadLine() ?? "";

        //     if (input == "1" && player.Money >= 20)
        //     {
        //         Console.WriteLine("\n 😴 푹 쉬었다. (HP/MP 회복)");
        //         player.Money -= 20;
        //         player.Heal();
        //         Thread.Sleep(1000);
        //     }
        //     else if (input == "1" && player.Money < 20)
        //     {
        //         Console.WriteLine("\n여관 주인 : 이봐! 당신 돈 없잖아! 나가!");
        //         Console.WriteLine("쫓겨났다...");
        //     }
        //     else
        //     {
        //         Console.WriteLine("\n돌아가자.");
        //     }
        // }
            
        // static void State()
        // {
        //         //--- 상태창 함수 ---
        //     Console.WriteLine($"\n [ {player.Name}의 상태 ]");
        //     Console.WriteLine($"❤️  HP : {player.Hp} / {player.MaxHp}");
        //     Console.WriteLine($"💧  MP : {player.Mp} / {player.MaxMp}");
        //     Console.WriteLine($"⚔️  Atx: {player.Atk}");
        //     Console.WriteLine($"🛡️  Def: {player.Def}");
        //     Console.WriteLine($"🪙  Money: {player.Money}");
        //     Console.WriteLine("\n(엔터 키를 누르면 돌아갑니다.)");
        //     Console.ReadLine();
        // }


        // static void Open_Inventory()
        // {
        //     //가방 목록 보여주기
        //     Console.WriteLine("\n=== [ 🎒 가방 ] ===");

        //     //for문으로 리스트 순회
        //     for (int i = 0; i < player.Inventory.Count; i++)
        //     {
        //         //(i+1). 아이템 이름 출력
        //         Console.WriteLine($"{i +1 }. {player.Inventory[i].Name}");
        //     }
        //     Console.WriteLine("0. 취소");
        //     Console.WriteLine("사용할 아이템 번호 : ");
            
        //     int itemNum = int.Parse(Console.ReadLine() ?? "0");
            
        //     if (itemNum > 0 && itemNum <= player.Inventory.Count)
        //     {
        //         //리스트는 0번부터 시작. (입력값 -1) 넘겨줌
        //         player.UseItem(itemNum - 1);
        //     }
        //     else
        //     {
        //         Console.WriteLine("취소했습니다.");
        //     }
        // }
        
