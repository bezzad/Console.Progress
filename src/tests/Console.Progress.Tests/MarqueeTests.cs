﻿namespace Console.Progress.Tests;

using System;
using System.Collections.Generic;
using AutoFixture.Xunit2;
using Xunit;

public class MarqueeTests
{
    [Theory(DisplayName = "MarqueeFormat instantiates with the given values"), AutoData]
    public void MarqueeFormat_Instantiates_With_The_Given_Values(char empty, string complete, string left, string right, int paddingLeft, int paddingRight, char pad, bool bounce, bool reverseTextOnBounce, bool leftToRight, int? gap)
    {
#pragma warning disable IDE0039 // Use local function
        Func<bool> c = () => true;
        Func<bool> e = () => true;
        Func<bool> h = () => false;
#pragma warning restore IDE0039 // Use local function

        MarqueeFormat f = null;
        var ex = Record.Exception(() => f = new MarqueeFormat(empty, complete, left, right, paddingLeft, paddingRight, pad, bounce, reverseTextOnBounce, leftToRight, gap, c, e, h));

        Assert.Null(ex);

        Assert.Equal(empty, f.Empty);
        Assert.Equal(complete, f.Complete);
        Assert.Equal(left, f.Left);
        Assert.Equal(right, f.Right);
        Assert.Equal(paddingLeft, f.PaddingLeft);
        Assert.Equal(paddingRight, f.PaddingRight);
        Assert.Equal(pad, f.Pad);
        Assert.Equal(bounce, f.Bounce);
        Assert.Equal(reverseTextOnBounce, f.ReverseTextOnBounce);
        Assert.Equal(leftToRight, f.LeftToRight);
        Assert.Equal(gap, f.Gap);
        Assert.Equal(paddingLeft + paddingRight + left.Length + right.Length, f.Width);

        Assert.Equal(c, f.CompleteWhen);
        Assert.Equal(e, f.EmptyWhen);
        Assert.Equal(h, f.HiddenWhen);
    }

    [Theory(DisplayName = "Marquee instantiates with the given values"), AutoData]
    public void Marquee_Instantiates_With_The_Given_Values(string text, int width, MarqueeFormat format)
    {
        Marquee m = null;
        var ex = Record.Exception(() => m = new Marquee(text, width, format: format));

        Assert.Null(ex);

        Assert.Equal(text, m.Text);
        Assert.Equal(width, m.Width);
        Assert.Equal(format, m.Format);

        Assert.Equal(format.Gap, m.Gap);
        Assert.Equal(text.Length + width, m.Position);
        Assert.False(m.Reversed);
    }

    [Fact(DisplayName = "Marquee scrolls left")]
    public void Marquee_Scrolls_Left()
    {
        var m = new Marquee("a", 2);
        var s = new List<string>();

        for (int i = 0; i < 4; i++)
        {
            s.Add(m.ToString());
            m.Scroll();
        }

        Assert.Equal("  ", s[0]);
        Assert.Equal(" a", s[1]);
        Assert.Equal("a ", s[2]);
        Assert.Equal("  ", s[3]);
    }

    [Fact(DisplayName = "Marquee scrolls right")]
    public void Marquee_Scrolls_Right()
    {
        var m = new Marquee("a", 2, format: new MarqueeFormat(leftToRight: true));
        var s = new List<string>();

        for (int i = 0; i < 4; i++)
        {
            s.Add(m.ToString());
            m.Scroll();
        }

        Assert.Equal("  ", s[0]);
        Assert.Equal("a ", s[1]);
        Assert.Equal(" a", s[2]);
        Assert.Equal("  ", s[3]);
    }

    [Fact(DisplayName = "Marquee bounces")]
    public void Marquee_Bounces()
    {
        var m = new Marquee("ab", 2, format: new MarqueeFormat(bounce: true));
        var s = new List<string>();

        for (int i = 0; i < 9; i++)
        {
            s.Add(m.ToString());
            m.Scroll();
        }

        Assert.Equal("  ", s[0]);
        Assert.Equal(" a", s[1]);
        Assert.Equal("ab", s[2]);
        Assert.Equal("b ", s[3]);
        Assert.Equal("  ", s[4]);
        Assert.Equal("b ", s[5]);
        Assert.Equal("ab", s[6]);
        Assert.Equal(" a", s[7]);
        Assert.Equal("  ", s[8]);
    }

    [Fact(DisplayName = "Marquee reverses on bounce")]
    public void Marquee_Reverses_On_Bounce()
    {
        var m = new Marquee("ab", 2, format: new MarqueeFormat(bounce: true, reverseTextOnBounce: true));
        var s = new List<string>();

        for (int i = 0; i < 9; i++)
        {
            s.Add(m.ToString());
            m.Scroll();
        }

        Assert.Equal("  ", s[0]);
        Assert.Equal(" a", s[1]);
        Assert.Equal("ab", s[2]);
        Assert.Equal("b ", s[3]);
        Assert.Equal("  ", s[4]);
        Assert.Equal("a ", s[5]);
        Assert.Equal("ba", s[6]);
        Assert.Equal(" b", s[7]);
        Assert.Equal("  ", s[8]);
    }

    [Fact(DisplayName = "Marquee returns zero length string when hidden")]
    public void Marquee_Returns_Zero_Length_String_When_Hidden()
    {
        var hidden = false;
        var m = new Marquee("Hello, World!", 10, format: new MarqueeFormat(hiddenWhen: () => hidden));

        var s1 = m.ToString();

        hidden = true;
        var s2 = m.ToString();

        Assert.Equal(10, s1.Length);
        Assert.Equal(0, s2.Length);
    }

    [Fact(DisplayName = "Marquee returns empty string when empty")]
    public void Marquee_Returns_Empty_String_When_Empty()
    {
        var empty = false;
        var m = new Marquee("Hello, World!", 10, format: new MarqueeFormat(empty: '.', emptyWhen: () => empty));

        for (int i = 0; i < 5; i++)
        {
            m.Scroll();
        }

        var s1 = m.ToString();

        empty = true;
        var s2 = m.ToString();

        Assert.Equal(".....Hello", s1);
        Assert.Equal("..........", s2);
    }

    [Fact(DisplayName = "Marquee returns complete string when completed")]
    public void Marquee_Returns_Complete_String_When_Complete()
    {
        var complete = false;
        var m = new Marquee("Hello, World!", 10, format: new MarqueeFormat(empty: '.', complete: "Done!", completeWhen: () => complete));

        for (int i = 0; i < 5; i++)
        {
            m.Scroll();
        }

        var s1 = m.ToString();

        complete = true;
        var s2 = m.ToString();

        Assert.Equal(".....Hello", s1);
        Assert.Equal("Done!.....", s2);
    }

    [Fact(DisplayName = "Marquee adds formatting")]
    public void Marquee_Adds_Formating()
    {
        var m = new Marquee(" ", 1, format: new MarqueeFormat(left: ".", right: "!", paddingLeft: 3, paddingRight: 2, pad: ','));

        Assert.Equal(",,,. !,,", m.ToString());
    }

    [Fact(DisplayName = "Marquee respects gap setting")]
    public void Marquee_Respects_Gap_Setting()
    {
        var m = new Marquee("a", 6, format: new MarqueeFormat(gap: 1));

        Assert.Equal(" a a a", m.ToString());
    }

    [Fact(DisplayName = "Marquee scrolls on tostring when set")]
    public void Marquee_Scrolls_On_ToString_When_Set()
    {
        var m = new Marquee("a", 2, scrollOnToString: true);

        var s1 = m.ToString();
        var s2 = m.ToString();

        Assert.Equal(" a", s1);
        Assert.Equal("a ", s2);
    }
}
