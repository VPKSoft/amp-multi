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
using amp.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace amp.Database.QueryHelpers;

/// <summary>
/// A class to divide a query into multiple tasks and run the divided query in parallel.
/// </summary>
public class QueryDivider<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryDivider{T}"/> class.
    /// </summary>
    /// <param name="queryable">The <see cref="IQueryable{T}"/> query to divide into pieces and run parallel.</param>
    /// <param name="taskSize">Size of the parallel tasks to divide the query into.</param>
    /// <param name="exceptionReporter">A class implementing the <see cref="IExceptionReporter"/> interface for reporting exceptions to external handler.</param>
    public QueryDivider(IQueryable<T> queryable, int taskSize, IExceptionReporter exceptionReporter)
    {
        SetQueryTasks(queryable, taskSize, exceptionReporter);
    }

    /// <summary>
    /// Divides the specified query into <paramref name="taskSize"/>-sized parts and creates the tasks.
    /// </summary>
    /// <param name="query">The query to divide into tasks.</param>
    /// <param name="taskSize">Size of a single task.</param>
    /// <param name="reporter">A class implementing the <see cref="IExceptionReporter"/> interface for reporting exceptions to external handler.</param>
    [MemberNotNull(nameof(queryable), nameof(exceptionReporter))]
    public void SetQueryTasks(IQueryable<T> query, int taskSize, IExceptionReporter reporter)
    {
        exceptionReporter = reporter;
        var count = query.Count();
        queryable = query;
        currentCount = 0;
        totalCount = (int)Math.Ceiling((double)count / taskSize);

        for (int i = 0; i * taskSize < count; i++)
        {
            queryTasks.Add(new KeyValuePair<Func<TaskData, Task<List<T>>>, TaskData>(TaskFromTaskData,
                new TaskData { TaskIndex = i + 1, TaskSize = taskSize, ProgressAction = ReportProgress, }));
        }
    }

    /// <summary>
    /// Occurs when the progress of the query tasks completion has changed.
    /// </summary>
    public event EventHandler<QueryProgressChangedEventArgs>? ProgressChanged;

    /// <summary>
    /// Occurs when query has been completed.
    /// </summary>
    public event EventHandler<QueryCompletedEventArgs<T>>? QueryCompleted;

    private void ReportProgress(int taskIndex)
    {
        currentCount++;
        ProgressChanged?.Invoke(this,
            new QueryProgressChangedEventArgs
            { CurrentCount = currentCount, TotalCount = totalCount, TaskIndex = taskIndex, });
    }

    private async Task<List<T>> TaskFromTaskData(TaskData taskData)
    {
        var result = await queryable.Skip(taskData.TaskIndex * taskData.TaskSize).Take(taskData.TaskSize).ToListAsync();
        resultList.AddRange(result);
        taskData.ProgressAction(taskData.TaskIndex);
        return result;
    }

    /// <summary>
    /// Starts running the query tasks in parallel in a separate thread.
    /// </summary>
    public void RunQueryTasks()
    {
        RunQueryTasks(CancellationToken.None);
    }

    /// <summary>
    /// Starts running the query tasks in parallel in a separate thread.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    public void RunQueryTasks(CancellationToken cancellationToken)
    {
        try
        {
            queryTaskThread = new Thread(QueryThreadMethod);
            queryRunning = true;
            queryTaskThread.Start();
        }
        catch (Exception ex)
        {
            exceptionReporter.RaiseExceptionOccurred(ex, GetType().Name, nameof(RunQueryTasks));
            queryRunning = false;
        }
    }

    private async void QueryThreadMethod()
    {
        try
        {
            var tasks = queryTasks.Select(f => f.Key(f.Value));
            await Task.WhenAll(tasks);
            QueryCompleted?.Invoke(this, new QueryCompletedEventArgs<T> { ResultList = resultList, });
        }
        catch (Exception ex)
        {
            exceptionReporter.RaiseExceptionOccurred(ex, GetType().Name, nameof(QueryThreadMethod));
        }

        queryRunning = false;
    }

    /// <summary>
    /// Gets the total task count for the divided query.
    /// </summary>
    /// <value>The total task count.</value>
    public int TotalTaskCount => totalCount;

    /// <summary>
    /// Gets a value indicating whether a query is currently running.
    /// </summary>
    /// <value><c>true</c> if a query is currently running; otherwise, <c>false</c>.</value>
    public bool QueryRunning => queryRunning;

    /// <summary>
    /// Waits for current query to be finished.
    /// </summary>
    public void WaitForCurrentQuery()
    {
        while (QueryRunning)
        {
            Thread.Sleep(10);
        }
    }

    private volatile bool queryRunning;
    private Thread? queryTaskThread;
    private readonly List<T> resultList = new();
    private int currentCount;
    private int totalCount;
    private readonly List<KeyValuePair<Func<TaskData, Task<List<T>>>, TaskData>> queryTasks = new();
    private IQueryable<T> queryable;
    private IExceptionReporter exceptionReporter;
}