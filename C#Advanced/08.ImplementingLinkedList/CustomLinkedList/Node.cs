﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CustomLinkedList
{
    public class Node
    {
        public int Value { get; set; }
        public Node NextNode { get; set; }
        public Node PreviousNode { get; set; }

        public Node(int value)
        {
            Value = value;
        }
    }
}
