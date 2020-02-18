using System.Collections.Generic;
using System;
public class Parser : IParser
{
    private Dictionary<char, Func<string, string>> wrappers =
    new Dictionary<char, Func<string, string>>();

    private Dictionary<char, char> parents =
    new Dictionary<char, char>();

    public Parser()
    {
        parents.Add('*', '+');
        wrappers.Add('+', x => string.Format("<ul>{0}</ul>", x));
        wrappers.Add('*', x => string.Format("<li>{0}</li>", x));
        wrappers.Add('#', x => string.Format("<h1>{0}</h1>", x));
        wrappers.Add('P', x => string.Format("<p>{0}</p>", x));
    }
    public string Parse(string original)
    {
        char [] separator = { '|' };
        string [] splitted = original.Split(separator);
        string result = "";
        string child = "";
        string lastParentCmd = "";
        bool isMultiWrap = false;
        bool hasOpenParent = false;
        foreach(string content in splitted)
        {
            if(content.Length <= 0) continue;

            char cmd = content[0];
            cmd = char.IsUpper(cmd) ? 'P' : cmd;
            isMultiWrap = cmd.Equals('*');
            if(isMultiWrap)
            {
                hasOpenParent = true;
                lastParentCmd += cmd;
                var childWrap = wrappers[cmd];
                child += childWrap(RemoveMarkup(cmd, content));
                continue;
            } else if(hasOpenParent) {
                hasOpenParent = false;
                char parentCmd = parents[lastParentCmd[0]];
                var parentWrap = wrappers[parentCmd];
                result += parentWrap(child);
                lastParentCmd = String.Empty;
            }
            var wrap = wrappers[cmd];
            result += wrap(RemoveMarkup(cmd, content));
        }
        if(hasOpenParent) {
            hasOpenParent = false;
            char parentCmd = parents[lastParentCmd[0]];
            var parentWrap = wrappers[parentCmd];
            result += parentWrap(child);
            lastParentCmd = String.Empty;
        }
        return result;
    }

    private string RemoveMarkup(char markup, string content)
    {
        if(String.IsNullOrWhiteSpace(content))
        {
            return "";
        }
        return content.Split(markup)[1];
    }
}