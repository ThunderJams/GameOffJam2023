using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object containing all the tweakable values for gameplay, for ease of access and modification
/// The current game parameter is accessed in the game manager instance
/// </summary>
[CreateAssetMenu]
public class GameParameters : ScriptableObject
{
    public string presetName;

    public float cannonPropellingStrength = 5;
    public float cannonFuseBaseSpeed = 2f;

    public float minimumCatSpawnTimer = 0.5f;

    public float globalCatGravityMultiplier = 1f;
    
    //For the scale reactivity and responsiveness
    public float towerBeamMass = 100;
    public float towerBeamDrag = 50;
    public float dishLinearDrag = 0;

    //Round settings
    public int roundTimer = 30;
    public int startingCatAmount = 4;


    //Time increment scaling
    public float incrementRate = 0.25f;
    //The game is easier if the scratchOMeter is too high
    public float catOMeterVariance = 0.25f;

    //Score values and multipliers
    public float baseCatDroppedScore = 10;

    //Overall cat size
    public int catScaleMultiplier = 1;

}
