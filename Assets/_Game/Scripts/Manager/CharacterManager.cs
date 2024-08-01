using HuySpace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    [SerializeField] private Transform characterHolder;
    [SerializeField] private Player playerPrefab;

    private List<Enemy> botList = new List<Enemy>();

    [field :SerializeField] public Player player { get; private set; }
    [field: SerializeField] public int CharacterAlive { get; private set; } = 0;
    public int BotInBattle => botList.Count;

    public event Action OnBotDieAction;

    // Initial
    public void OnInit()
    {
        player.OnInit();
        CollectBot();
    }

    // Spawn Player
    public void SpawnPlayer(Vector3 pos)
    {
        player = Instantiate(playerPrefab, pos, Quaternion.identity, characterHolder);
        player.OnInit();
        CameraManager.Ins.InGameCam.SetTarget(player);
    }

    // Spawn Bot
    public Enemy SpawnBot(Vector3 pos)
    {
        Enemy bot = SimplePool.Spawn<Enemy>(PoolType.Bot, pos, Quaternion.identity);
        bot.OnInit();
        botList.Add(bot);

        return bot;
    }

    public void SpawnBotWithQty(int qty)
    {
        Vector3 randomPos = Vector3.zero;

        for (int i = 0; i < qty; i++)
        {
            randomPos = LevelManager.Ins.GetRandomPos();

            while (Vector3.Distance(randomPos, player.characterTransform.position) <= player.attackRange + 1f)
            {
                randomPos = LevelManager.Ins.GetRandomPos();
            }

            SpawnBot(randomPos);
        }
    }

    public void DeathBot(Enemy bot)
    {
        botList.Remove(bot);
        CharacterAlive--;

        OnBotDieAction?.Invoke();
    }

    public void CollectBot()
    {
        foreach (Enemy bot in botList)
        {
            SimplePool.Despawn(bot);
        }
    }

    public Enemy GetRandomBot()
    {
        return botList[UnityEngine.Random.Range(0, botList.Count)];
    }

    public void SetAmountOfCharacterAlive(int qty)
    {
        CharacterAlive = qty;
    }
}
