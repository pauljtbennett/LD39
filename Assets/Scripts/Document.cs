using System;

namespace LD39
{
    public enum DocumentType
    {
        JPG,
        PNG,
        BMP,
        GIF,
        DOC,
        PDF,
        TXT,
        WAV,
        MP3,
        AVI,
        EXE
    }

    public class Document
    {
        public string name { get; private set; }
        public DocumentType type { get; private set; }
        public float size { get; private set; }

        // WritePad specific
        public string writingText;
        public int contentLength;

        // Scribbler specific
        public int rotation;
        public bool inverted;
        public string imagePath;

        public Document(string name, DocumentType type)
        {
            DateTime now = DateTime.Now;
            name = name + " " + now.ToLongTimeString().Replace(":", "_");
            name = name.Replace(" ", "_").ToUpper();
            name += "." + type.ToString();

            this.name = name;
            this.type = type;
        }
    }
}