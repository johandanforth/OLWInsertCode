using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

namespace RtfToHtml {
  public class RtfParser {
    public static string ParseRtf(string rtfValue) {
      ParserState state = new ParserState(rtfValue);
      IParseResult result = ParseRtf(state);
      if (result.Failed)
        return "***FAILED***";
      string html = result.Value;
      if (state.InBackgroundSpan)
        html += "</span>";
      if (state.InFontColorSpan)
        html += "</span>";
      return html;
    }

    private static IParseResult ParseRtf(ParserState state) {
      if (state.Current != '{') return FailedParse.Get;
      state.Advance();
      IParseResult result = ParseHeader(state);
      if (result.Failed) return result;
      result = ParseDocument(state);
      if (result.Failed) return result;
      if (state.Current != '}') return FailedParse.Get;
      state.Advance();
      return result;
    }

    private static IParseResult ParseDocument(ParserState state) {
      StringBuilder sb = new StringBuilder();
      while (state.Current != '}') {
        if (state.Current == '\\') {
          IParseResult result = ParseDocEscapedChar(state);
          if (result.Succeeded) {
            sb.Append(ConvertEntity(state.Current));
            state.Advance();
          }
          else {
            result = ParseKeyword(state);
            if (result.Failed) return result;
            string s = ProcessDocKeyword(result.Value, state);
            sb.Append(s);
          }
        }
        else {
          sb.Append(ConvertEntity(state.Current));
          state.Advance();
        }
      }
      return new SuccessfulParse(sb.ToString());
    }

    public static string ConvertEntity(char current) {
      switch (current) {
        case '&': return "&amp;";
        case '<': return "&lt;";
        case '>': return "&gt;";
        default:
          return current.ToString();
      }
    }

    private static IParseResult ParseDocEscapedChar(ParserState state) {
      state.Advance();
      if (char.IsLetterOrDigit(state.Current))
        return FailedParse.Get;

      return new SuccessfulParse(state.Current.ToString());
    }

    private static string ProcessDocKeyword(string keyword, ParserState state) {
      int colorIndex;
      Color color;
      string format;

      Regex regex = new Regex(@"([a-z]+)(\d*)");
      Match match = regex.Match(keyword);
      string keywordBase = match.Groups[1].Value;
      switch (keywordBase) {
        case "cf":
          colorIndex = int.Parse(match.Groups[2].Value);
          if (colorIndex == 0) {
            state.InFontColorSpan = false;
            return "</span>";
          }
          color = state.ColorTable[colorIndex];
          format = state.InFontColorSpan ? "</span>" : "";
          format += "<span style=\"color: #{0:x2}{1:x2}{2:x2};\">";
          state.InFontColorSpan = true;
          return String.Format(format, color.R, color.G, color.B);
        case "cb":
          colorIndex = int.Parse(match.Groups[2].Value);
          if (colorIndex == 0) {
            state.InBackgroundSpan = false;
            return "</span>";
          }
          color = state.ColorTable[colorIndex];
          format = state.InBackgroundSpan ? "</span>" : "";
          format += "<span style=\"background-color: #{0:x2}{1:x2}{2:x2};\">";
          state.InBackgroundSpan = true;
          return String.Format(format, color.R, color.G, color.B);
        case "par":
          return Environment.NewLine;
        default:
          break;
      }
      return String.Empty;
    }

    private static IParseResult ParseColors(ParserState state) {
      do {
        StringBuilder sb = new StringBuilder();
        while (state.Current != ';') {
          sb.Append(state.Current);
          state.Advance();
        }
        state.ColorTable.Add(sb.ToString());
        state.Advance();
      } while (state.Current != '}');
      return new SuccessfulParse("");
    }

    private static IParseResult ParseHeader(ParserState state) {
      IParseResult result;
      do {
        result = ParseHeaderKeyword(state);
        if (result.Failed) return result;
      } while (state.Current != '{');
      do {
        result = ParseHeaderGroup(state);
        if (result.Failed) return result;
      } while (state.Current != '\\');
      return result;
    }

    private static IParseResult ParseKeyword(ParserState state) {
      StringBuilder sb = new StringBuilder();
      do {
        sb.Append(state.Current);
        state.Advance();
      } while (char.IsLetterOrDigit(state.Current));
      if (state.Current == ' ')
        state.Advance();
      return new SuccessfulParse(sb.ToString());
    }

    private static IParseResult ParseHeaderKeyword(ParserState state) {
      if (state.Current != '\\') return FailedParse.Get;
      state.Advance();

      return ParseKeyword(state);
    }

    private static IParseResult ParseHeaderGroupData(ParserState state) {
      StringBuilder sb = new StringBuilder();
      while (state.Current != '}') {
        if (state.Current == '{') {
          IParseResult result = ParseHeaderGroup(state);
          if (result.Failed)
            return result;
          sb.Append(result.Value);
        }
        else {
          sb.Append(state.Current);
          state.Advance();
        }
      }
      return new SuccessfulParse(sb.ToString());
    }

    private static IParseResult ParseHeaderGroup(ParserState state) {
      if (state.Current != '{') return FailedParse.Get;
      state.Advance();
      IParseResult result = ParseHeaderKeyword(state);
      if (result.Failed) return result;
      if (result.Value == "colortbl") {
        result = ParseColors(state);
      }
      else {
        result = ParseHeaderGroupData(state);
      }
      if (state.Current != '}') return FailedParse.Get;
      state.Advance();
      return result;
    }

  }
}
