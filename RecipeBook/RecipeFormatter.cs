using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
/*  Copyright (C) 2015 Ben Aherne

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/
namespace RecipeBook
{
    /// <summary>
    /// RecipeFormatter is the class responsible formatting text
    /// and adding it to 
    /// </summary>
    class RecipeFormatter : Recipe
    {
        public RecipeFormatter(){}
#region appearance fields
        private const int TitleFontSize = 38;
        private const string TitleFont = "Segoe UI";
        private const int DateFontSize = 18;
        private const string DateFont = "Segoe UI";
        private const int PromptFontSize = 20;
        private const string PromptFont = "Segoe UI";
        private const string TextFont = "Segoe UI";
        private readonly FontWeight _titleFontWeight = FontWeights.SemiBold;
        private readonly FontStyle _titleFontStyle = default(FontStyle);
        private readonly Brush _titleBrush = Brushes.DarkGoldenrod;
        private readonly FontWeight _dateFontWeight = FontWeights.SemiBold;
        private readonly FontStyle _dateFontStyle = default(FontStyle);
        private readonly Brush _dateBrush = Brushes.DarkGoldenrod;
        private readonly Brush _textBrush = Brushes.Cornsilk;
        private readonly FontWeight _promptFontWeight = FontWeights.Light;
        private readonly FontStyle _promptFontStyle = FontStyles.Italic;
        private readonly Brush _promptBrush = Brushes.Cornsilk;
        private readonly FontWeight _textFontWeight = FontWeights.Normal;
        private readonly FontStyle _textFontStyle = default(FontStyle);
        private const int TextFontSize = 20;
#endregion

        /// <summary>
        /// Adds a formatted web url to a HyperLink object.
        /// </summary>
        /// <param name="hypLink">The HyperLink object to add a url to.</param>
        /// <param name="url">The url to be added to the HyperLink obect.</param>
        public void AddHyperLink(Hyperlink hypLink, string url)
        {
            hypLink.Inlines.Clear();
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                hypLink.NavigateUri = new Uri(url, UriKind.RelativeOrAbsolute);
                hypLink.Inlines.Add(url);
            }
            else
            {
                hypLink.Inlines.Add("Provided url is unsupported. Please ensure urls start with http://");
            }

        }
        /// <summary>
        /// Adds a formatted string to a provided RichTextBox object.
        /// </summary>
        /// <param name="box">The RichTextBox object to add a string to.</param>
        /// <param name="content">The string to add to the RichTextBox.</param>
        /// <param name="section">Specifies which section of the window the RichTextBox should be styled as.</param>
        /// <param name="isSeparated">Optional parameter to add a seperator under the RichTextBoxs content.</param>
        public void AddContent(RichTextBox box, string content, DocumentSection section, bool isSeparated = false)
        {
            box.Document.Blocks.Clear();    
            Paragraph text = MakeParagraphFromString(content, section);
            if (section == DocumentSection.Date || section == DocumentSection.Title)
            {
                text.LineHeight = 5;
            }
            box.Document.Blocks.Add(text);
            if (isSeparated)
            {
                box.Document.Blocks.Add(SeperatorLine());
            }
        }
        /// <summary>
        /// Adds a set of formatted paragraphs to a provided RichTextBox object from an ArrayList of strings.
        /// </summary>
        /// <param name="box">The RichTextBox object to add a string to.</param>
        /// <param name="content">The ArrayList to parse into paragraphs.</param>
        /// <param name="section">Specifies which section of the window the RichTextBox should be styled as.</param>
        /// <param name="fontSizeRatio">Optional parameter, allows font size manipulation,</param>
        public void AddContent(RichTextBox box, ArrayList content, DocumentSection section, double fontSizeRatio = 1)
        {
            box.Document.Blocks.Clear();
            List list = MakeListItems(content, DocumentSection.Text, fontSizeRatio);
            box.Document.Blocks.Add(list);
        }
        private Paragraph SeperatorLine()
        {
            Paragraph separator = new Paragraph();
            separator.Inlines.Add(new Line()
            {
                Stretch = Stretch.Fill,
                Stroke = Brushes.Silver,
                X2 = 1,
                StrokeDashArray = { 2, 2 },
            });
            separator.LineHeight = TextFontSize * 1.6;
            return separator;
        }
        private Paragraph MakeParagraphFromString(String text, DocumentSection section)
        {
            Paragraph paragraph = new Paragraph()
            {
                FontSize = GetDataForDocumentSection(section, TextSection.Size),
                Foreground = GetDataForDocumentSection(section, TextSection.Brush),
                FontStyle = GetDataForDocumentSection(section, TextSection.Style),
                FontFamily = new FontFamily(GetDataForDocumentSection(section, TextSection.TypeFace)),
                FontWeight = GetDataForDocumentSection(section, TextSection.Weight),
                LineHeight = GetDataForDocumentSection(section, TextSection.Size) * 1.6,
            };
            paragraph.Inlines.Add(new Run(text.Trim()));
            return paragraph;
        }

        private List MakeListItems(ArrayList list, DocumentSection section, double fontSizeRatio = 1)
        {
            List paragraphList = new List
            {
                MarkerOffset = 1,
                StartIndex = 1,
                MarkerStyle = TextMarkerStyle.Decimal,
                Padding = new Thickness(15)
            };
            foreach (string line in list)
            {
                if (!(line == "" || line == "\\n"))
                {
                    Paragraph paragraph = new Paragraph()
                    {
                        FontSize = GetDataForDocumentSection(section, TextSection.Size)*fontSizeRatio,
                        Foreground = GetDataForDocumentSection(section, TextSection.Brush),
                        FontStyle = GetDataForDocumentSection(section, TextSection.Style),
                        FontFamily = new FontFamily(GetDataForDocumentSection(section, TextSection.TypeFace)),
                        FontWeight = GetDataForDocumentSection(section, TextSection.Weight),
                        Margin = new Thickness(TextFontSize*fontSizeRatio)
                    };
                    paragraph.Inlines.Add(new Run(line.Trim()));
                    paragraphList.ListItems.Add(new ListItem(paragraph));
                }
            }
            return paragraphList;
        }

        private dynamic GetTitleData(TextSection section)
        {
            switch (section)
            {
                case TextSection.Size:
                    return TitleFontSize;
                case TextSection.TypeFace:
                    return TitleFont;
                case TextSection.Brush:
                    return _titleBrush;
                case TextSection.Style:
                    return _titleFontStyle;
                case TextSection.Weight:
                    return _titleFontWeight;
            }
            throw new NoSectionTypeException();
        }
        private dynamic GetDateData(TextSection section)
        {
            switch (section)
            {
                case TextSection.Size:
                    return DateFontSize;
                case TextSection.TypeFace:
                    return DateFont;
                case TextSection.Brush:
                    return _dateBrush;
                case TextSection.Style:
                    return _dateFontStyle;
                case TextSection.Weight:
                    return _dateFontWeight;
            }
            throw new NoSectionTypeException();
        }

        private dynamic GetPromptData(TextSection section)
        {
            switch (section)
            {
                case TextSection.Size:
                    return PromptFontSize;
                case TextSection.TypeFace:
                    return PromptFont;
                case TextSection.Brush:
                    return _promptBrush;
                case TextSection.Style:
                    return _promptFontStyle;
                case TextSection.Weight:
                    return _promptFontWeight;
            }
            throw new NoSectionTypeException();
        }
        private dynamic GetTextData(TextSection section)
        {
            switch (section)
            {
                case TextSection.Size:
                    return TextFontSize;
                case TextSection.TypeFace:
                    return TextFont;
                case TextSection.Brush:
                    return _textBrush;
                case TextSection.Style:
                    return _textFontStyle;
                case TextSection.Weight:
                    return _textFontWeight;
            }
            throw new NoSectionTypeException();
        }

        private dynamic GetDataForDocumentSection(DocumentSection docSection, TextSection textSection )
        {
            switch (docSection)
            {
                case DocumentSection.Title:
                    return GetTitleData(textSection);
                case DocumentSection.Date:
                    return GetDateData(textSection);
                case DocumentSection.Prompt:
                    return GetPromptData(textSection);
                case DocumentSection.Text:
                    return GetTextData(textSection);
            }
            throw new NoSectionTypeException();
        }
    }
    /// <summary>
    /// Specifies the aspects of text that can be formatted.
    /// </summary>
    public enum TextSection
    {
        Size, Brush,
        TypeFace, Weight,
        Style,
    }
    /// <summary>
    /// Specifies the sections of the window available for text editing.
    /// </summary>
    public enum DocumentSection
    {
        Title, Date,
        Prompt, Text
    }
}
