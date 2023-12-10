namespace AdventOfCode2023;

public class MaxHeap<T> where T : IComparable<T>
{
    private readonly List<T> heap;

    public MaxHeap()
    {
        heap = new List<T>();
    }

    public int Count => heap.Count;

    public void Insert(T item)
    {
        heap.Add(item);
        var i = heap.Count - 1;
        while (i > 0)
        {
            var parent = (i - 1) / 2;
            if (heap[parent].CompareTo(heap[i]) >= 0)
                break;
            Swap(parent, i);
            i = parent;
        }
    }

    public T ExtractMax()
    {
        if (heap.Count == 0)
            throw new InvalidOperationException("Heap is empty.");
        var max = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        Heapify(0);
        return max;
    }

    private void Heapify(int i)
    {
        var left = 2 * i + 1;
        var right = 2 * i + 2;
        var largest = i;
        if (left < heap.Count && heap[left].CompareTo(heap[largest]) > 0)
            largest = left;
        if (right < heap.Count && heap[right].CompareTo(heap[largest]) > 0)
            largest = right;
        if (largest != i)
        {
            Swap(i, largest);
            Heapify(largest);
        }
    }

    private void Swap(int i, int j)
    {
        var temp = heap[i];
        heap[i] = heap[j];
        heap[j] = temp;
    }
}