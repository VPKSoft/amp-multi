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

using amp.Database.DataModel;
using amp.Playback.Classes;
using amp.Tests.FakingIt;

namespace amp.Tests;

[TestClass]
public class QuietHours
{
    private readonly bool[] niceNeighborsArray = {
        true,   // 00
        true,   // 01
        true,   // 02
        true,   // 03
        true,   // 04
        true,   // 05
        true,   // 06
        true,   // 07
        true,   // 08
        false,  // 09
        false,  // 10
        false,  // 11
        false,  // 12
        false,  // 13
        false,  // 14
        false,  // 15
        false,  // 16
        false,  // 17
        false,  // 18
        false,  // 19
        false,  // 20
        false,  // 21
        false,  // 22
        true,  // 23
    };

    private readonly bool[] daySleeper = {
        false,   // 00
        false,   // 01
        false,   // 02
        false,   // 03
        false,   // 04
        false,   // 05
        false,   // 06
        true,   // 07
        true,   // 08
        true,  // 09
        true,  // 10
        true,  // 11
        true,  // 12
        true,  // 13
        true,  // 14
        true,  // 15
        true,  // 16
        false,  // 17
        false,  // 18
        false,  // 19
        false,  // 20
        false,  // 21
        false,  // 22
        false,  // 23
    };

    [TestMethod]
    public void TestQuietSameDay1()
    {
        var fakeSettings = new FakeQuietHourSettings
        {
            QuietHoursFrom = "23:00",
            QuietHoursTo = "09:00",
            QuietHours = true,
        };

        var quietHourHandler = new QuietHourHandler<AudioTrack, AlbumTrack, Album>(fakeSettings);

        var dateCurrent = new DateTime(2022, 8, 11, 0, 0, 0);

        for (var i = 0; i < 24; i++)
        {
            Assert.AreEqual(niceNeighborsArray[i], quietHourHandler.IsQuietHourTime(dateCurrent));
            if (i == 9) // Clock 09:59
            {
                Assert.AreEqual(true, quietHourHandler.IsQuietHourTime(dateCurrent.AddMinutes(-1)));
            }
            if (i == 23) // Clock 22:59
            {
                Assert.AreEqual(false, quietHourHandler.IsQuietHourTime(dateCurrent.AddMinutes(-1)));
            }
            dateCurrent = dateCurrent.AddHours(1);
        }

        // Clock 00:00, Midnight
    }

    [TestMethod]
    public void TestQuietSameDay2()
    {
        var fakeSettings = new FakeQuietHourSettings
        {
            QuietHoursFrom = "07:00",
            QuietHoursTo = "17:00",
            QuietHours = true,
        };

        var quietHourHandler = new QuietHourHandler<AudioTrack, AlbumTrack, Album>(fakeSettings);

        var dateCurrent = new DateTime(2022, 8, 11, 0, 0, 0);

        for (var i = 0; i < 24; i++)
        {
            Assert.AreEqual(daySleeper[i], quietHourHandler.IsQuietHourTime(dateCurrent));
            if (i == 7) // Clock 09:01
            {
                Assert.AreEqual(false, quietHourHandler.IsQuietHourTime(dateCurrent.AddMinutes(-1)));
            }
            if (i == 17) // Clock 23:01
            {
                Assert.AreEqual(true, quietHourHandler.IsQuietHourTime(dateCurrent.AddMinutes(-1)));
            }
            dateCurrent = dateCurrent.AddHours(1);
        }
    }
}