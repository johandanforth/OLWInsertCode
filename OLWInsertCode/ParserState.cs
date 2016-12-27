using System;

namespace RtfToHtml {
  public class ParserState {
    private readonly string rtfValue;
    private int position;

    public ParserState(string rtfValue) {
      this.rtfValue = rtfValue;
      position = -1;
      Advance();
      ColorTable = new ColorTable();
    }

    public void Advance() {
      do {
        position++;
        if (position >= rtfValue.Length) {
          Current = '\0';
          return;
        }
      } while (rtfValue[position] == '\r' || rtfValue[position] == '\n');
      Current = rtfValue[position];
    }

    public char Current { get; private set; }
    public ColorTable ColorTable { get; private set; }
    public bool InFontColorSpan { get; set; }
    public bool InBackgroundSpan { get; set; }
  }
}
