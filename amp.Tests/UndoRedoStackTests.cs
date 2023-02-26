#region License
/*
MIT License

Copyright(c) 2023 Petteri Kautonen

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

using amp.Shared.Classes;

namespace amp.Tests;

[TestClass]
public class UndoRedoStackTests
{
    [TestMethod]
    public void TestBasics()
    {
        var undoRedoStack = new UndoRedoStack<int>();

        for (var i = 0; i < 10; i++)
        {
            undoRedoStack.Add(i + 1);
        }

        Assert.AreEqual(10, undoRedoStack.UndoCount);

        Assert.AreEqual(10, undoRedoStack.Undo());

        Assert.AreEqual(10, undoRedoStack.Redo());

        while (undoRedoStack.CanUndo)
        {
            undoRedoStack.Undo();
        }

        Assert.AreEqual(1, undoRedoStack.Redo());

        undoRedoStack.Redo();
        undoRedoStack.Redo();

        Assert.AreEqual(4, undoRedoStack.Redo());
    }

    [TestMethod]
    public void TestCapacity()
    {
        var undoRedoStack = new UndoRedoStack<int>(5);

        for (var i = 0; i < 10; i++)
        {
            undoRedoStack.Add(i + 1);
        }

        for (var i = 10; i >= 6; i--)
        {
            Assert.AreEqual(i, undoRedoStack.Undo());
        }

        Assert.AreEqual(false, undoRedoStack.CanUndo);

        Assert.AreEqual(default(long), undoRedoStack.Undo());
    }

    [TestMethod]
    public void TestUnlimitedCanUndo()
    {
        var undoRedoStack = new UndoRedoStack<int>();

        for (var i = 0; i < 10; i++)
        {
            undoRedoStack.Add(i + 1);
        }

        for (var i = 0; i < 10; i++)
        {
            Assert.AreEqual(true, undoRedoStack.CanUndo);
            undoRedoStack.Undo();
        }

        Assert.AreEqual(false, undoRedoStack.CanUndo);
    }
}