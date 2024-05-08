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
                    if (nodes[start].Priority < priority) // ������ ���� �켱������ ������ ������ ����� �ƴϸ� ��ŸƮ�� ����
                        harf = end;
                    else
                        harf = start;
                    break;
                }
                else
                {
                    harf = start + ((end - start) / 2);
                    if (nodes[start].Priority > priority)  // ��� ������ ���� ������ �߰��� ������ ������ �߰��� �������� ����
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
