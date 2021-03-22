using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public bool IsStageClear => _enemy.Status.HP.Value <= 0;
    public bool IsGameOver => _player.Status.HP.Value <= 0;

    private PlayerBehaviour _player;
    private Enemy _enemy;
    private CanvasController _canvas;


    protected override void Awake()
    {
        base.Awake();
        _player = FindObjectOfType<PlayerBehaviour>();
        _enemy = FindObjectOfType<Enemy>();
        _canvas = FindObjectOfType<CanvasController>();
    }

    void Update()
    {
        if (IsStageClear) _canvas.ActivateStageClearPanel();
        else if (IsGameOver) _canvas.ActivateGameOverPanel();
    }

}
