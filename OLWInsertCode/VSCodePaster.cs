using System;
using System.Windows.Forms;
using OpenLiveWriter.Api;
using RtfToHtml;

namespace OLWInsertCode {

  [WriterPlugin("9DD2F859-25C5-4236-B022-C34335D37360", "PasteCodeFromVS")]
  [InsertableContentSource("Paste Code From VS")]
  public class VSCodePaster : ContentSource {

    public override DialogResult CreateContent(IWin32Window dialogOwner, ref string content) {
      DialogResult result = DialogResult.OK;

      if (Clipboard.ContainsText(TextDataFormat.Rtf)) {
        string s = Clipboard.GetText(TextDataFormat.Rtf);
        string html = RtfParser.ParseRtf(s);
        content = String.Format("<div class=\"jmbcodeblock\">{0}<pre>{1}{0}</pre>{0}</div>{0}", Environment.NewLine, html);
      }
      else {
        content = "Clipboard is empty of RTF text. Did you copy some code from Visual Studio?";
      }
      return result;
    }
  }
}
