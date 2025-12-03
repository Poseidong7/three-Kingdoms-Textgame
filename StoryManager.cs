using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace TextRPG
{
    public class StoryManager
    {
        //Unit Player를 여기서 관리하거나 Program에서 받아옴
        Unit? player;

        //[신규] 동료 명단 (리스트)
        public List<Unit> colleagues = new List<Unit>();

        //스마트 Sleep 함수
        //설정된 배율에 따라 대기 시간을 자동으로 조절.
        void Sleep(int milliseconds)
        {
            int finalTime = (int)(milliseconds * GameSettings.TextSpeedMultiplier);
            Thread.Sleep(finalTime);
        }

        //게임 시작 전 속도 설정 메뉴
        void SetupGame()
        {
            Console.Clear();
            Console.WriteLine("⚙️ 게임 설정을 진행합니다.");
            Console.WriteLine("\n[텍스트 속도 설정]");
            Console.WriteLine("1. 느림 (여유롭게)");
            Console.WriteLine("2. 보통 (추천)");
            Console.WriteLine("3. 빠름 (한국인)");
            Console.Write("선택 >> ");

            string input = Console.ReadLine() ?? "2";

            if (input == "1") GameSettings.SetTextSpeed(GameSettings.SpeedOption.Slow);
            else if (input == "3") GameSettings.SetTextSpeed(GameSettings.SpeedOption.Fast);
            else GameSettings.SetTextSpeed(GameSettings.SpeedOption.Normal);

            Console.WriteLine("\n설정이 완료되었습니다. 게임을 시작합니다...");
            Thread.Sleep(1000); //여기는 고정 시간 (설정 적용 전)
        }

        //[핵심]게임의 전체 흐름 총괄
        public void StartStory()
        {
            // 0. 게임 설정
            SetupGame();

            // [추가] 이어하기 확인
            Console.WriteLine("1. 새로 시작  2. 이어하기");
            string choice = Console.ReadLine() ?? "1";
            
            // 이어하기 선택시
            if (choice == "2")
            {
                //데이터 매니저에게 저장된 데이터 요청
                Unit? loadedPlayer = DataManager.Load();

                if (loadedPlayer != null)
                {
                    //불러오기 성공
                    player = loadedPlayer; //주인공 교체
                    Console.WriteLine($"\n반갑습니다, {player.Name} 장군! 여정을 계속합니다.");
                    Thread.Sleep(1000);

                    //[추후 수정] 오프닝 건너뛰고 바로 마을로 이동
                    EnterBase();
                    return;
                }
                else
                {
                    //파일이 없으면 실패 메세지 띄우고 새로시작
                    Console.WriteLine("\n❌ 저장된 파일이 없습니다. 새로 시작합니다.");
                    Thread.Sleep(1000);
                }
            }

            // 1. 오프닝 & 캐릭터 생성
            CreatePlayer();

            // 2. 1장 시작
            Opening_TaverBrawl();

            EnterBase();

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

            Console.WriteLine("\n그대의 병과를 선택하시오:");
            Console.WriteLine("1. 기병 🐎 (이동속도 빠름, 보병에 강함)");
            Console.WriteLine("2. 보병 🛡️ (방어력 높음, 궁병에 강함)");
            Console.WriteLine("3. 궁병 🏹 (원거리, 창병에 강함)");
            Console.WriteLine("4. 창병 🔱 (공격력 높음, 기병에 강함)");
            Console.WriteLine("5. 책사 📜 (계략 중심)");
            Console.Write("선택 : ");
            string jobInput = Console.ReadLine() ?? "1";

            JobType myJob = JobType.Infantry; //기본 보병
            int hp=100, mp=50, atk=10, def=5;

            // 밸런스 조절 (예시)
            if (jobInput == "1") { myJob = JobType.Cavalry; hp=180; atk=25; def=5; }
            else if (jobInput == "2") { myJob = JobType.Infantry; hp=250; atk=15; def=15; }
            else if (jobInput == "3") { myJob = JobType.Archer; hp=120; atk=30; def=2; }
            else if (jobInput == "4") { myJob = JobType.Spearman; hp=150; atk=28; def=8; }
            else if (jobInput == "5") { myJob = JobType.Tactician; hp=100; mp=100; atk=10; def=2; }

            // 플레이어 객체 생성
            player = new Unit(name, myJob, Rank.N, hp, mp, atk, def, 500);
            
            // 초기 아이템 지급
            player.GetItem(new HealthPotion());

            Console.WriteLine($"\n🚩 '{player.Name}' 장군, 출진 준비 완료!");
            Thread.Sleep(1000);
        }

        

        // --- [오프닝 : 폭풍 전야의 술잔] ---
        void Opening_TaverBrawl()
            {
            ConsoleColor narrator = ConsoleColor.Gray;
            ConsoleColor enemyColor = ConsoleColor.DarkYellow;
            ConsoleColor allyColor = ConsoleColor.Green;
            ConsoleColor playerColor = ConsoleColor.Cyan;

            Console.Clear();
            Console.ForegroundColor = narrator;
            Console.WriteLine("서기 184년 초봄..");
            Sleep(1000);
            Console.WriteLine("탁군(涿郡) 외곽의 허름한 객잔.");
            Sleep(1000);
            Console.WriteLine("국경 지대에는 흉흉한 소문만이 안개처럼 떠돌고 있습니다.");
            Sleep(1500);

            Console.WriteLine($"\n[{player!.Name}]"); // 실제 플레이어 이름 사용
            Console.ForegroundColor = playerColor;
            Console.WriteLine("(탁한 술잔을 기울이며...)");
            Console.WriteLine("\"세상이 곧 뒤집어질 것 같군.. 피 냄새가 바람에 실려와.\"");
            Sleep(2000);

            // [사건 발생]
            Console.ForegroundColor = enemyColor;
            Console.WriteLine("\n[쾅!!]");
            Sleep(300);
            Console.WriteLine("\n[황건적 조장]");
            Console.WriteLine("\"어이 주인장! 있는 술 다 내와! 돈은 '누런 하늘(黃天)'께서 내주실 거다!\"");
            Sleep(1500);

            Console.ForegroundColor = narrator;
            Console.WriteLine("\n머리에 누런 두건을 쓴 사내들이 주막을 점거합니다.");
            Console.WriteLine("주막 주인 노인이 덜덜 떨며 그들 앞을 막아섭니다.");
            Sleep(1500);

            Console.ForegroundColor = enemyColor;
            Console.WriteLine("\n[황건적 조장]");
            Console.WriteLine("\"이 늙은이가 죽고 싶어 환장했나!\"");
            Console.WriteLine("놈이 시퍼런 칼을 뽑아 노인을 겨눕니다.");
            Sleep(1500);

            // [선택의 순간]
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n==================================================");
            Console.WriteLine("[!운명의 선택!] 당신의 행동을 결정하십시오.");
            Console.WriteLine("1. [무력] 당장 칼을 뽑아 놈들을 베어버린다.");
            Console.WriteLine("2. [회유] 술값을 대신 내주며 말로 해결하려 한다.");
            Console.WriteLine("==================================================");
            Console.Write("선택 >> ");
            
            string choice = Console.ReadLine() ?? "1";

            // [전개]
            Console.ForegroundColor = playerColor;
            Console.WriteLine($"\n[{player.Name}]");
            if (choice == "2") Console.WriteLine("\"이보시오, 술값은 내가 낼 테니 그 칼 거두시오.\"");
            else Console.WriteLine("\"그 더러운 칼 치우지 못해?!\"");
            Sleep(1000);

            Console.ForegroundColor = enemyColor;
            Console.WriteLine("\n[황건적 조장]");
            Console.WriteLine("\"뭐야? 웬 놈이냐! 네놈도 저 늙은이와 함께 저승으로 보내주마!\"");
            Sleep(1000);

            // [첫 번째 동료 등장]
            Console.ForegroundColor = allyColor;
            Console.WriteLine("\n[???]");
            Console.WriteLine("\"여럿이서 하나를 덤비다니, 부끄러운 줄도 모르는 놈들이군!\"");
            Sleep(1000);
            Console.WriteLine("구석에서 삿갓을 쓴 건장한 사내가 일어나 당신의 등 뒤를 지킵니다.");
            Sleep(1500);

            Console.WriteLine("\n[방랑 무인]");
            Console.WriteLine("\"형씨, 등 뒤는 내가 맡겠소. 한번 놀아봅시다!\"");
            Sleep(2000);

            //[실제 전투 연결]
            //튜토리얼용 적 생성
            Unit tutorialEnemy = new Unit("황건적 조장", JobType.Bandit, Rank.N, 30, 0, 5, 0, 50);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n>>> 전투가 시작됩니다!");
            Console.ResetColor();
            Sleep(1000);

            //Program의 전투 엔진 호출
            bool isWin = Program.StartBattle(player, tutorialEnemy);

            if(isWin)
            {
                Console.ForegroundColor = allyColor;
                Console.WriteLine("\n[방랑 무인]");
                Console.WriteLine("\"후우.. 솜씨가 제법이군. 내 이름은 '단복'이라 하오.\"");
                Sleep(1000);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n[시스템] 첫 번째 동료 [단복(협객)]과 인연을 맺었습니다.");

                //단복 생성 후 리스트에 추가
                Unit danbok = new Unit("단복", JobType.Tactician, Rank.R, 80, 20, 15, 5, 0);
                colleagues.Add(danbok); //영입
                Sleep(2000);
            }

            else
            {
                //튜토리얼에서 져..? 그래도.. 봐준다.
                Console.WriteLine("\n[방랑 무인] \"쳇, 오늘은 운이 없군. 일단 피합시다!\"");
            }

            Console.ResetColor();
        }


        //[마을 로직] > 추후 개선 예정

        // --- [2] 본진 (마을) 시스템 ---
        void EnterBase()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("======== [⛺ 본진] ========");
                Console.WriteLine($"현재 위치 : 낙양 근교");
                Console.WriteLine("1. ⚔️ 전장으로 (반복 사냥)");
                Console.WriteLine("2. 🛌 막사 휴식 (병력 및 기력 회복)");
                Console.WriteLine("3. 📊 장수 정보 확인");
                Console.WriteLine("4. 🎒 가방 열기");
                Console.WriteLine("5. 🍺 주막 (장수 등용 - 100금)"); 
                Console.WriteLine("6. 👥 동료 관리"); // [신규]
                Console.WriteLine("7. 💾 저장하기"); 
                Console.WriteLine("8. 🚪 다음 스토리 진행"); // 번호 밀림
                Console.WriteLine("====================");
                Console.Write("무엇을 하시겠소? >> ");
                
                string input = Console.ReadLine() ?? "";

                if (input == "1")
                {
                    // [수정] 사냥터 입장 -> 랜덤 적 생성 후 전투
                    Console.WriteLine("주변의 잔당을 소탕하러 갑니다...");
                    Thread.Sleep(1000);
                    
                    // 랜덤 적 생성 (연습용 황건적)
                    Unit dummyEnemy = new Unit("황건적 잔당", JobType.Bandit, Rank.N, 50, 0, 10, 1, 30);
                    
                    bool win = Program.StartBattle(player!, dummyEnemy);
                    if (win) Console.WriteLine("승리하여 복귀했습니다.");
                    else { Console.WriteLine("부상을 입고 복귀했습니다."); player!.Hp = 1; } // 죽지 않게 처리
                }
                else if (input == "2")
                {
                    Hotel(); // 여관(막사) 입장
                }
                else if (input == "3")
                {
                    State(); // 상태창 열기
                }
                else if (input == "4")
                {
                    Open_Inventory(); // 가방 열기
                }

                else if (input == "5")
                {
                    Unit? newUnit = GachaManager.Roll(player!); //가챠 실행 결과 받기
                    
                    // 뽑았으면 리스트에 넣기
                    if (newUnit != null)
                    {
                        colleagues.Add(newUnit);
                        //나중에 중복체크 로직 추가 예정
                    }
                }

                else if (input == "6")
                {
                    ManageColleagues(); //동료 관리 함수 호출
                }
                else if (input == "7")
                {
                    DataManager.Save(player!); //[추가] 저장 연결 -> 추후 수정 요망
                }

                else if (input == "8")
                {
                    Console.WriteLine("군비를 갖추고 다음 전장으로 떠납니다!");
                    Thread.Sleep(1000);
                    break; // 마을 루프 탈출 -> 다음 챕터로 이동
                }
            }
        }

        // --- 휴식 기능 ---
        void Hotel()
        {
            Console.WriteLine("\n[군수관]");
            Console.WriteLine("\"장군, 병력과 기력을 회복하시겠습니까? (비용: 20냥)\"");
            Console.WriteLine($"보유 군자금: {player!.Money}냥");
            Console.Write("1. 휴식한다  2. 돌아간다 >> ");
            
            string input = Console.ReadLine() ?? "";

            if (input == "1")
            {
                if (player.Money >= 20)
                {
                    player.Money -= 20;
                    player.Heal();
                    Console.WriteLine("\n💤 막사에서 편안하게 휴식을 취했습니다.");
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.WriteLine("\n\"장군, 군자금이 부족합니다.\"");
                    Thread.Sleep(1000);
                }
            }
            else
            {
                Console.WriteLine("\n돌아갑니다.");
            }
        }
            
        // --- 상태창 기능 ---
        void State()
        {
            Console.Clear();
            Console.WriteLine($"\n [ {player!.Name}의 상태 ]");
            Console.WriteLine($"소속 : {Faction.None} | 병과 : {player.Job}");
            Console.WriteLine($"❤️  병력 : {player.Hp} / {player.MaxHp}");
            Console.WriteLine($"💧  기력 : {player.Mp} / {player.MaxMp}");
            Console.WriteLine($"⚔️  무력 : {player.Atk}");
            Console.WriteLine($"🛡️  통솔 : {player.Def}");
            Console.WriteLine($"💰  군자금 : {player.Money}");
            Console.WriteLine("\n(엔터 키를 누르면 돌아갑니다.)");
            Console.ReadLine();
        }

        // --- 인벤토리 기능 ---
        void Open_Inventory()
        {
            Console.WriteLine("\n=== [ 🎒 군수품 목록 ] ===");

            if (player!.Inventory.Count == 0)
            {
                Console.WriteLine("(비어있음)");
            }
            else
            {
                for (int i = 0; i < player.Inventory.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {player.Inventory[i].Name}");
                }
            }
            
            Console.WriteLine("0. 취소");
            Console.Write("사용할 아이템 번호 >> ");
            
            // 숫자가 아니면 0으로 처리해서 에러 방지
            int.TryParse(Console.ReadLine(), out int itemNum);
            
            if (itemNum > 0 && itemNum <= player.Inventory.Count)
            {
                player.UseItem(itemNum - 1);
            }
            else
            {
                Console.WriteLine("취소했습니다.");
            }
            Thread.Sleep(500);
        }
        
        //[신규] 동료 목록 보여주기
        void ManageColleagues()
        {
            Console.Clear();
            Console.WriteLine("=== [ 👥 동료 목록 ] ===");

            if (colleagues.Count == 0)
            {
                Console.WriteLine("(아직 동료가 없습니다.)");
            }
            else
            {
                for (int i = 0; i < colleagues.Count; i++)
                {
                    Unit u = colleagues[i];
                    Console.WriteLine($"{i+1}. [{u.MyRank}] {u.Name} ({u.Job}) - HP:{u.Hp}");                }
            }
            
            Console.WriteLine("\n(엔터 키를 누르면 돌아갑니다)");
            Console.ReadLine();
        }
    }
}