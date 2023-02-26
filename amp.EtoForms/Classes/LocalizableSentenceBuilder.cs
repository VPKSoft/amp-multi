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

using CommandLine;
using CommandLine.Text;

namespace amp.EtoForms.Classes;

/// <summary>
/// Class LocalizableSentenceBuilder.
/// Implements the <see cref="SentenceBuilder" />
/// </summary>
/// <seealso cref="SentenceBuilder" />
public class LocalizableSentenceBuilder : SentenceBuilder
{
    /// <inheritdoc />
    public override Func<string> RequiredWord { get; } =
        () => Shared.Localization.Messages.RequiredOption0IsMissing;

    /// <inheritdoc />
    public override Func<string> OptionGroupWord { get; } = () => Shared.Localization.Messages.Group;

    /// <inheritdoc />
    public override Func<string> ErrorsHeadingText { get; } = () => Shared.Localization.Messages.ERRORS;

    /// <inheritdoc />
    public override Func<string> UsageHeadingText { get; } = () => Shared.Localization.Messages.USAGE;

    /// <inheritdoc />
    public override Func<bool, string> HelpCommandText { get; } = option => option
        ? Shared.Localization.Messages.DisplayThisHelpScreen
        : Shared.Localization.Messages.DisplayMoreInformationOnASpecificCommand;

    /// <inheritdoc />
    public override Func<bool, string> VersionCommandText { get; } =
        (_) => Shared.Localization.Messages.DisplayVersionInformation;

    /// <inheritdoc />
    public override Func<Error, string> FormatError
    {
        get
        {
            return error =>
            {
                switch (error.Tag)
                {
                    case ErrorType.BadFormatTokenError:
                        return string.Format(Shared.Localization.Messages.Token0IsNotRecognized,
                            ((BadFormatTokenError)error).Token);
                    case ErrorType.MissingValueOptionError:
                        return string.Format(Shared.Localization.Messages.Option0HasNoValue,
                            ((MissingValueOptionError)error).NameInfo.NameText);
                    case ErrorType.UnknownOptionError:
                        return string.Format(Shared.Localization.Messages.RequiredOption0IsMissing,
                            ((UnknownOptionError)error).Token);
                    case ErrorType.MissingRequiredOptionError:
                        var errorMissing = ((MissingRequiredOptionError)error);
                        return errorMissing.NameInfo.Equals(NameInfo.EmptyName)
                            ? Shared.Localization.Messages.RequiredOption0IsMissing
                            : string.Format(Shared.Localization.Messages.RequiredOption0IsMissing,
                                errorMissing.NameInfo.NameText);
                    case ErrorType.BadFormatConversionError:
                        var badFormat = ((BadFormatConversionError)error);
                        return badFormat.NameInfo.Equals(NameInfo.EmptyName)
                            ? Shared.Localization.Messages.AValueNotBoundToOptionNameIsDefinedWithABadFormat
                            : string.Format(Shared.Localization.Messages.Option0IsDefinedWithABadFormat,
                                badFormat.NameInfo.NameText);
                    case ErrorType.SequenceOutOfRangeError:
                        var seqOutRange = ((SequenceOutOfRangeError)error);
                        return seqOutRange.NameInfo.Equals(NameInfo.EmptyName)
                            ? Shared.Localization.Messages
                                .ASequenceValueNotBoundToOptionNameIsDefinedWithFewItemsThanRequired
                            : string.Format(
                                Shared.Localization.Messages
                                    .ASequenceOption0IsDefinedWithFewerOrMoreItemsThanRequired,
                                seqOutRange.NameInfo.NameText);
                    case ErrorType.BadVerbSelectedError:
                        return string.Format(Shared.Localization.Messages.Verb0IsNotRecognized,
                            ((BadVerbSelectedError)error).Token);
                    case ErrorType.NoVerbSelectedError:
                        return Shared.Localization.Messages.NoVerbSelected;
                    case ErrorType.RepeatedOptionError:
                        return string.Format(Shared.Localization.Messages.Option0IsDefinedMultipleTimes,
                            ((RepeatedOptionError)error).NameInfo.NameText);
                    case ErrorType.SetValueExceptionError:
                        var setValueError = (SetValueExceptionError)error;
                        return string.Format(Shared.Localization.Messages.ErrorSettingValueToOption01,
                            setValueError.NameInfo.NameText, setValueError.Exception.Message);
                }

                throw new InvalidOperationException();
            };
        }
    }

    /// <inheritdoc />
    public override Func<IEnumerable<MutuallyExclusiveSetError>, string> FormatMutuallyExclusiveSetErrors
    {
        get
        {
            return errors =>
            {
                var bySet = (from e in errors
                    group e by e.SetName
                    into g
                    select new { SetName = g.Key, Errors = g.ToList(), }).ToList();

                var messages = bySet.Select(
                    set =>
                    {
                        var names = string.Join(
                            string.Empty,
                            (from e in set.Errors select $"'{e.NameInfo.NameText}', ").ToArray());

                        var incompatible = string.Join(
                            string.Empty,
                            (from x in
                                    (from s in bySet where !s.SetName.Equals(set.SetName) from e in s.Errors select e)
                                    .Distinct()
                                select $"'{x.NameInfo.NameText}', ").ToArray());
                        return
                            string.Format(Shared.Localization.Messages.Options0AreNotCompatibleWith1,
                                names.Substring(0, names.Length - 2), incompatible[..^2]);
                    }).ToArray();
                return string.Join(Environment.NewLine, messages);
            };
        }
    }
}