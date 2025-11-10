using System;
using System.Collections;
using System.Collections.Generic;


public class BinaryTree<T> : IEnumerable<T>
    where T : class, IComparable<T> 
{
    private class TreeNode
    {
        public T Value { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }

        public TreeNode(T value)
        {
            Value = value;
        }
    }

    private TreeNode _root;

    public void Add(T value)
    {
        if (value == null) return;

        if (_root == null)
        {
            _root = new TreeNode(value);
            return;
        }

        AddRecursive(_root, value);
    }

    private void AddRecursive(TreeNode node, T value)
    {
        
        int comparison = value.CompareTo(node.Value);

        if (comparison < 0)
        {
            // Йдемо ліворуч
            if (node.Left == null)
            {
                node.Left = new TreeNode(value);
            }
            else
            {
                AddRecursive(node.Left, value);
            }
        }
        else if (comparison > 0)
        {

            if (node.Right == null)
            {
                node.Right = new TreeNode(value);
            }
            else
            {
                AddRecursive(node.Right, value);
            }
        }
    }



    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in PostorderTraversal(_root))
        {
            yield return item;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private IEnumerable<T> PostorderTraversal(TreeNode node)
    {
        if (node != null)
        {
            foreach (var item in PostorderTraversal(node.Left))
            {
                yield return item;
            }

            foreach (var item in PostorderTraversal(node.Right))
            {
                yield return item;
            }

            yield return node.Value;
        }
    }
}