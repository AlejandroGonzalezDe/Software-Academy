﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    public class AlexLinkedList<T> : ILinkedList<T>
    {


        public AlexLinkedList()
        {
            this.Count = 0;
        }

        public AlexLinkedList(AlexNode<T>? initialNode)
        {
            this.Head = initialNode;

            this.Count++;
        }

        public int Count { get; private set; }

        public INode<T>? Head { get; private set; }

        public INode<T>? Tail
        {
            get
            {
                INode<T>? node = Head;
                while (node?.Next() != null)
                {
                    node = node.Next();
                }
                return node;
            }

        }

        public IEnumerable<INode<T>> Nodes
        {
            get
            {
                INode<T> node = Head;
                yield return Head;
                while (node?.Next() != null)
                {
                    node = node.Next();
                    yield return node;
                }
            }
        }

        public void AddFirst(INode<T> value)
        {
            if (this.Head != null)
            { value.LinkNext(this.Head); }

            this.Head = value;
            Count++;
        }

        public void AddLast(INode<T> value)
        {
            this.Tail?.LinkNext(value);
            Count++;
        }

        public void Clear()
        {
            Count = 0;
            Head = null;
        }

        public INode<T>[] FindAll(T value)
        {
            INode<T>[] result = new INode<T>[0];
            int resultCount = 0;
            foreach (INode<T> node in this.Nodes)
            {
                if (EqualityComparer<T>.Default.Equals(node.Content, value))
                {
                    Array.Resize(ref result, resultCount + 1);
                    result[resultCount++] = node;
                }
            }
            return result;
        }

        public INode<T>? FindFirst(T value)
        {
            INode<T>[] result = FindAll(value);//shame...shame...
            if (result.Length > 0) { return result[0]; }
            else { return null; }

        }

        public void InsertAfterNodeIndex(INode<T> value, int position)
        {
            if (position < 0 || position >= this.Count) throw new InvalidOperationException();
            if (position == Count - 1) { this.AddLast(value); return; }
            int loopCounter = 0;

            INode<T>? theNodeToAppend = null;
            foreach (INode<T> node in this.Nodes)
            {
                if (loopCounter == position)
                {
                    theNodeToAppend = node;
                    break;
                }
                loopCounter++;
            }
            value.LinkNext(theNodeToAppend.Next());
            theNodeToAppend.LinkNext(value);
            Count++;
        }

        public void RemoveAt(int IndexPosition)
        {
            if (Head == null
                || Count == 0
                || IndexPosition > Count - 1
                || IndexPosition < 0) throw new InvalidOperationException();

            INode<T>? prevNode = Head;
            if (IndexPosition == 0) { this.RemoveFirst(); return; };
            if (IndexPosition == Count - 1) { this.RemoveLast(); return; };
            for (int i = 1; i < IndexPosition; i++)
            {
                prevNode = prevNode?.Next();
            }
            prevNode?.LinkNext(prevNode?.Next()?.Next());
            Count--;


        }

        public void RemoveFirst()
        {
            if (Head == null || Count == 0) throw new InvalidOperationException();

            this.Head = Head.Next();
            Count--;

        }

        public void RemoveLast()
        {
            if (Head == null || Count == 0) throw new InvalidOperationException();
            var node = Head;
            while (node.Next() != Tail)
            {
                node = node.Next();
                continue;
            }
            node.LinkNext(null);
            Count--;

        }
    }
}