using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Entity", menuName = "Entity")]
public class EntitySO : ScriptableObject {

    public string typeName;
    public bool isPatrolling;
    public bool isBig;
    public int hp;
    public int damageBase;
    public float damageFrequency;
    public float movmentSpeed;

    //Test
    public int level;

}
