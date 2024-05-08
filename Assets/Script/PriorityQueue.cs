using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PriorityQueue<T>
{
    private class Node
    {
        public T Data {  get; private set; }
        public int Priority { get; set; } = 0;
        public Node(T data, int Priority)
        {
            this.Data = data;
            this.Priority = Priority;
        }
    }
    private List<Node> nodes = new List<Node>();

    public int Count => nodes.Count;
    public void EnQueue(T data,int priority)
    {
        Node newNode = new Node(data, priority);
        if(nodes.Count == 0)
            nodes.Add(newNode);
        else
        {
            int start = 0;
            int end = nodes.Count - 1;
            int harf = 0;
            while(start != end)
            {
                if (end - start == 1)
                {
                    if (nodes[start].Priority < priority) // 들어갈려는 것이 우선순위가 높으면 하프를 엔드로 아니면 스타트로 변경
                        harf = end;
                    else
                        harf = start;
                    break;
                }
                else
                {
                    harf = start + ((end - start) / 2);
                    if (nodes[start].Priority > priority)  // 들어 갈려는 것이 낮으면 중간을 끝으로 높으면 중간을 시작으로 변경
                        end = harf;
                    else
                        start = harf;
                }
            }
            if (nodes[harf].Priority > priority)
                nodes.Insert(harf, newNode);
            else
                nodes.Insert(harf, newNode);
        }
    }
    public T Dequeue()
    {
        Node tail = null;
        if(Count >0)
        {
            tail = nodes[Count - 1];
            nodes.RemoveAt(nodes.Count - 1);
        }
        if (tail == null)
            return tail.Data;
        return default(T);
    }
    public T Peek()
    {
        Node tail = null;
        if (Count > 0)
            tail = nodes[Count - 1];
        if (tail != null)
            return tail.Data;
        return default(T);
    }
}
