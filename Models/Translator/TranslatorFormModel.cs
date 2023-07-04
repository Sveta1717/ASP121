namespace ASP121.Models.Translator
{
    public class TranslatorFormModel
    {
        public String?  LangFrom        { get; set; }
        public String?  LangTo          { get; set; }
        public String?  OriginalText    { get; set; }
        public String?  TranslatedText  { get; set; }
        public String?  TranslateButton { get; set; }
        public String?  InverseButton   { get; set; }
    }
}
