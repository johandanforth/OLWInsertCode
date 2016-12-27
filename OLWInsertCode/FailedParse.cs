using System;

namespace RtfToHtml {
  public class FailedParse : IParseResult {
    private readonly static IParseResult instance = new FailedParse();
    private FailedParse() { }
    public bool Succeeded {
      get { return false; }
    }
    public bool Failed {
      get { return !Succeeded; }
    }
    public string Value {
      get { return null; }
    }
    public static IParseResult Get {
      get { return instance; }
    }
  }
}
