using System;
using System.Collections.Generic;
using System.IO; //파일 읽기용

namespace TextRPG
{
    public class GachaManager
    {
        // 뽑기 가능한 장수 목록 (메모리에 저장)
        private static List<string[]> warlordDatabase = new List<string[]>();
        
        // 1. 게임 시작 시 파일 읽어오기 (초기화)
        public static void LoadData()
        {
            //파일이 없으면 에러 방지
            if (!File.Exists("Warlords.csv"))
            {
                Console.WriteLine("❌ [오류] Warlords.csv 데이터 파일이 없습니다!");
                return;
            }

            //파일의 모든 줄을 읽어옴
            string[] lines = File.ReadAllLines("Warlords.csv");

            foreach (string line in lines)
            {
                //콤마로 쪼개서 저장 [이름, 직업, 등급, 설명]
                string[] data = line.Split(',');
                warlordDatabase.Add(data);
            }
            
            Console.WriteLine($"📚 장수 데이터 {warlordDatabase.Count}명 로드 완료.");
        }


        // 2. 뽑기 (비용을 지불하고 랜덤 장수 획득)
        public static Unit? Roll(Unit player)
        {
            int cost = 100; //뽑기 비용
            
            if (player.Money < cost)
            {
                Console.WriteLine($"🚫 돈이 부족합니다! (필요: {cost} / 보유: {player.Money})");
                return null; //돈 없으면 꽝
            }
            
            player.Money -= cost;


            //랜덤 추첨
            Random rand = new Random();
            int index = rand.Next(warlordDatabase.Count);
            string[] pick = warlordDatabase[index];

            //데이터 파싱 (문자열 -> Enum 변환)
            string name = pick[0];
            JobType job = (JobType)Enum.Parse(typeof(JobType), pick[1]);
            Rank rank = (Rank)Enum.Parse(typeof(Rank), pick[2]); //[추가] 등급 파싱
            string desc = pick[3];

            //결과 연출
            Console.Clear();
            Console.WriteLine("🛖 주막에 들어갑니다... 두구두구...");
            Thread.Sleep(1000);
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n✨ [{rank}] {job} '{name}' 등용 성공!");
            Console.ResetColor();
            Console.WriteLine($"   💬 \"{desc}\"");
            Thread.Sleep(1000);

            // [핵심] 뽑은 데이터로 실제 Unit 객체 생성! (스탯은 임의 설정) < 추후 개선
            // 나중에 csv에 스탯도 넣으면 좋음
            int hp = 100, atk = 10, def = 5;
            if (rank == Rank.SSR) {hp = 150; atk = 30; }
            else if (rank == Rank.SR) {hp = 150; atk = 20; }

            Unit newColleague = new Unit(name, job, rank, hp, 50, atk, def, 0);
            
            return newColleague; // 뽑은 장수를 배달.

            
        }
    }
}