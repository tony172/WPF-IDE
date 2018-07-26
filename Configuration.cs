using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ide
{
    [Serializable]
    public class Configuration : ISerializable
    {
        public string DefaultForeColor { get; set; }
        public string CommentForeColor { get; set; }
        public string CommentLineForeColor { get; set; }
        public string CommentLineDocForeColor { get; set; }
        public string NumberForeColor { get; set; }
        public string WordForeColor { get; set; }
        public string Word2ForeColor { get; set; }
        public string StringForeColor { get; set; }
        public string CharacterForeColor { get; set; }
        public string VerbatimForeColor { get; set; }
        public string StringEolBackColor { get; set; }
        public string OperatorForeColor { get; set; }
        public string PreprocessorForeColor { get; set; }
        public int marginWidth { get; set; }
        public string font { get; set; }
        public int fontSize { get; set; }
        public string compilerPath { get; set; }

       

        public Configuration()
        {
            DefaultForeColor = ColorTranslator.ToHtml( Color.Silver);
            CommentForeColor = ColorTranslator.ToHtml( Color.FromArgb(0, 128, 0));
            CommentLineForeColor = ColorTranslator.ToHtml(Color.FromArgb(0, 128, 0));
            CommentLineDocForeColor = ColorTranslator.ToHtml(Color.FromArgb(128, 128, 128));
            NumberForeColor = ColorTranslator.ToHtml(Color.Olive);
            WordForeColor = ColorTranslator.ToHtml(Color.Blue);
            Word2ForeColor = ColorTranslator.ToHtml(Color.Blue);
            StringForeColor = ColorTranslator.ToHtml(Color.FromArgb(163, 21, 21));
            CharacterForeColor = ColorTranslator.ToHtml(Color.FromArgb(163, 21, 21));
            VerbatimForeColor = ColorTranslator.ToHtml(Color.FromArgb(163, 21, 21));
            StringEolBackColor = ColorTranslator.ToHtml(Color.Pink);
            OperatorForeColor = ColorTranslator.ToHtml(Color.Purple);
            PreprocessorForeColor = ColorTranslator.ToHtml(Color.Maroon);
            marginWidth = 40;
            font = "Consolas";
            fontSize = 15;
        }

        public Configuration(SerializationInfo info, StreamingContext content)
        {
            
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            
        }

        public void Configure(ScintillaNET.WPF.ScintillaWPF scintilla)
        {
            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = font;
            scintilla.Styles[Style.Default].Size = fontSize;
            scintilla.StyleClearAll();

            scintilla.Styles[Style.Cpp.Default].ForeColor = ColorTranslator.FromHtml(DefaultForeColor);
            scintilla.Styles[Style.Cpp.Comment].ForeColor = ColorTranslator.FromHtml(CommentForeColor); // Green
            scintilla.Styles[Style.Cpp.CommentLine].ForeColor = ColorTranslator.FromHtml(CommentLineForeColor); // Green
            scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = ColorTranslator.FromHtml(CommentLineDocForeColor); // Gray
            scintilla.Styles[Style.Cpp.Number].ForeColor = ColorTranslator.FromHtml(NumberForeColor);
            scintilla.Styles[Style.Cpp.Word].ForeColor = ColorTranslator.FromHtml(WordForeColor);
            scintilla.Styles[Style.Cpp.Word2].ForeColor = ColorTranslator.FromHtml(Word2ForeColor);
            scintilla.Styles[Style.Cpp.String].ForeColor = ColorTranslator.FromHtml(StringForeColor); // Red
            scintilla.Styles[Style.Cpp.Character].ForeColor = ColorTranslator.FromHtml(CharacterForeColor); // Red
            scintilla.Styles[Style.Cpp.Verbatim].ForeColor = ColorTranslator.FromHtml(VerbatimForeColor); // Red
            scintilla.Styles[Style.Cpp.StringEol].BackColor = ColorTranslator.FromHtml(StringEolBackColor);
            scintilla.Styles[Style.Cpp.Operator].ForeColor = ColorTranslator.FromHtml(OperatorForeColor);
            scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = ColorTranslator.FromHtml(PreprocessorForeColor);
            
            scintilla.Lexer = Lexer.Cpp;
            scintilla.Margins[0].Width = marginWidth;

            scintilla.SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
            scintilla.SetKeywords(1, "bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void");
        }
    }
}
