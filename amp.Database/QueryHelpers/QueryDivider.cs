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
    public QueryDivider(IQueryable<T> queryable, int taskSize)
    {
        SetQueryTasks(queryable, taskSize);
    }

    /// <summary>
    /// Divides the specified query into <paramref name="taskSize"/>-sized parts and creates the tasks.
    /// </summary>
    /// <param name="query">The query to divide into tasks.</param>
    /// <param name="taskSize">Size of a single task.</param>
    [MemberNotNull(nameof(queryable))]
    public void SetQueryTasks(IQueryable<T> query, int taskSize)
    {
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
        queryTaskThread = new Thread(QueryThreadMethod);
        queryTaskThread.Start();
    }

    private async void QueryThreadMethod()
    {
        var tasks = queryTasks.Select(f => f.Key(f.Value));
        await Task.WhenAll(tasks);
        QueryCompleted?.Invoke(this, new QueryCompletedEventArgs<T> { ResultList = resultList, });
    }

    /// <summary>
    /// Gets the total task count for the divided query.
    /// </summary>
    /// <value>The total task count.</value>
    public int TotalTaskCount => totalCount;

    private Thread? queryTaskThread;
    private readonly List<T> resultList = new();
    private int currentCount;
    private int totalCount;
    private readonly List<KeyValuePair<Func<TaskData, Task<List<T>>>, TaskData>> queryTasks = new();
    private IQueryable<T> queryable;
}