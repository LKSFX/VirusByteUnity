using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    [Range(-1, 1)]
    public int directionX = 0;
    [Range(-1, 1)]
    public int directionY = 0;
    public float speed;
    [Tooltip("Força este objeto a mover-se mesmo que não tenha recebido o sinal de liberação.")]
    public bool forceMove = false;
    public bool isMoveAllowed {
        get { return _isMoveAllowed; }
    }
    protected bool _isMoveAllowed;

    /// <summary>
    /// Define se este objeto está se movendo, isso também dependerá da velocidade do objeto,
    /// caso ela seja zero continuará parado. Em geral, os vírus tendem a não se moverem quando fora do campo de visão.
    /// </summary>
    /// <param name="move"></param>
    public void setMove(bool move) {
        _isMoveAllowed = move;
    }

}
