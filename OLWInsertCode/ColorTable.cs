using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace RtfToHtml {
  public class ColorTable {
    readonly List<Color> list = new List<Color>();

    public void Add(string color) {
      if (string.IsNullOrEmpty(color))
        Add(Color.Black);
      else {
        Regex regex = new Regex(@"\\red(\d+)\\green(\d+)\\blue(\d+)");
        Match match = regex.Match(color);
        if (!match.Success) throw new Exception("Invalid color item in color table");
        Add(Color.FromArgb(int.Parse(match.Groups[1].Value),
                           int.Parse(match.Groups[2].Value),
                           int.Parse(match.Groups[3].Value)));
      }
    }

    public void Add(Color color) {
      list.Add(color);
    }

    public Color this[int index] {
      get { return list[index]; }
    }
  }
}
