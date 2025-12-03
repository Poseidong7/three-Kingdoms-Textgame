using System;
using System.IO; //파일 관리
using System.Text.Json; //JSON 변환기 (직렬화)
using System.Collections.Generic;

namespace TextRPG
{
    // 1. 저장용 데이터 설계도 (Save File의 형태)
    public class PlayerData
    {
        public string Name {get; set; } = "";
        public JobType Job {get; set; }
        public int Level {get; set; }
        public int Exp { get; set; }
        public Rank rank {get; set; }
        public int Hp { get; set; }
        public int MaxHp { get; set; }
        public int Mp { get; set; }
        public int MaxMp { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Money { get; set; }

        //[핵심] 아이템은 객체 자체가 아니라 '이름(문자열)'만 저장!
        public List<string> InventoryNames {get; set; } = new List<string>();
    }

    public static class DataManager
    {
        static string fileName = "save.json"; //저장될 파일 이름

        //[저장 기능] Unit -> PlayerData -> 파일
        public static void Save(Unit player)
        {
            // 1. Unit 객체를 저장용 데이터로 변환
            PlayerData data = new PlayerData();
            data.Name = player.Name;
            data.Job = player.Job;
            data.rank = player.MyRank;
            data.Level = player.Level;
            data.Exp = player.Exp;
            data.Hp = player.Hp;
            data.MaxHp = player.MaxHp;
            data.Mp = player.Mp;
            data.MaxMp = player.MaxMp;
            data.Atk = player.Atk;
            data.Def = player.Def;
            data.Money = player.Money;

            //인벤토리의 아이템 이름만 따서 저장 목록에 넣기
            foreach (IItem item in player.Inventory)
            {
                data.InventoryNames.Add(item.Name);
            }

            // 2. JSON 텍스트로 변환 (직렬화)
            string jsonString = JsonSerializer.Serialize(data);

            // 3. 파일로 쓰기
            File.WriteAllText(fileName, jsonString);

            Console.WriteLine("💾 게임이 저장되었습니다! (save.json)");
            Thread.Sleep(1000);
        }


        // [불러오기 기능] 파일 -> PlayerData -> Unit
        public static Unit? Load()
        {
            // 1. 파일이 없으면 실패
            if (!File.Exists(fileName))
            {
                return null;
            }

            // 2. 파일 읽기
            string jsonString = File.ReadAllText(fileName);

            // 3. 텍스트 -> 데이터 변환 (역직렬화)
            PlayerData? data = JsonSerializer.Deserialize<PlayerData>(jsonString);

            if (data == null) return null;

            // 4. 저장된 데이터로 실제 Unit 생성
            Unit loadedPlayer = new Unit(data.Name, data.Job, data.rank, data.MaxHp, data.MaxMp, data.Atk, data.Def, data.Money);

            // 레벨과 경험치, 현재 체력 복구
            loadedPlayer.Level = data.Level;
            loadedPlayer.Exp = data.Exp;
            loadedPlayer.Hp = data.Hp;
            loadedPlayer.Mp = data.Mp;

            // 5. 아이템 복구 (이름을 보고 다시 새 지급)
            foreach (string itemName in data.InventoryNames)
            {
                if (itemName == "빨간 포션") loadedPlayer.Inventory.Add(new HealthPotion());
                else if (itemName == "파란 포션") loadedPlayer.Inventory.Add(new ManaPotion());
                //아이템 많아지면 ItemManager 따로 신설
            }

            return loadedPlayer;
        }
    }
}