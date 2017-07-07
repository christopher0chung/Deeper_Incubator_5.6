using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deeper_RolloverInt {

    private int _intVal;
    public int intVal
    {
        get { return _intVal; }
        set
        {
            if (value != _intVal)
            {
                if (value < 0)
                {
                    _intVal = _prevalueOffset + _prevalueRange - 1;
                    //Debug.Log(_intVal);
                }
                else
                {
                    _prevalue = (value - _prevalueOffset) % _prevalueRange;
                    _intVal = _prevalue + _prevalueOffset;
                    //Debug.Log(_intVal);
                }
            }
        }
    }

    private int _prevalue;
    private int _prevalueRange;
    private int _prevalueOffset;

    public Deeper_RolloverInt (int StartingVal, int MinValInclusive, int MaxValInclusive)
    {
        _prevalueOffset = MinValInclusive;
        _prevalueRange = MaxValInclusive - MinValInclusive + 1;
        intVal = StartingVal;
    }

    public void ChangeRange (int NewVal, int MinValInclusive, int MaxValInclusive)
    {
        _prevalueOffset = MinValInclusive;
        _prevalueRange = MaxValInclusive - MinValInclusive + 1;
        intVal = NewVal;
    }
}
