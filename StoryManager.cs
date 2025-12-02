using System;
using System.Threading;

namespace TextRPG
{
    public class StoryManager
    {
        //Unit Player를 여기서 관리하거나 Program에서 받아옴
        Unit? player;
        
        //[핵심]게임의 전체 흐름 총괄
        public void StartStory()
        {
            // 1. 오프닝 & 캐릭터 생성
            CreatePlayer();

            // 2. 1장 시작
            Chapter1_YellowTurban();

            // 3. 추후 구현
        }

        // 캐릭터 생성 함수
        void CreatePlayer()
        {
            Console.Clear();
            Console.WriteLine("📜  삼국지 - 천하쟁패 (天下爭覇)  📜");
            Console.WriteLine("난세의 영웅이여, 그대의 이름을 천하에 알리시오.");
            Console.Write("이름 (자) 입력 >> ");
            string name = Console.ReadLine() ?? "무명";

            Console.WriteLine("\n그대의 주특기는 무엇이오?");
            Console.WriteLine("1. 맹장 (猛將) - 무력 중시");
            Console.WriteLine("2. 책사 (策士) - 지력 중시");
            Console.Write("선택 : ");
            string jobInput = Console.ReadLine() ?? "1";

            JobType myJob = JobType.Warlord;
            int hp=100, mp=50, atk=10, def=5;

            if (jobInput == "1")
            {
                myJob = JobType.Warlord;
                hp = 250; mp = 20; atk = 20; def = 10;
            }
            else if (jobInput == "2")
            {
                myJob = JobType.Strategist;
                hp = 100; mp = 100; atk = 30; def = 3;
            }

            // 플레이어 객체 생성
            player = new Unit(name, myJob, hp, mp, atk, def, 500);
            
            // 초기 아이템 지급
            player.GetItem(new HealthPotion());

            Console.WriteLine($"\n🚩 '{player.Name}' 장군, 출진 준비 완료!");
            Thread.Sleep(1000);
        }
        


        // --- 챕터 1 : 황건적의 난 ---
        void Chapter1_YellowTurban()
            {
            // 텍스트 색상 설정 편의를 위한 변수
            //ConsoleColor defaultColor = ConsoleColor.Gray;
            ConsoleColor narratorColor = ConsoleColor.White;
            ConsoleColor playerColor = ConsoleColor.Cyan;
            ConsoleColor liuBeiColor = ConsoleColor.Green;
            ConsoleColor zhangFeiColor = ConsoleColor.Yellow;
            ConsoleColor guanYuColor = ConsoleColor.Red;

            // [이전 상황에서 이어짐]
            Console.ForegroundColor = narratorColor;
            Console.WriteLine("\n[내레이션]");
            Console.WriteLine("그 남자는 당신의 인기척에 황급히 눈물을 훔치며 고개를 듭니다.");
            Thread.Sleep(1500);

            Console.ForegroundColor = liuBeiColor;
            Console.WriteLine("\n[???]");
            Console.WriteLine("\"실례를 범했소. 저는 유비, 자는 현덕이라 하오.\"");
            Thread.Sleep(1000);
            Console.WriteLine("\"황건적의 무리가 천하를 어지럽히는데, 힘은 없고 나이는 먹어가니..\"");
            Thread.Sleep(1000);
            Console.WriteLine("\"그저 한탄만 하고 있었을 뿐입니다.\"");
            Thread.Sleep(1500);

            Console.ForegroundColor = playerColor;
            Console.WriteLine($"\n[{player!.Name}]");
            Console.WriteLine("\"한탄만 한다고 세상이 바뀌겠소? 나에게도 뜻이 있으니 함께 도모해 봅시다.\"");
            Thread.Sleep(1500);

            Console.ForegroundColor = narratorColor;
            Console.WriteLine("\n그때였습니다. 장터 뒤쪽에서 우레와 같은 고함 소리가 들려옵니다.");
            Thread.Sleep(500);
            Console.WriteLine("마치 호랑이 수염을 가진듯한 거한이 성큼성큼 다가옵니다.");
            Thread.Sleep(1500);

            Console.ForegroundColor = zhangFeiColor;
            Console.WriteLine("\n[???]");
            Console.WriteLine("\"이보시오! 대장부가 나라를 위해 칼을 뽑을 생각은 않고, 울고 짜기만 할 셈이오?!\"");
            Thread.Sleep(1000);
            Console.WriteLine($"\"내 성은 장이요 이름은 비, 자는 익덕이라 하오!\"");
            Thread.Sleep(1000);
            Console.WriteLine("\"저기 술집에 가서 이야기나 합시다! 내가 돈은 좀 있소!\"");
            Thread.Sleep(2000);

            Console.Clear(); // 장면 전환 효과
            Console.ForegroundColor = narratorColor;
            Console.WriteLine("============== [장소: 탁현의 주막] ==============");
            Thread.Sleep(1000);
            Console.WriteLine("세 사람은 주막에 앉아 술잔을 기울입니다.");
            Thread.Sleep(1000);
            Console.WriteLine("그때, 붉은 얼굴에 긴 수염을 가진 위풍당당한 사내가 들어와 술을 주문합니다.");
            Thread.Sleep(1500);

            Console.ForegroundColor = guanYuColor;
            Console.WriteLine("\n[???]");
            Console.WriteLine("\"주인장! 여기 술을 빨리 데워 주시오. 마시는 대로 투군하러 갈 것이오.\"");
            Thread.Sleep(1500);

            Console.ForegroundColor = liuBeiColor;
            Console.WriteLine("\n[유비]");
            Console.WriteLine("\"보아하니 호걸이신 듯한데, 우리와 합석하여 대사를 논함이 어떻겠소?\"");
            Thread.Sleep(1500);

            Console.ForegroundColor = guanYuColor;
            Console.WriteLine("\n[관우]");
            Console.WriteLine("\"반갑소. 내 성은 관이요 이름은 우, 자는 운장이라 하오.\"");
            Thread.Sleep(1000);
            Console.WriteLine("\"탐관오리를 죽이고 5년째 쫓기는 신세나, 이제 나라를 위해 싸우고자 하오.\"");
            Thread.Sleep(2000);

            Console.Clear();
            Console.ForegroundColor = narratorColor;
            Console.WriteLine("============== [장소: 장비의 집 뒤편, 복숭아 밭] ==============");
            Thread.Sleep(1000);
            Console.WriteLine("다음 날, 장비의 집 뒤편 복숭아 밭에는 꽃이 만발했습니다.");
            Thread.Sleep(1000);
            Console.WriteLine("검은 소와 흰 말을 제물로 바치고, 세 사람은 향을 피웁니다.");
            Thread.Sleep(2000);

            Console.ForegroundColor = ConsoleColor.Magenta; // 중요한 맹세
            Console.WriteLine("\n[도원결의(桃園結義)]");
            Thread.Sleep(1000);
            Console.WriteLine("\"유비, 관우, 장비, 그리고 당신은 옆에서 그들을 지켜봅니다.\"");
            Thread.Sleep(1000);
            Console.WriteLine("\"비록 성은 다르오나 의형제를 맺어, 힘을 합쳐 곤경에 빠진 백성을 구하려 하오니..\"");
            Thread.Sleep(1500);
            Console.WriteLine("\"한날한시에 태어나지 못했어도, 한날한시에 죽기를 원하나이다!\"");
            Thread.Sleep(1000);
            Console.WriteLine("\"황천(皇天)과 후토(后土)는 굽어살펴 주소서!\"");
            Thread.Sleep(3000);

            Console.ResetColor();
            Console.WriteLine("\n==================================================");
            Console.WriteLine("시스템: 유비, 관우, 장비와 합류 하였습니다.");
            Console.WriteLine("시스템: 의용군 500명이 모였습니다.");
            Console.WriteLine("==================================================");
            Thread.Sleep(2000);

            Console.Clear();
            Console.ForegroundColor = narratorColor;
            Console.WriteLine("며칠 뒤, 유주 태수 유언의 구원 요청이 도착했습니다.");
            Thread.Sleep(1000);
            Console.WriteLine("황건적의 장수 '정원지'가 5만의 군사를 이끌고 쳐들어왔습니다.");
            Thread.Sleep(1000);
            Console.WriteLine("대흥산 아래, 누런 두건을 쓴 도적 떼가 개미떼처럼 깔려 있습니다.");
            Thread.Sleep(2000);

            Console.ForegroundColor = zhangFeiColor;
            Console.WriteLine("\n[장비]");
            Console.WriteLine("\"형님! 저기 쥐새끼 같은 놈들이 보입니다! 내 당장 가서 목을 따오겠소!\"");
            Thread.Sleep(1000);
            
            Console.ForegroundColor = playerColor;
            Console.WriteLine($"\n[{player.Name}]");
            Console.Write("선택지를 입력하세요 (1. 형님 침착하게 가야합니다! / 2. 같이 쓸어버리시죠!): ");
            Console.Write("선택 >>");
            string choice = Console.ReadLine() ?? "1";
            
            if (choice == "2")
            {
                Console.WriteLine("\n>>> 당신은 장비와 함께 적진으로 돌격합니다!");
            }

            else
            {
                Console.WriteLine("\n>>> 신중하게 접근하려 했으나, 적이 먼저 공격해옵니다!");
            }
            
            Thread.Sleep(1000);
            

            // --[전투 발생!] --
            // 1. 적 생성
            Unit enemy = new Unit("황건적 등무", JobType.Warlord, 80, 0, 15, 2, 100);
            
            // 2. Program에 있는 전투 엔진 가동
            bool isWin = Program.StartBattle(player!, enemy);
            
            // 3. 결과에 따른 분기
            if (isWin)
            {
                Console.WriteLine("\n[승리] 황건적 부장, 등무가 목이 날아갔습니다. 당신의 명성이 상승합니다!");
                // 다음 챕터...
            }
            else
            {
                Console.WriteLine("\n[패배] 부상을 입고 퇴각했습니다... (게임 오버)");
                Environment.Exit(0);
            }
        }
    }
}