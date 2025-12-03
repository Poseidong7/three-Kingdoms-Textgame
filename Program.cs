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
                    Console.WriteLine("아직 미구현");
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
                    player.GainExp(50); //레벨업 테스트
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
