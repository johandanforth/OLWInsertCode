using System;

namespace RtfToHtml {
  public class SuccessfulParse : IParseResult {
    public SuccessfulParse(string value) {
      Value = value;
    }
    public bool Succeeded {
      get { return true; }
    }
    public bool Failed {
      get { return !Succeeded; }
    }
    public string Value { get; private set; }
  }
}
