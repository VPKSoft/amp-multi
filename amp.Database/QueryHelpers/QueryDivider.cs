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
using Microsoft.EntityFrameworkCore;
using VPKSoft.Utils.Common.Interfaces;

namespace amp.Database.QueryHelpers;

/// <summary>
/// A class to divide a query into multiple tasks and run the divided query in either parallel or linear.
/// </summary>
public class QueryDivider<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QueryDivider{T}"/> class.
    /// </summary>
    /// <param name="queryable">The <see cref="IQueryable{T}"/> query to divide into pieces and run either parallel or linear.</param>
    /// <param name="taskSize">Size of the parallel tasks to divide the query into.</param>
    /// <param name="exceptionReporter">A class implementing the <see cref="IExceptionReporter"/> interface for reporting exceptions to external handler.</param>
    /// <param name="parallelQueryRunning">A value indicating whether to run the query in parallel or in linear mode.</param>
    public QueryDivider(IQueryable<T> queryable, int taskSize, IExceptionReporter exceptionReporter, bool parallelQueryRunning)
    {
        this.taskSize = taskSize;
        parallel = parallelQueryRunning;

        if (!parallelQueryRunning)
        {
            this.queryable = queryable;
            this.exceptionReporter = exceptionReporter;
        }
        else
        {
            SetQueryTasks(queryable, exceptionReporter);
        }
    }

    /// <summary>
    /// Divides the specified query into <see cref="taskSize"/>-sized parts and creates the tasks.
    /// </summary>
    /// <param name="query">The query to divide into tasks.</param>
    /// <param name="reporter">A class implementing the <see cref="IExceptionReporter"/> interface for reporting exceptions to external handler.</param>
    [MemberNotNull(nameof(queryable), nameof(exceptionReporter))]
    private void SetQueryTasks(IQueryable<T> query, IExceptionReporter reporter)
    {
        exceptionReporter = reporter;
        var count = query.Count();
        queryable = query;
        currentCount = 0;
        totalCount = (int)Math.Ceiling((double)count / taskSize);

        for (int i = 0; i * taskSize < count; i++)
        {
            queryTasks.Add(new KeyValuePair<Func<TaskData, Task<List<T>>>, TaskData>(TaskFromTaskData,
                new TaskData { TaskIndex = i, TaskSize = taskSize, ProgressAction = ReportProgress, }));
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
        currentCount = currentCount + 1;
        ProgressChanged?.Invoke(this,
            new QueryProgressChangedEventArgs
            { CurrentCount = currentCount, TotalCount = totalCount, TaskIndex = taskIndex, });
    }

    private async Task<List<T>> TaskFromTaskData(TaskData taskData)
    {
        var result = await queryable.Skip(taskData.TaskIndex * taskData.TaskSize).Take(taskData.TaskSize).ToListAsync(token);
        resultList.AddRange(result);
        taskData.ProgressAction(taskData.TaskIndex);
        return result;
    }

    /// <summary>
    /// Starts running the query tasks in parallel in a separate thread.
    /// </summary>
    public void RunQueryTasksParallel()
    {
        RunQueryTasksParallel(CancellationToken.None);
    }

    /// <summary>
    /// Starts running the query tasks in parallel in a separate thread.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    public void RunQueryTasksParallel(CancellationToken cancellationToken)
    {
        if (!parallel)
        {
            throw new InvalidOperationException(
                "Set the parallelQueryRunning flag to true in the constructor for parallel query.");
        }

        starCalled = true;

        try
        {
            token = cancellationToken;
            Task.Factory.StartNew(QueryThreadMethodParallel, cancellationToken);
        }
        catch (Exception ex)
        {
            exceptionReporter.RaiseExceptionOccurred(ex, GetType().Name, nameof(RunQueryTasksParallel));
            queryRunning = false;
        }
    }

    /// <summary>
    /// Starts running the query in linear mode in a separate thread.
    /// </summary>
    public void RunQueryTasksLinear()
    {
        RunQueryTasksLinear(CancellationToken.None);
    }

    /// <summary>
    /// Starts running the query in linear mode in a separate thread.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    public void RunQueryTasksLinear(CancellationToken cancellationToken)
    {
        if (parallel)
        {
            throw new InvalidOperationException(
                "Set the parallelQueryRunning flag to false in the constructor for linear query.");
        }

        starCalled = true;

        try
        {
            token = cancellationToken;
            Task.Factory.StartNew(QueryThreadMethodLinear, cancellationToken);
        }
        catch (Exception ex)
        {
            exceptionReporter.RaiseExceptionOccurred(ex, GetType().Name, nameof(RunQueryTasksLinear));
            queryRunning = false;
        }
    }

    private async void QueryThreadMethodParallel()
    {
        if (queryRunning)
        {
            throw new InvalidOperationException(
                "Query is in progress. Wait it for to finish before starting a new one.");
        }

        queryRunning = true;

        try
        {
            var tasks = queryTasks.Select(f => f.Key(f.Value)).ToList();
            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            exceptionReporter.RaiseExceptionOccurred(ex, GetType().Name, nameof(QueryThreadMethodParallel));
        }

        queryRunning = false;
        starCalled = false;
        QueryCompleted?.Invoke(this, new QueryCompletedEventArgs<T> { ResultList = resultList, });
    }

    private void QueryThreadMethodLinear()
    {
        if (queryRunning)
        {
            throw new InvalidOperationException(
                "Query is in progress. Wait it for to finish before starting a new one.");
        }

        queryRunning = true;

        try
        {
            var count = queryable.Count();
            currentCount = 0;
            totalCount = (int)Math.Ceiling((double)count / taskSize);

            ReportProgress(0);

            for (int i = 0; i * taskSize < count; i++)
            {
                var result = queryable.Skip(i * taskSize).Take(taskSize).ToList();
                resultList.AddRange(result);
                if (token.IsCancellationRequested)
                {
                    break;
                }
                ReportProgress(i);
            }
        }
        catch (Exception ex)
        {
            exceptionReporter.RaiseExceptionOccurred(ex, GetType().Name, nameof(QueryThreadMethodParallel));
        }

        queryRunning = false;
        starCalled = false;
        QueryCompleted?.Invoke(this, new QueryCompletedEventArgs<T> { ResultList = resultList, });
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

    /// <summary>
    /// Waits for the current query to start.
    /// </summary>
    public void WaitForStart()
    {
        if (starCalled)
        {
            while (!queryRunning && !starCalled)
            {
                Thread.Sleep(10);
            }
        }
    }

    private volatile bool starCalled;
    private volatile bool queryRunning;
    private volatile List<T> resultList = new();
    private volatile int currentCount;
    private volatile int totalCount;
    private readonly List<KeyValuePair<Func<TaskData, Task<List<T>>>, TaskData>> queryTasks = new();
    private volatile IQueryable<T> queryable;
    private IExceptionReporter exceptionReporter;
    private volatile int taskSize;
    private CancellationToken token;
    private volatile bool parallel;
}