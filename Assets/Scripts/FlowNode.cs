using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FlowNode<T> : IEnumerable<FlowNode<T>>
{
    public T Value { get; set; }

    public FlowNode<T>[] Destinations { get; set; }

    public FlowNode<T> this[int index]
    {
        get { return Destinations[index]; }
        set { Destinations[index] = value; }
    }

    public IEnumerator<FlowNode<T>> GetEnumerator()
    {
        return ((IEnumerable<FlowNode<T>>)Destinations).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Destinations.GetEnumerator();
    }

    public FlowNode() { }

    public FlowNode(T value)
    {
        this.Value = value;
    }
}
