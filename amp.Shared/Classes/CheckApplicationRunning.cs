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

namespace amp.Shared.Classes;

/// <summary>
/// Application utilities, for example to check if an application is already running.
/// </summary>
public static class CheckApplicationRunning
{
    /// <summary>A static list to hold the created mutexes.</summary>
    private static readonly List<Mutex> mutexes = new();

    /// <summary>
    /// Gets or sets the action to report an exception.
    /// </summary>
    /// <value>The exception action.</value>
    public static Action<Exception>? ExceptionAction { get; set; }

    /// <summary>
    /// Checks if an application with a given unique string is already running.
    /// </summary>
    /// <param name="uniqueId">An (assumed) unique ID to use for the check.</param>
    /// <returns>True if an application with a given unique string is already running, otherwise false.</returns>
    public static bool CheckIfRunning(string uniqueId)
    {
        try
        {
            Mutex.OpenExisting(uniqueId);
            return true;
        }
        catch
        {
            var mutex = new Mutex(true, uniqueId);
            CheckApplicationRunning.mutexes.Add(mutex);
            return false;
        }
    }

    /// <summary>
    /// Disposes a mutex reserved by an application if one exists in the internal collection.
    /// </summary>
    /// <param name="uniqueId">An (assumed) unique ID for the mutex to dispose of.</param>
    public static void DisposeMutexByName(string uniqueId)
    {
        try
        {
            using var mutex = Mutex.OpenExisting(uniqueId);
            var index = CheckApplicationRunning.mutexes.IndexOf(mutex);
            if (index == -1)
            {
                return;
            }

            CheckApplicationRunning.mutexes.RemoveAt(index);
        }
        catch (Exception ex)
        {
            ExceptionAction?.Invoke(ex);
        }
    }

    /// <summary>
    /// Checks if an application with a given unique string is already running but doesn't create a new <see cref="System.Threading.Mutex" /> with the given <paramref name="uniqueId" /> name.
    /// </summary>
    /// <param name="uniqueId">An (assumed) unique ID to use for the check.</param>
    /// <returns>True if an application with a given unique string is already running, otherwise false.</returns>
    public static bool CheckIfRunningNoAdd(string uniqueId)
    {
        try
        {
            Mutex.OpenExisting(uniqueId);
            return true;
        }
        catch
        {
            return false;
        }
    }
}