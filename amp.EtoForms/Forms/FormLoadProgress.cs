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

using amp.EtoForms.Forms.AdditionalClasses;
using Eto.Drawing;
using Eto.Forms;
using Microsoft.EntityFrameworkCore;
using Control = Eto.Forms.Control;
using Label = Eto.Forms.Label;
using ProgressBar = Eto.Forms.ProgressBar;

namespace amp.EtoForms.Forms;

/// <summary>
/// A dialog to divide query into parts, run the query and display the progress.
/// Implements the <see cref="Dialog" />
/// </summary>
/// <typeparam name="T">The type of the query result.</typeparam>
/// <seealso cref="Dialog" />
public class FormLoadProgress<T> : Form
{
    /// <summary>
    /// Prevents a default instance of the <see cref="FormLoadProgress{T}"/> class from being created.
    /// </summary>
    /// <remarks>Do not use this constructor.</remarks>
    // ReSharper disable once UnusedMember.Local, intentionally prevent the class from being instantiated.
    private FormLoadProgress()
    {
        // Dummy empty queryable.
        queryable = new List<T>().Select(f => f).AsQueryable();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FormLoadProgress{T}"/> class.
    /// </summary>
    /// <param name="queryable">The <see cref="IQueryable{T}"/> query to divide into pieces and run parallel.</param>
    /// <param name="taskSize">Size of the parallel tasks to divide the query into.</param>
    private FormLoadProgress(IQueryable<T> queryable, int taskSize)
    {
        MinimumSize = new Size(300, 30);
        var count = queryable.Count();
        this.queryable = queryable;

        for (int i = 0; i * taskSize < count; i++)
        {
            queryTasks.Add(new KeyValuePair<Func<TaskData, Task<List<T>>>, TaskData>(Key,
                new TaskData { TaskIndex = i + 1, TaskSize = taskSize, ProgressAction = AddToProgress, }));
        }

        progressBar.MaxValue = (int)Math.Ceiling(count / (double)taskSize);
        Content = new StackLayout
        {
            Items =
            {
                new StackLayoutItem(lbProgress),
                new StackLayoutItem(progressBar, HorizontalAlignment.Stretch),
            },
            Padding = Globals.DefaultPadding,
            Spacing = Globals.DefaultSpacing.Height,
        };
        WindowStyle = WindowStyle.None;
        SizeChanged += SizeOrPositionChanged;
        LocationChanged += SizeOrPositionChanged;
    }

    private void SizeOrPositionChanged(object? sender, EventArgs e)
    {
        if (owner != null)
        {
            var left = owner.Location.X + (owner.Width - Width) / 2;
            var top = owner.Location.Y + (owner.Height - Height) / 2;

            Location = new Point(left, top);
        }
    }

    private void AddToProgress(int taskIndex)
    {
        Application.Instance.Invoke(() =>
        {
            progressBar.Value++;
            var valueSpan = progressBar.MaxValue - progressBar.MinValue;
            var percentage = valueSpan == 0 ? 0.0 : progressBar.Value * 100 / (double)valueSpan;

            lbProgress.Text = string.Format(Shared.Localization.UI.Loading0Percentage, percentage.ToString("F1"));
            Application.Instance.RunIteration();
        });
    }

    private readonly List<T> resultList = new();

    private async Task<List<T>> Key(TaskData arg)
    {
        var result = await queryable.Skip(arg.TaskIndex * arg.TaskSize).Take(arg.TaskSize).ToListAsync();
        resultList.AddRange(result);
        arg.ProgressAction(arg.TaskIndex);
        return result;
    }

    /// <summary>
    /// Displays the form <see cref="Window.Topmost"/> with mode set to <c>true</c> and runs the tasks generated from the <paramref name="queryable"/>
    /// </summary>
    /// <param name="owner">The owner of the form.</param>
    /// <param name="queryable">The <see cref="IQueryable{T}"/> instance to divide the query into tasks.</param>
    /// <param name="taskSize">Size of a single task.</param>
    /// <returns>A <see cref="List{T}"/> containing the query results.</returns>
    public static async Task<List<T>> RunWithProgress(Control? owner, IQueryable<T> queryable, int taskSize)
    {
        using var form = new FormLoadProgress<T>(queryable, taskSize);
        form.owner = owner;
        form.Topmost = true;
        form.Show();
        await form.RunTasks();
        return form.resultList;
    }

    private Control? owner;
    private readonly List<KeyValuePair<Func<TaskData, Task<List<T>>>, TaskData>> queryTasks = new();
    private readonly IQueryable<T> queryable;

    private async Task RunTasks()
    {
        var tasks = queryTasks.Select(f => f.Key(f.Value));
        await Task.WhenAll(tasks);
        Close();
    }

    private readonly Label lbProgress = new();
    private readonly ProgressBar progressBar = new();
}