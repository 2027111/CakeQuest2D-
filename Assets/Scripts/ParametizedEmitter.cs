using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class ParameterizedEmitter<T> : SignalEmitter
{
    public T parameter;
}

public class SignalEmitterWithBool : ParameterizedEmitter<bool> { }

//Put this in its own file
public class SignalEmitterWithInt : ParameterizedEmitter<int> { }

public class SignalEmitterWithString : ParameterizedEmitter<string> { }

