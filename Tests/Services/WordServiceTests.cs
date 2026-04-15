using Project.Services;

namespace Project.Tests.Services;

public class WordServiceTests
{
    private readonly WordService _sut = new();

    // ── WordCount ────────────────────────────────────────────────────────────

    [Fact]
    public void WordCount_ExactMatch_ReturnsCorrectCount()
    {
        int result = _sut.WordCount("the", "the cat sat on the mat");
        Assert.Equal(2, result);
    }

    [Fact]
    public void WordCount_CaseInsensitive()
    {
        int result = _sut.WordCount("The", "THE cat sat on the mat");
        Assert.Equal(2, result);
    }

    [Fact]
    public void WordCount_EmbeddedInOtherWords_IsNotCounted()
    {
        // "type" as a whole word appears only once; "types" and "typedef" are not matches
        int result = _sut.WordCount("type", "types typedef type");
        Assert.Equal(1, result);
    }

    [Fact]
    public void WordCount_WordNotPresent_ReturnsZero()
    {
        int result = _sut.WordCount("xyz", "hello world");
        Assert.Equal(0, result);
    }

    [Fact]
    public void WordCount_EmptySearchWord_ReturnsZero()
    {
        int result = _sut.WordCount("", "hello world");
        Assert.Equal(0, result);
    }

    [Fact]
    public void WordCount_EmptyText_ReturnsZero()
    {
        int result = _sut.WordCount("hello", "");
        Assert.Equal(0, result);
    }

    [Fact]
    public void WordCount_BothEmpty_ReturnsZero()
    {
        int result = _sut.WordCount("", "");
        Assert.Equal(0, result);
    }

    [Fact]
    public void WordCount_SingleCharWord_CountsCorrectly()
    {
        int result = _sut.WordCount("a", "a cat sat on a mat");
        Assert.Equal(2, result);  // whole-word "a" appears twice; "cat", "sat", "mat" are not matches
    }

    [Fact]
    public void WordCount_RepeatedWord_CountsAll()
    {
        int result = _sut.WordCount("go", "go go go go go go go go go go");
        Assert.Equal(10, result);
    }

    // ── InspectWordCount ─────────────────────────────────────────────────────

    [Fact]
    public void InspectWordCount_Zero_ReturnsNull()
    {
        bool? result = _sut.InspectWordCount(0);
        Assert.Null(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(9)]
    public void InspectWordCount_BetweenOneAndNine_ReturnsFalse(int count)
    {
        bool? result = _sut.InspectWordCount(count);
        Assert.Equal(false, result);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(11)]
    [InlineData(100)]
    public void InspectWordCount_TenOrMore_ReturnsTrue(int count)
    {
        bool? result = _sut.InspectWordCount(count);
        Assert.Equal(true, result);
    }
}
