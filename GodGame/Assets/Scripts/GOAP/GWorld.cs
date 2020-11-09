using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();//singleton
    private static WorldStates world;

    static GWorld()
    {
        world = new WorldStates();
    }

    private GWorld()
    {

    }

    public static GWorld Instance
    {
        get { return instance; }//fails here***
    }

    public WorldStates GetWorld()
    {
        return world;
    }
}
