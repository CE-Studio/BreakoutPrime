using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public enum GameState { menu, game, pause, cutscene };
    public GameState state = GameState.game;

    public float playRange = 8;
    public float playHeight = 4;
    public float paddleLevel = -4;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
