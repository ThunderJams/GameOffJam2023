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
    public float dishMass = 1;

    //Round settings
    public int roundTimer = 30;
    public int maxRoundTimer = 40;
    public int startingCatAmount = 2;
    public int maximumCatAmount = 9;
    public int inBetweenRoundTime = 4;
    [Range(0.1f, 1)] public float catOMeterDecreaseValue = 0.8f;

    //Time increment scaling
    public float incrementRate = 0.75f;
    public float desiredRoundMaxDifficulty = 15;
    public float finalTimeMultiplier = 0.5f;

    //The game is easier if the scratchOMeter is too high
    public float catOMeterVariance = 0.25f;

    //Score values and multipliers
    public float baseCatDroppedScoreMultiplier = 1;

    //Overall cat size
    public float catScaleMultiplier = 1;
    public float catMassAddition= 1;

    public float baseCatCooldown = 5f;

}
