using Godot;
using System;

public class CityObjectNode : Spatial
{
    uint model;

    public override void _Ready()
    {
        
    }

    public void SetModel(uint _model){
        model = _model;
    }

    public uint GetModel() => model;

}
