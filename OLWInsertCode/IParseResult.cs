using System;

namespace RtfToHtml {
  // The result of a parse operation
  public interface IParseResult {
    bool Succeeded { get; }
    bool Failed { get; }
    string Value { get; }
  }
}
