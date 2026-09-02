using System;
using System.Collections.Generic;

public class MinHeap<T>
{
    private struct HeapNode
    {
        public T Item;
        public int Priority;
    }

    private List<HeapNode> heap = new List<HeapNode>();

    public int Count => heap.Count;

    public void Enqueue(T item, int priority)
    {
        HeapNode node = new HeapNode { Item = item, Priority = priority  };
        heap.Add(node);
        int index = heap.Count - 1;

        while (index > 0 && heap[index].Priority < heap[(index - 1) / 2].Priority)
        {
            int parentIndex = (index - 1) / 2;
            HeapNode tmp = heap[index];
            heap[index] = heap[parentIndex];
            heap[parentIndex] = tmp;

            index = parentIndex;
        }
    }

    public T Dequeue()
    {

        if (heap.Count == 0)
        {
            throw new InvalidOperationException("힙이 비어있음");
        }

        T result = heap[0].Item;

        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);

        int index = 0;
        int count = heap.Count;

        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;
            int smallest = index;

            if (leftChild < count && heap[leftChild].Priority < heap[smallest].Priority)
            {
                smallest = leftChild;
            }

            if (rightChild < count && heap[rightChild].Priority < heap[smallest].Priority)
            {
                smallest = rightChild;
            }

            if (smallest == index)
            {
                break;
            }

            HeapNode tmp = heap[index];
            heap[index] = heap[smallest];
            heap[smallest] = tmp;

            index = smallest;
        }
        return result;
    }
}
