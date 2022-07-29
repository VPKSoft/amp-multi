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

using amp.Database.QueryHelpers;
using Eto.Drawing;
using Eto.Forms;
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
public class FormLoadProgress<T> : Dialog
{
    /// <summary>
    /// Prevents a default instance of the <see cref="FormLoadProgress{T}"/> class from being created.
    /// </summary>
    /// <remarks>Do not use this constructor.</remarks>
    // ReSharper disable once UnusedMember.Local, intentionally prevent the class from being instantiated.
    private FormLoadProgress()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FormLoadProgress{T}"/> class.
    /// </summary>
    /// <param name="queryable">The <see cref="IQueryable{T}"/> query to divide into pieces and run parallel.</param>
    /// <param name="taskSize">Size of the parallel tasks to divide the query into.</param>
    private FormLoadProgress(IQueryable<T> queryable, int taskSize)
    {
        queryDivider = new QueryDivider<T>(queryable, taskSize);
        queryDivider.ProgressChanged += QueryDivider_ProgressChanged;
        progressBar.MaxValue = queryDivider.TotalTaskCount;

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
        queryDivider.QueryCompleted += QueryDivider_QueryCompleted;
    }

    private async void QueryDivider_ProgressChanged(object? sender, QueryProgressChangedEventArgs e)
    {
        await Application.Instance.InvokeAsync(() =>
        {
            lbProgress.Text = string.Format(Shared.Localization.UI.Loading0Percentage,
                e.CurrentPercentage.ToString("F1"));
            progressBar.Value = e.CurrentCount;
        });
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

    /// <summary>
    /// Displays the form <see cref="Window.Topmost"/> with mode set to <c>true</c> and runs the tasks generated from the <paramref name="queryable"/>
    /// </summary>
    /// <param name="owner">The owner of the form.</param>
    /// <param name="queryable">The <see cref="IQueryable{T}"/> instance to divide the query into tasks.</param>
    /// <param name="taskSize">Size of a single task.</param>
    /// <returns>A <see cref="List{T}"/> containing the query results.</returns>
    public static FormLoadProgress<T> RunWithProgress(Control? owner, IQueryable<T> queryable, int taskSize)
    {
        using var form = new FormLoadProgress<T>(queryable, taskSize);
        form.owner = owner;
        form.queryDivider!.RunQueryTasks();
        form.ShowModal(owner);
        return form;
    }

    private async void QueryDivider_QueryCompleted(object? sender, QueryCompletedEventArgs<T> e)
    {
        await Application.Instance.InvokeAsync(() =>
        {
            ResultList = e.ResultList;
            Close();
        });
    }

    /// <summary>
    /// Gets or sets the result of the <see cref="IQueryable{T}"/> query as a list.
    /// </summary>
    /// <value>The result as a list.</value>
    public List<T> ResultList { get; private set; } = new();

    private Control? owner;
    private readonly QueryDivider<T>? queryDivider;
    private readonly Label lbProgress = new();
    private readonly ProgressBar progressBar = new();
}