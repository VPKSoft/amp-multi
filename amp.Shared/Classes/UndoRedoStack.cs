#region License
/*
MIT License

Copyright(c) 2022 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

using System.Diagnostics.CodeAnalysis;
using VPKSoft.DropOutStack;

/*
 * Real copyright (c): https://www.cambiaresearch.com/articles/82/generic-undoredo-stack-in-csharp
 * This is a derivative version of the UndoRedoStack by CambiaResearch.com.
 * Copyright © 2002-2022 CambiaResearch.com. All rights reserved.
 * Code taken @ 2022-07-13T09:38:42.032Z.
 * The license is unknown.
 */

namespace amp.Shared.Classes;

/// <summary>
/// A class to store undo and redo data either with specified capacity or "unlimited" capacity.
/// </summary>
/// <typeparam name="T"></typeparam>
public class UndoRedoStack<T>
{
    private Stack<T> undo;
    private Stack<T> redo;

    private DropOutStack<T> undoDropOut;
    private DropOutStack<T> redoDropOut;

    private readonly int capacity = -1;

    /// <summary>
    /// Initializes a new instance of the <see cref="UndoRedoStack{T}"/> class.
    /// </summary>
    public UndoRedoStack()
    {
        Reset();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UndoRedoStack{T}"/> class.
    /// </summary>
    /// <param name="capacity">A maximum size limit for the stack.</param>
    public UndoRedoStack(int capacity)
    {
        this.capacity = capacity;
        Reset();
    }

    /// <summary>
    /// Resets the undo/redo data of this instance.
    /// </summary>
    [MemberNotNull(nameof(undo), nameof(redo), nameof(undoDropOut), nameof(redoDropOut))]
    public void Reset()
    {
        undo = new Stack<T>();
        redo = new Stack<T>();
        if (capacity == -1)
        {
            undoDropOut = new DropOutStack<T>(1);
            redoDropOut = new DropOutStack<T>(1);
        }
        else
        {
            undoDropOut = new DropOutStack<T>(capacity);
            redoDropOut = new DropOutStack<T>(capacity);
        }
    }

    /// <summary>
    /// Adds the specified value to the <see cref="UndoRedoStack{T}"/>.
    /// </summary>
    /// <param name="value">The value to add.</param>
    public void Add(T value)
    {
        if (capacity == -1)
        {
            undo.Push(value);
            redo.Clear(); // Once we issue a new command, the redo stack clears
        }
        else
        {
            undoDropOut.Push(value);
            redoDropOut.Clear(); // Once we issue a new command, the redo stack clears
        }
    }

    /// <summary>
    /// Gets a value indicating whether the <see cref="Undo"/> command can return valid undo data.
    /// </summary>
    /// <value><c>true</c> if the <see cref="Undo"/> command can return valid undo data; otherwise, <c>false</c>.</value>
    public bool CanUndo => capacity == -1 ? undo.Count > 0 : undoDropOut.Count > 0;

    /// <summary>
    /// Gets a value indicating whether the <see cref="Redo"/> command can return valid redo data.
    /// </summary>
    /// <value><c>true</c> if the <see cref="Redo"/> command can return valid undo redo; otherwise, <c>false</c>.</value>
    public bool CanRedo => capacity == -1 ? redo.Count > 0 : redoDropOut.Count > 0;

    /// <summary>
    /// Gets the amount of items which can be undone.
    /// </summary>
    /// <value>The undo count.</value>
    public int UndoCount => capacity == -1 ? undo.Count : undoDropOut.Count;

    /// <summary>
    /// Gets the amount of items which can be redone.
    /// </summary>
    /// <value>The redo count.</value>
    public int RedoCount => capacity == -1 ? redo.Count : redoDropOut.Count;

    /// <summary>
    /// Returns the item to undo.
    /// </summary>
    /// <returns><typeparamref name="T"/>? value.</returns>
    public T? Undo()
    {
        if ((capacity == -1 ? undo.Count : undoDropOut.Count) > 0)
        {
            T? item;
            if (capacity == -1)
            {
                item = undo.Pop();
                redo.Push(item);
            }
            else
            {
                item = undoDropOut.Pop();
                redoDropOut.Push(item);
            }

            return item;
        }

        return default;
    }

    /// <summary>
    /// Returns the item to redo.
    /// </summary>
    /// <returns><typeparamref name="T"/>? value.</returns>
    public T? Redo()
    {
        if ((capacity == -1 ? redo.Count : redoDropOut.Count) > 0)
        {
            T? item;
            if (capacity == -1)
            {
                item = redo.Pop();
                undo.Push(item);
            }
            else
            {
                item = redoDropOut.Pop();
                undoDropOut.Push(item);
            }

            return item;
        }

        return default;
    }
}