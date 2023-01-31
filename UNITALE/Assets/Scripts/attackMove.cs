using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackMove : MonoBehaviour
{
    // Defining the variables for different moves
    public string moveName;
    public int moveDamage;
    public bool mentalPhysical;
    [TextArea(1, 10)]
    // Description of the move
    public string[] moveDescription;
}
